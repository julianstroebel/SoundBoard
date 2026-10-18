using System.Diagnostics;
using System.Globalization;
using System.Text;
using Windows.Media.SpeechSynthesis;

namespace SoundBoard
{
    /// <summary>
    ///  Eine Stimme, die Text offline in WAV-Daten umwandelt.
    /// </summary>
    internal interface ITtsVoice
    {
        string Id { get; }
        string DisplayName { get; }
        Task<byte[]> SynthesizeAsync(string text, CancellationToken ct);
    }

    internal static class TtsService
    {
        /// <summary>
        ///  Ordner für Piper (piper.exe + de_*.onnx Stimmen).
        /// </summary>
        public static string PiperFolder => Path.Combine(AppConfig.Folder, "piper");

        /// <summary>
        ///  Alle deutschen Stimmen, die natürlichsten (Piper) zuerst.
        /// </summary>
        public static List<ITtsVoice> GetVoices()
        {
            var voices = new List<ITtsVoice>();

            string[] piperDirs = [Path.Combine(AppContext.BaseDirectory, "piper"), PiperFolder];
            foreach (string dir in piperDirs.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                string exe = Path.Combine(dir, "piper.exe");
                if (!File.Exists(exe))
                {
                    continue;
                }

                foreach (string model in Directory.EnumerateFiles(dir, "de_*.onnx", SearchOption.AllDirectories))
                {
                    // Dieselbe Stimme kann neben der exe und in AppData liegen; die erste gewinnt
                    string id = "piper:" + Path.GetFileNameWithoutExtension(model);
                    if (File.Exists(model + ".json") && !voices.Any(v => v.Id == id))
                    {
                        voices.Add(new PiperVoice(exe, model));
                    }
                }
            }

            try
            {
                List<VoiceInformation> windows = SpeechSynthesizer.AllVoices
                    .Where(v => v.Language.StartsWith("de", StringComparison.OrdinalIgnoreCase))
                    .ToList();
                if (windows.Count == 0)
                {
                    windows = SpeechSynthesizer.AllVoices.ToList();
                }

                voices.AddRange(windows
                    .OrderBy(v => v.DisplayName.Contains("Katja") ? 0 : v.DisplayName.Contains("Stefan") ? 1 : 2)
                    .Select(v => new WindowsVoice(v)));
            }
            catch (Exception)
            {
                // Keine Windows-Sprachausgabe verfügbar
            }

            return voices;
        }
    }

    /// <summary>
    ///  Windows-Sprachausgabe (OneCore), läuft komplett offline.
    /// </summary>
    internal sealed class WindowsVoice(VoiceInformation info) : ITtsVoice
    {
        public string Id => "win:" + info.Id;
        public string DisplayName => info.DisplayName.Replace("Microsoft ", "") + " · Windows";

        public async Task<byte[]> SynthesizeAsync(string text, CancellationToken ct)
        {
            using var synth = new SpeechSynthesizer { Voice = info };
            using SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(text).AsTask(ct);
            using var buffer = new MemoryStream();
            await stream.AsStreamForRead().CopyToAsync(buffer, ct);
            return buffer.ToArray();
        }
    }

    /// <summary>
    ///  Piper: neuronale Offline-Stimmen, klingen deutlich natürlicher als die Windows-Stimmen.
    /// </summary>
    internal sealed class PiperVoice(string exe, string model) : ITtsVoice
    {
        public string Id => "piper:" + Path.GetFileNameWithoutExtension(model);

        public string DisplayName
        {
            get
            {
                // z.B. de_DE-thorsten-high
                string[] parts = Path.GetFileNameWithoutExtension(model).Split('-');
                if (parts.Length < 3)
                {
                    return parts[0] + " · Piper";
                }
                string name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parts[1].Replace('_', ' '));
                return $"{name} ({parts[2]}) · Piper";
            }
        }

        public async Task<byte[]> SynthesizeAsync(string text, CancellationToken ct)
        {
            string file = Path.Combine(Path.GetTempPath(), $"soundboard_{Guid.NewGuid():N}.wav");
            var psi = new ProcessStartInfo(exe)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardInputEncoding = new UTF8Encoding(false),
                WorkingDirectory = Path.GetDirectoryName(exe)!
            };
            psi.ArgumentList.Add("--model");
            psi.ArgumentList.Add(model);
            psi.ArgumentList.Add("--output_file");
            psi.ArgumentList.Add(file);

            using Process process = Process.Start(psi) ?? throw new InvalidOperationException("Piper konnte nicht gestartet werden.");
            Task<string> error = process.StandardError.ReadToEndAsync(ct);
            Task<string> output = process.StandardOutput.ReadToEndAsync(ct);

            // Piper erzeugt pro Zeile eine eigene Ausgabe, daher alles in eine Zeile
            await process.StandardInput.WriteLineAsync(text.ReplaceLineEndings(" "));
            process.StandardInput.Close();

            try
            {
                await process.WaitForExitAsync(ct);
                await output;
                if (process.ExitCode != 0 || !File.Exists(file))
                {
                    throw new InvalidOperationException("Piper meldet einen Fehler:\n" + await error);
                }
                return await File.ReadAllBytesAsync(file, ct);
            }
            catch (OperationCanceledException)
            {
                try { process.Kill(); } catch (Exception) { }
                throw;
            }
            finally
            {
                try { File.Delete(file); } catch (Exception) { }
            }
        }
    }
}

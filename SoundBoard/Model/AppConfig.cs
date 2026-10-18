using System.Text.Json;
using System.Text.Json.Serialization;

namespace SoundBoard
{
    public enum ButtonAction
    {
        None,
        Audio,
        Tts
    }

    /// <summary>
    ///  Ein gespeicherter Text für Text-to-Speech.
    /// </summary>
    public sealed class SavedText
    {
        public string Name { get; set; } = "";
        public string Text { get; set; } = "";

        public override string ToString() => Name;
    }

    /// <summary>
    ///  Aktion und Darstellung eines der 16 Soundbuttons.
    /// </summary>
    public sealed class ButtonConfig
    {
        public const float DefaultFontSize = 12F;

        public ButtonAction Action { get; set; }
        public string? AudioFile { get; set; }
        public string? TtsText { get; set; }

        public string? Title { get; set; }
        public float FontSize { get; set; } = DefaultFontSize;
        public string? Color { get; set; }
        public string? IconGlyph { get; set; }
        public string? ImageFile { get; set; }

        public MidiTrigger? Midi { get; set; }

        public static string DefaultTitle(int id) => $"Button {id}";

        public static ButtonConfig CreateDefault(int id) => new() { Title = DefaultTitle(id) };

        public ButtonConfig Clone() => (ButtonConfig)MemberwiseClone();

        public Color GetColor()
        {
            if (string.IsNullOrEmpty(Color))
            {
                return System.Drawing.Color.Empty;
            }

            try
            {
                return ColorTranslator.FromHtml(Color);
            }
            catch (Exception)
            {
                return System.Drawing.Color.Empty;
            }
        }
    }

    /// <summary>
    ///  Buttons und gespeicherte Texte, abgelegt als JSON unter %AppData%\SoundBoard.
    /// </summary>
    public sealed class AppConfig
    {
        public const int ButtonCount = 16;

        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        public static string Folder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SoundBoard");
        private static string FilePath => Path.Combine(Folder, "config.json");
        private static string ImageFolder => Path.Combine(Folder, "images");

        public List<SavedText> Texts { get; set; } = [];
        public List<ButtonConfig> Buttons { get; set; } = [];

        /// <summary>
        ///  MIDI-Befehl für "Wiedergabe STOP".
        /// </summary>
        public MidiTrigger? StopMidi { get; set; }

        /// <summary>
        ///  Beschreibt, wo ein MIDI-Befehl bereits verwendet wird (z.B. "Button 3"), oder null.
        /// </summary>
        public string? FindMidiUsage(MidiTrigger trigger, int ignoreButtonId = 0, bool ignoreStop = false)
        {
            if (!ignoreStop && trigger == StopMidi)
            {
                return "Wiedergabe STOP";
            }
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (i + 1 != ignoreButtonId && trigger == Buttons[i].Midi)
                {
                    return $"Button {i + 1}";
                }
            }
            return null;
        }

        /// <summary>
        ///  Entfernt einen MIDI-Befehl überall außer an der angegebenen Stelle.
        /// </summary>
        public void ReleaseMidi(MidiTrigger trigger, int keepButtonId = 0, bool keepStop = false)
        {
            if (!keepStop && trigger == StopMidi)
            {
                StopMidi = null;
            }
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (i + 1 != keepButtonId && trigger == Buttons[i].Midi)
                {
                    Buttons[i].Midi = null;
                }
            }
        }

        public static AppConfig Load()
        {
            AppConfig? config = null;
            try
            {
                if (File.Exists(FilePath))
                {
                    config = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText(FilePath), jsonOptions);
                }
            }
            catch (Exception)
            {
                // Defekte Datei sichern und mit Standardwerten weitermachen
                try { File.Copy(FilePath, FilePath + ".defekt", true); } catch (Exception) { }
            }

            config ??= new AppConfig();
            config.Texts ??= [];
            config.Buttons ??= [];
            for (int i = config.Buttons.Count; i < ButtonCount; i++)
            {
                config.Buttons.Add(ButtonConfig.CreateDefault(i + 1));
            }
            return config;
        }

        public void Save()
        {
            Directory.CreateDirectory(Folder);
            string temp = FilePath + ".tmp";
            File.WriteAllText(temp, JsonSerializer.Serialize(this, jsonOptions));
            File.Move(temp, FilePath, true);
        }

        /// <summary>
        ///  Kopiert ein Bild in den Programmordner, damit der Button nicht vom Originalpfad abhängt.
        /// </summary>
        public static string ImportImage(string source)
        {
            Directory.CreateDirectory(ImageFolder);
            string target = Path.Combine(ImageFolder, Guid.NewGuid().ToString("N") + Path.GetExtension(source));
            File.Copy(source, target);
            return target;
        }

        public static bool IsImportedImage(string? path)
        {
            return !string.IsNullOrEmpty(path)
                && Path.GetFullPath(path).StartsWith(ImageFolder + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///  Lädt ein Bild ohne die Datei gesperrt zu halten. Null, wenn nicht vorhanden oder ungültig.
        /// </summary>
        public static Image? LoadImage(string? path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return null;
            }

            try
            {
                using var stream = new MemoryStream(File.ReadAllBytes(path));
                using Image image = Image.FromStream(stream);
                return new Bitmap(image);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

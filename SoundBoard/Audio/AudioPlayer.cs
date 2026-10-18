using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SoundBoard
{
    /// <summary>
    ///  Spielt Audio-Dateien und erzeugte WAV-Daten ab. Es läuft immer nur ein Sound gleichzeitig.
    /// </summary>
    internal sealed class AudioPlayer : IDisposable
    {
        private WaveOutEvent? output;
        private IDisposable? reader;
        private VolumeSampleProvider? volumeProvider;
        private float volume = 1f;

        /// <summary>
        ///  Wird ausgelöst, wenn die Wiedergabe endet oder gestoppt wird.
        /// </summary>
        public event EventHandler? PlaybackStopped;

        public float Volume
        {
            get => volume;
            set
            {
                volume = Math.Clamp(value, 0f, 1f);
                if (volumeProvider != null)
                {
                    volumeProvider.Volume = volume;
                }
            }
        }

        public void PlayFile(string path)
        {
            var fileReader = new AudioFileReader(path);
            start(fileReader, fileReader);
        }

        public void PlayWave(byte[] wav)
        {
            var waveReader = new WaveFileReader(new MemoryStream(wav));
            start(waveReader.ToSampleProvider(), waveReader);
        }

        public void Stop()
        {
            WaveOutEvent? current = output;
            if (current == null)
            {
                return;
            }

            output = null;
            current.Stop();
            current.Dispose();
            reader?.Dispose();
            reader = null;
            volumeProvider = null;
            PlaybackStopped?.Invoke(this, EventArgs.Empty);
        }

        private void start(ISampleProvider source, IDisposable owner)
        {
            Stop();

            var waveOut = new WaveOutEvent();
            try
            {
                volumeProvider = new VolumeSampleProvider(source) { Volume = volume };
                waveOut.Init(volumeProvider);
            }
            catch
            {
                waveOut.Dispose();
                owner.Dispose();
                volumeProvider = null;
                throw;
            }

            waveOut.PlaybackStopped += (s, e) =>
            {
                if (output == waveOut)
                {
                    Stop();
                }
            };

            output = waveOut;
            reader = owner;
            waveOut.Play();
        }

        public void Dispose()
        {
            PlaybackStopped = null;
            Stop();
        }
    }
}

using SoundBoard.Properties;

namespace SoundBoard
{
    /// <summary>
    ///  Programmeinstellungen: Stimme, Lautstärke, MIDI-Gerät und MIDI für "Wiedergabe STOP".
    ///  Änderungen wirken sofort.
    /// </summary>
    public partial class fSettings : Form
    {
        private readonly AppConfig config;
        private readonly AudioPlayer player;
        private readonly MidiService midi;
        private readonly Action saveConfig;
        private readonly Action updateLeds;
        private readonly Func<string> sendAllLeds;

        /// <summary>
        ///  Auswahl für die LED-Farbsteuerung.
        /// </summary>
        private sealed record LedModeItem(string Key, string Name)
        {
            public override string ToString() => Name;
        }

        /// <param name="updateLeds">Sendet geänderte LED-Farben an den Controller.</param>
        /// <param name="sendAllLeds">Sendet alle LED-Farben neu und beschreibt, was gesendet wurde.</param>
        internal fSettings(AppConfig config, IReadOnlyList<ITtsVoice> voices, AudioPlayer player, MidiService midi, Action saveConfig,
            Action updateLeds, Func<string> sendAllLeds)
        {
            InitializeComponent();

            this.config = config;
            this.player = player;
            this.midi = midi;
            this.saveConfig = saveConfig;
            this.updateLeds = updateLeds;
            this.sendAllLeds = sendAllLeds;

            LedModeItem[] ledModes =
            [
                new("", "Aus"),
                new(fMain.LedModeMidicraft, "MIDICRAFT (z.B. PUSH²) – Farbpalette per Velocity")
            ];
            cmb_ledmode.Items.AddRange(ledModes);
            cmb_ledmode.SelectedItem = ledModes.FirstOrDefault(m => m.Key == Settings.Default.ledfeedback) ?? ledModes[0];

            cmb_voice.DisplayMember = nameof(ITtsVoice.DisplayName);
            if (voices.Count == 0)
            {
                cmb_voice.Items.Add("Keine Stimme gefunden");
                cmb_voice.SelectedIndex = 0;
                cmb_voice.Enabled = false;
            }
            else
            {
                foreach (ITtsVoice voice in voices)
                {
                    cmb_voice.Items.Add(voice);
                }
                int index = voices.ToList().FindIndex(v => v.Id == Settings.Default.ttsvoice);
                cmb_voice.SelectedIndex = index >= 0 ? index : 0;
            }

            sld_volume.Value = Settings.Default.volume;
            lbl_volume.Text = $"{sld_volume.Value} %";

            loadDevices();
            updateMidiStatus();
            updateStopTrigger();
            midi.StatusChanged += midi_StatusChanged;
        }

        private void cmb_voice_SelectionChangeCommitted(object? sender, EventArgs e)
        {
            if (cmb_voice.SelectedItem is ITtsVoice voice)
            {
                Settings.Default.ttsvoice = voice.Id;
                Settings.Default.Save();
            }
        }

        private void sld_volume_ValueChanged(object? sender, EventArgs e)
        {
            player.Volume = sld_volume.Value / 100f;
            lbl_volume.Text = $"{sld_volume.Value} %";
            Settings.Default.volume = sld_volume.Value;
        }

        #region MIDI

        private void loadDevices()
        {
            List<MidiDeviceItem> devices = MidiService.GetDeviceItems();
            string selection = midi.Selection;

            // Gespeichertes Gerät zeigen, auch wenn es gerade nicht angeschlossen ist
            if (!devices.Any(d => d.Key == selection))
            {
                devices.Add(new MidiDeviceItem(selection, selection + " (nicht verbunden)"));
            }

            cmb_mididevice.Items.Clear();
            foreach (MidiDeviceItem device in devices)
            {
                cmb_mididevice.Items.Add(device);
            }
            cmb_mididevice.SelectedItem = devices.First(d => d.Key == selection);
        }

        private void cmb_mididevice_SelectionChangeCommitted(object? sender, EventArgs e)
        {
            if (cmb_mididevice.SelectedItem is MidiDeviceItem device)
            {
                Settings.Default.mididevice = device.Key;
                Settings.Default.Save();
                midi.Open(device.Key);
            }
        }

        private void btn_midirefresh_Click(object? sender, EventArgs e)
        {
            // Neu öffnen, damit frisch angesteckte Geräte erkannt werden
            midi.Open(midi.Selection);
            loadDevices();
        }

        private void midi_StatusChanged(object? sender, EventArgs e) => updateMidiStatus();

        private void btn_ledsend_Click(object? sender, EventArgs e)
        {
            lbl_ledhint.Text = sendAllLeds();
        }

        private void cmb_ledmode_SelectionChangeCommitted(object? sender, EventArgs e)
        {
            if (cmb_ledmode.SelectedItem is LedModeItem mode)
            {
                Settings.Default.ledfeedback = mode.Key;
                Settings.Default.Save();
                updateLeds();
                updateMidiStatus();
            }
        }

        private void updateMidiStatus()
        {
            lbl_midistatus.Text = midi.Status;
            if (midi.IsOpen && Settings.Default.ledfeedback != "" && !midi.CanSend)
            {
                lbl_midistatus.Text += " – kein MIDI-Ausgang für LED-Farben gefunden";
            }
            btn_stoplearn.Enabled = midi.IsOpen;
        }

        private void updateStopTrigger()
        {
            lbl_stoptrigger.Text = config.StopMidi?.ToString() ?? "Nicht belegt";
            btn_stopclear.Enabled = config.StopMidi != null;
        }

        private void btn_stoplearn_Click(object? sender, EventArgs e)
        {
            if (midi.IsLearning)
            {
                stopLearning();
                return;
            }

            btn_stoplearn.Text = "Abbrechen";
            btn_stoplearn.Variant = ButtonVariant.Accent;
            lbl_stoptrigger.Text = "Taste am MIDI-Gerät drücken …";
            midi.BeginLearn(stopLearned);
        }

        private void stopLearned(MidiTrigger trigger)
        {
            stopLearning();

            string? usage = config.FindMidiUsage(trigger, ignoreStop: true);
            if (usage != null && MessageBox.Show(this,
                    $"{trigger} ist bereits „{usage}“ zugewiesen.\nDort entfernen und für Wiedergabe STOP verwenden?",
                    "SoundBoard", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            config.ReleaseMidi(trigger, keepStop: true);
            config.StopMidi = trigger;
            saveConfig();
            updateStopTrigger();
            updateLeds();
        }

        private void stopLearning()
        {
            midi.CancelLearn();
            btn_stoplearn.Text = "Anlernen";
            btn_stoplearn.Variant = ButtonVariant.Default;
            updateStopTrigger();
        }

        private void btn_stopclear_Click(object? sender, EventArgs e)
        {
            config.StopMidi = null;
            saveConfig();
            updateStopTrigger();
            updateLeds();
        }

        #endregion

        private void fSettings_FormClosing(object? sender, FormClosingEventArgs e)
        {
            midi.CancelLearn();
            midi.StatusChanged -= midi_StatusChanged;
            Settings.Default.Save();
        }
    }
}

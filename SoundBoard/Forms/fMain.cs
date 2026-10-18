using SoundBoard.Properties;

namespace SoundBoard
{
    public partial class fMain : Form
    {
        private readonly AppConfig config = AppConfig.Load();
        private readonly AudioPlayer player = new();
        private readonly MidiService midi;
        private readonly ModernButton[] pads;
        private readonly Dictionary<string, byte[]> ttsCache = new();
        private List<ITtsVoice> voices = new();
        private CancellationTokenSource? ttsCts;
        private ModernButton? activePad;
        private fTts? ttsWindow;

        /// <summary>
        ///  Zuletzt an den Controller gesendete LED-Velocity pro (Kanal, Note).
        /// </summary>
        private Dictionary<(int channel, int note), int> ledState = new();

        /// <summary>
        ///  Sendet nach einem Tastendruck alle LED-Farben neu, kurz verzögert, damit sie nach der
        ///  geräteeigenen LED-Reaktion ankommen. Mehrere Tastendrücke hintereinander werden zusammengefasst.
        /// </summary>
        private readonly System.Windows.Forms.Timer ledRefreshTimer = new() { Interval = 40 };

        public fMain()
        {
            InitializeComponent();

            pads = [button1, button2, button3, button4, button5, button6, button7, button8,
                    button9, button10, button11, button12, button13, button14, button15, button16];
            player.PlaybackStopped += player_PlaybackStopped;
            toolStripStatusLabel1.Text = $"Version {AppInfo.Version} - {AppInfo.Company}";

            // Nach InitializeComponent erstellen, damit der UI-SynchronizationContext existiert
            midi = new MidiService();
            midi.TriggerReceived += midi_TriggerReceived;
            midi.StatusChanged += midi_StatusChanged;
            midi.ActivityReceived += midi_ActivityReceived;
            ledRefreshTimer.Tick += ledRefreshTimer_Tick;
        }

        bool editmode = false;

        private void fMain_Load(object sender, EventArgs e)
        {
            lbl_editmode.Visible = false;

            player.Volume = Settings.Default.volume / 100f;
            voices = TtsService.GetVoices();
            for (int id = 1; id <= AppConfig.ButtonCount; id++)
            {
                applyButton(id);
            }

            midi.Open(Settings.Default.mididevice);
            applyTheme();
        }

        private void fMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            ttsCts?.Cancel();
            ledRefreshTimer.Dispose();
            clearLeds();
            midi.Dispose();
            player.Dispose();
            Settings.Default.Save();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    stopPlayback();
                    return true;
                case Keys.Control | Keys.T:
                    openTtsWindow();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void applyTheme()
        {
            ThemeManager.Current = Settings.Default.darkmode ? Theme.Dark : Theme.Light;
            ThemeManager.Apply(this);
            foreach (Form owned in OwnedForms)
            {
                ThemeManager.Apply(owned);
            }

            Theme t = ThemeManager.Current;
            this.BackColor = editmode ? t.EditBack : t.Back;
            lbl_editmode.ForeColor = t.Danger;
            lbl_status.ForeColor = t.TextMuted;
            lbl_midi.ForeColor = t.TextMuted;

            int iconSize = LogicalToDeviceUnits(16);
            setMenuIcon(btn_themetoggle, IconFont.Render(t.IsDark ? IconFont.Sun : IconFont.Moon, iconSize, t.Text));
            setMenuIcon(btn_stopmenu, IconFont.RenderStop(iconSize, t.Danger));
            setMenuIcon(btn_ttsmenu, IconFont.Render(IconFont.Speech, iconSize, t.Text));
            btn_themetoggle.Text = t.IsDark ? "Light Mode" : "Dark Mode";
            this.Invalidate(true);
        }

        private static void setMenuIcon(ToolStripItem item, Image icon)
        {
            Image? old = item.Image;
            item.Image = icon;
            old?.Dispose();
        }

        private void btn_themetoggle_Click(object sender, EventArgs e)
        {
            Settings.Default.darkmode = !Settings.Default.darkmode;
            Settings.Default.Save();
            applyTheme();
        }

        private void editModeToggle()
        {
            editmode = !editmode;
            lbl_editmode.Visible = editmode;
            btn_editmodetoggle.Checked = editmode;
            applyTheme();
        }

        private void btn_editmodetoggle_Click(object sender, EventArgs e)
        {
            editModeToggle();
        }

        private void btn_einstellungen_Click(object sender, EventArgs e)
        {
            using var form_settings = new fSettings(config, voices, player, midi, saveConfig, updateLeds, sendAllLeds);
            ThemeManager.Apply(form_settings);
            form_settings.ShowDialog(this);
            updateLeds();
        }

        private void btn_about_Click(object sender, EventArgs e)
        {
            using var form_about = new fAbout();
            ThemeManager.Apply(form_about);
            form_about.ShowDialog(this);
        }

        #region Soundbuttons

        /// <summary>
        ///  Überträgt Beschriftung, Farbe, Icon und Bild aus der Konfiguration auf den Button.
        /// </summary>
        private void applyButton(int id)
        {
            ButtonConfig cfg = config.Buttons[id - 1];
            ModernButton pad = pads[id - 1];

            pad.Text = cfg.Title ?? "";
            pad.Font = new Font("Segoe UI Semibold", cfg.FontSize);
            pad.CornerRadius = 12;
            pad.CustomColor = cfg.GetColor();
            pad.IconGlyph = cfg.IconGlyph;

            Image? oldImage = pad.Image;
            pad.Image = AppConfig.LoadImage(cfg.ImageFile);
            oldImage?.Dispose();
        }

        private void btnClicker(int id)
        {
            scheduleLedRefresh();

            if (editmode)
            {
                editButton(id);
            }
            else
            {
                playSound(id);
            }
        }

        private void editButton(int id)
        {
            ButtonConfig old = config.Buttons[id - 1];
            using var form_buttonedit = new fButtonEdit(id, old, config.Texts, cfg => playConfig(cfg, null), stopPlayback,
                midi, trigger => config.FindMidiUsage(trigger, ignoreButtonId: id));
            ThemeManager.Apply(form_buttonedit);
            if (form_buttonedit.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            ButtonConfig result = form_buttonedit.Result;
            if (old.ImageFile != result.ImageFile && AppConfig.IsImportedImage(old.ImageFile))
            {
                try { File.Delete(old.ImageFile!); } catch (Exception) { }
            }

            // Ein MIDI-Befehl gehört immer nur zu einer Aktion
            if (result.Midi != null)
            {
                config.ReleaseMidi(result.Midi, keepButtonId: id);
            }

            config.Buttons[id - 1] = result;
            saveConfig();
            applyButton(id);
            setStatus($"Button {id} gespeichert.");
            updateLeds();
        }

        private void playSound(int id)
        {
            ButtonConfig cfg = config.Buttons[id - 1];
            if (cfg.Action == ButtonAction.None)
            {
                setStatus($"Button {id} ist noch leer – im Editierungsmodus (Strg+E) belegen.");
                return;
            }
            playConfig(cfg, pads[id - 1]);
        }

        private void playConfig(ButtonConfig cfg, ModernButton? pad)
        {
            switch (cfg.Action)
            {
                case ButtonAction.Audio:
                    playFile(cfg.AudioFile, pad);
                    break;
                case ButtonAction.Tts:
                    _ = speakAsync(cfg.TtsText, pad);
                    break;
            }
        }

        private void playFile(string? path, ModernButton? pad)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                showError($"Die Audio-Datei wurde nicht gefunden:\n{path}");
                return;
            }

            cancelTts();
            try
            {
                player.PlayFile(path);
                setActive(pad);
                setStatus("▶  " + Path.GetFileName(path));
            }
            catch (Exception ex)
            {
                showError($"Die Audio-Datei konnte nicht abgespielt werden:\n{ex.Message}");
            }
        }

        private void setActive(ModernButton? pad)
        {
            if (activePad != null)
            {
                activePad.IsActive = false;
            }
            activePad = pad;
            if (pad != null)
            {
                pad.IsActive = true;
            }
            updateLeds();
        }

        private void player_PlaybackStopped(object? sender, EventArgs e)
        {
            setActive(null);
            setStatus("");
        }

        private void stopPlayback()
        {
            cancelTts();
            player.Stop();
        }

        private void btn_stopmenu_Click(object sender, EventArgs e)
        {
            stopPlayback();
        }

        #endregion

        #region MIDI

        private void midi_TriggerReceived(object? sender, MidiTrigger trigger)
        {
            if (trigger == config.StopMidi)
            {
                stopPlayback();
                return;
            }

            int index = config.Buttons.FindIndex(b => b.Midi == trigger);
            if (index >= 0)
            {
                playSound(index + 1);
            }
        }

        private void midi_StatusChanged(object? sender, EventArgs e)
        {
            lbl_midi.Text = midi.Status;

            // Neues Gerät bzw. neu verbunden: alle LEDs sofort und zur Sicherheit kurz darauf nochmal
            // senden, da manche USB-Geräte Nachrichten direkt nach dem Öffnen verwerfen
            ledState.Clear();
            updateLeds();
            scheduleLedRefresh(750);
        }

        #endregion

        #region LED-Feedback

        /// <summary>
        ///  Wert der Einstellung "ledfeedback" für MIDICRAFT-Controller.
        /// </summary>
        internal const string LedModeMidicraft = "midicraft";

        /// <summary>
        ///  Farbe für belegte Buttons ohne eigene Farbe.
        /// </summary>
        private static readonly Color defaultLedColor = ColorTranslator.FromHtml("#3B6CF6");

        /// <summary>
        ///  Bringt die LEDs am Controller auf den aktuellen Stand: Button-Farbe, weiß während der Wiedergabe,
        ///  aus für leere Buttons, rot für "Wiedergabe STOP". Gesendet wird nur, was sich geändert hat.
        /// </summary>
        private void updateLeds()
        {
            var wanted = new Dictionary<(int channel, int note), int>();
            if (Settings.Default.ledfeedback == LedModeMidicraft && midi.CanSend)
            {
                for (int id = 1; id <= AppConfig.ButtonCount; id++)
                {
                    ButtonConfig cfg = config.Buttons[id - 1];
                    if (cfg.Midi is { Type: MidiMessageType.Note } trigger)
                    {
                        wanted[(trigger.Channel, trigger.Number)] = ledVelocity(cfg, pads[id - 1]);
                    }
                }

                if (config.StopMidi is { Type: MidiMessageType.Note } stop)
                {
                    wanted[(stop.Channel, stop.Number)] = MidicraftPalette.FromColor(ThemeManager.Current.Danger);
                }
            }

            // Nicht mehr benötigte LEDs ausschalten (z.B. nach Umbelegen oder Abschalten des Feedbacks)
            foreach ((int channel, int note) key in ledState.Keys.Except(wanted.Keys))
            {
                midi.SendNoteOn(key.channel, key.note, MidicraftPalette.Off);
            }
            foreach (KeyValuePair<(int channel, int note), int> led in wanted)
            {
                if (!ledState.TryGetValue(led.Key, out int sent) || sent != led.Value)
                {
                    midi.SendNoteOn(led.Key.channel, led.Key.note, led.Value);
                }
            }
            ledState = wanted;
        }

        /// <summary>
        ///  Alle LED-Farben nach <paramref name="delayMs"/> vollständig neu senden.
        /// </summary>
        private void scheduleLedRefresh(int delayMs = 40)
        {
            ledRefreshTimer.Stop();
            ledRefreshTimer.Interval = delayMs;
            ledRefreshTimer.Start();
        }

        private void midi_ActivityReceived(object? sender, EventArgs e)
        {
            scheduleLedRefresh();
        }

        private void ledRefreshTimer_Tick(object? sender, EventArgs e)
        {
            ledRefreshTimer.Stop();
            sendAllLeds();
        }

        /// <summary>
        ///  Sendet sofort alle LED-Farben neu und beschreibt, was gesendet wurde (für die Einstellungen).
        /// </summary>
        private string sendAllLeds()
        {
            // Gemerkten Stand verwerfen, damit wirklich jede Taste neu gesendet wird
            ledState.Clear();
            updateLeds();

            if (Settings.Default.ledfeedback != LedModeMidicraft)
            {
                return "LED-Farben sind ausgeschaltet.";
            }
            if (!midi.CanSend)
            {
                return "Nichts gesendet – kein MIDI-Ausgang verbunden.";
            }
            if (ledState.Count == 0)
            {
                return "Nichts gesendet – kein Button und kein STOP ist auf eine Note angelernt.";
            }
            return "Gesendet um " + DateTime.Now.ToString("HH:mm:ss") + ": " + string.Join(", ",
                ledState.OrderBy(l => l.Key.channel).ThenBy(l => l.Key.note)
                    .Select(l => $"Kanal {l.Key.channel} Note {l.Key.note} → {l.Value}"));
        }

        private int ledVelocity(ButtonConfig cfg, ModernButton pad)
        {
            if (cfg.Action == ButtonAction.None)
            {
                return MidicraftPalette.Off;
            }
            if (pad == activePad)
            {
                return MidicraftPalette.White;
            }
            Color color = cfg.GetColor();
            return MidicraftPalette.FromColor(color.IsEmpty ? defaultLedColor : color);
        }

        /// <summary>
        ///  Schaltet alle zuletzt gesetzten LEDs aus (beim Beenden).
        /// </summary>
        private void clearLeds()
        {
            foreach ((int channel, int note) key in ledState.Keys)
            {
                midi.SendNoteOn(key.channel, key.note, MidicraftPalette.Off);
            }
            ledState.Clear();
        }

        #endregion

        #region Text-to-Speech

        private ITtsVoice? currentVoice()
        {
            return voices.FirstOrDefault(v => v.Id == Settings.Default.ttsvoice) ?? voices.FirstOrDefault();
        }

        private async Task speakAsync(string? text, ModernButton? pad)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                setStatus("Bitte zuerst einen Text eingeben.");
                return;
            }
            if (currentVoice() is not ITtsVoice voice)
            {
                showError("Es wurde keine Stimme für Text-to-Speech gefunden.");
                return;
            }

            cancelTts();
            var cts = new CancellationTokenSource();
            ttsCts = cts;
            try
            {
                string key = voice.Id + "\n" + text;
                if (!ttsCache.TryGetValue(key, out byte[]? wav))
                {
                    setStatus("Sprache wird erzeugt …");
                    wav = await voice.SynthesizeAsync(text, cts.Token);
                    if (ttsCache.Count > 50)
                    {
                        ttsCache.Clear();
                    }
                    ttsCache[key] = wav;
                }

                if (cts.IsCancellationRequested)
                {
                    return;
                }
                player.PlayWave(wav);
                setActive(pad);
                setStatus("▶  Text-to-Speech · " + voice.DisplayName);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                setStatus("");
                showError($"Text-to-Speech fehlgeschlagen:\n{ex.Message}");
            }
            finally
            {
                if (ttsCts == cts)
                {
                    ttsCts = null;
                }
                cts.Dispose();
            }
        }

        private void cancelTts()
        {
            ttsCts?.Cancel();
            ttsCts = null;
        }

        private void btn_ttsmenu_Click(object sender, EventArgs e)
        {
            openTtsWindow();
        }

        private void openTtsWindow()
        {
            if (ttsWindow == null || ttsWindow.IsDisposed)
            {
                ttsWindow = new fTts(config, saveConfig, text => _ = speakAsync(text, null), stopPlayback, assignText) { Owner = this };
                ThemeManager.Apply(ttsWindow);
                ttsWindow.Location = new Point(
                    Left + (Width - ttsWindow.Width) / 2,
                    Top + (Height - ttsWindow.Height) / 2);
                ttsWindow.Show();
            }
            else
            {
                ttsWindow.Activate();
            }
        }

        private void assignText(int id, string text, string title)
        {
            ButtonConfig cfg = config.Buttons[id - 1];
            bool sameText = cfg.Action == ButtonAction.Tts && cfg.TtsText == text;
            if (cfg.Action != ButtonAction.None && !sameText
                && confirm($"Button {id} ist bereits belegt. Mit diesem Text ersetzen?") != DialogResult.Yes)
            {
                return;
            }

            cfg.Action = ButtonAction.Tts;
            cfg.TtsText = text;
            cfg.AudioFile = null;
            if (string.IsNullOrEmpty(cfg.Title) || cfg.Title == ButtonConfig.DefaultTitle(id))
            {
                cfg.Title = title;
            }

            saveConfig();
            applyButton(id);
            setStatus($"Text liegt jetzt auf Button {id}.");
            updateLeds();
        }

        #endregion

        private void saveConfig()
        {
            try
            {
                config.Save();
            }
            catch (Exception ex)
            {
                showError($"Die Einstellungen konnten nicht gespeichert werden:\n{ex.Message}");
            }
        }

        private void setStatus(string text)
        {
            lbl_status.Text = text;
        }

        private void showError(string message)
        {
            MessageBox.Show(this, message, "SoundBoard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private DialogResult confirm(string message)
        {
            return MessageBox.Show(this, message, "SoundBoard", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnClicker(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            btnClicker(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            btnClicker(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            btnClicker(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            btnClicker(5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            btnClicker(6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            btnClicker(7);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            btnClicker(8);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            btnClicker(9);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            btnClicker(10);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            btnClicker(11);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            btnClicker(12);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            btnClicker(13);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            btnClicker(14);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            btnClicker(15);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            btnClicker(16);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

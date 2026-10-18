namespace SoundBoard
{
    public partial class fButtonEdit : Form
    {
        private static readonly string[] presetColors =
        [
            "#3B6CF6", "#7C5CFA", "#D946EF", "#E5484D", "#F97316",
            "#EAB308", "#22C55E", "#14B8A6", "#0EA5E9", "#64748B"
        ];

        private readonly int id;
        private readonly Action<ButtonConfig> preview;
        private readonly Action stop;
        private readonly MidiService midi;
        private readonly Func<MidiTrigger, string?> findMidiUsage;
        private readonly List<(ModernButton button, string? color)> colorButtons = new();
        private readonly List<(ModernButton button, string? glyph)> iconButtons = new();
        private ModernButton btn_customcolor = null!;
        private ButtonConfig cfg;
        private string? loadedImageFile;
        private bool loading;

        /// <summary>
        ///  Die bearbeitete Konfiguration, gültig nach DialogResult.OK.
        /// </summary>
        public ButtonConfig Result => cfg;

        /// <param name="findMidiUsage">Liefert, wo ein MIDI-Befehl sonst schon verwendet wird (z.B. "Button 3"), oder null.</param>
        internal fButtonEdit(int id, ButtonConfig config, IEnumerable<SavedText> texts, Action<ButtonConfig> preview, Action stop,
            MidiService midi, Func<MidiTrigger, string?> findMidiUsage)
        {
            InitializeComponent();

            this.id = id;
            this.preview = preview;
            this.stop = stop;
            this.midi = midi;
            this.findMidiUsage = findMidiUsage;
            cfg = config.Clone();

            Text = $"SoundBoard - Button {id} bearbeiten";
            cmb_texts.Items.AddRange(texts.OrderBy(t => t.Name, StringComparer.CurrentCultureIgnoreCase).ToArray<object>());
            cmb_texts.Enabled = cmb_texts.Items.Count > 0;
            cmb_texts.Items.Insert(0, cmb_texts.Enabled ? "Gespeicherten Text übernehmen …" : "Noch keine gespeicherten Texte");

            buildColorButtons();
            buildIconButtons();
            loadValues();
        }

        private void buildColorButtons()
        {
            ModernButton standard = createChoice(flp_colors, 32, "Standard");
            colorButtons.Add((standard, null));
            standard.Click += (_, _) => setColor(null);

            foreach (string color in presetColors)
            {
                ModernButton swatch = createChoice(flp_colors, 32, color);
                swatch.CustomColor = ColorTranslator.FromHtml(color);
                colorButtons.Add((swatch, color));
                swatch.Click += (_, _) => setColor(color);
            }

            btn_customcolor = createChoice(flp_colors, 32, "Eigene Farbe …");
            btn_customcolor.Text = "…";
            btn_customcolor.Click += btn_customcolor_Click;
        }

        private void buildIconButtons()
        {
            ModernButton none = createChoice(flp_icons, 34, "Kein Icon");
            none.Text = "–";
            iconButtons.Add((none, null));
            none.Click += (_, _) => setIcon(null);

            foreach (string glyph in IconFont.Glyphs)
            {
                ModernButton icon = createChoice(flp_icons, 34, null);
                icon.IconGlyph = glyph;
                iconButtons.Add((icon, glyph));
                icon.Click += (_, _) => setIcon(glyph);
            }
        }

        private ModernButton createChoice(FlowLayoutPanel panel, int size, string? tooltip)
        {
            var button = new ModernButton
            {
                Size = new Size(size, size),
                Margin = new Padding(0, 0, 4, 4),
                CornerRadius = 6,
                TabStop = false
            };
            if (tooltip != null)
            {
                toolTip1.SetToolTip(button, tooltip);
            }
            panel.Controls.Add(button);
            return button;
        }

        private void loadValues()
        {
            loading = true;
            txt_audiofile.Text = cfg.AudioFile ?? "";
            txt_ttstext.Text = cfg.TtsText ?? "";
            txt_title.Text = cfg.Title ?? "";
            sld_fontsize.Value = (int)Math.Round(cfg.FontSize);
            lbl_fontsizevalue.Text = $"{sld_fontsize.Value} pt";
            cmb_texts.SelectedIndex = 0;
            loading = false;

            setMode(cfg.Action);
            updatePreview();
            updateMidi();
        }

        #region MIDI

        private void updateMidi()
        {
            lbl_miditrigger.Text = cfg.Midi?.ToString() ?? (midi.IsOpen ? "Nicht belegt" : "Nicht belegt – zuerst unter Datei › Einstellungen ein MIDI-Gerät wählen");
            if (cfg.Midi != null && cfg.Midi.Type != MidiMessageType.Note && Properties.Settings.Default.ledfeedback != "")
            {
                // LED-Feedback läuft über Note On auf derselben Note
                lbl_miditrigger.Text += " – LED-Farbe nur bei Noten möglich";
            }
            btn_midilearn.Enabled = midi.IsOpen || midi.IsLearning;
            btn_midiclear.Enabled = cfg.Midi != null;
        }

        private void btn_midilearn_Click(object? sender, EventArgs e)
        {
            if (midi.IsLearning)
            {
                stopLearning();
                return;
            }

            btn_midilearn.Text = "Abbrechen";
            btn_midilearn.Variant = ButtonVariant.Accent;
            lbl_miditrigger.Text = "Taste, Pad oder Regler am MIDI-Gerät betätigen …";
            midi.BeginLearn(midiLearned);
        }

        private void midiLearned(MidiTrigger trigger)
        {
            stopLearning();

            string? usage = findMidiUsage(trigger);
            if (usage != null && MessageBox.Show(this,
                    $"{trigger} ist bereits „{usage}“ zugewiesen.\nBeim Übernehmen dort entfernen und für diesen Button verwenden?",
                    "SoundBoard", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            cfg.Midi = trigger;
            updateMidi();
        }

        private void stopLearning()
        {
            midi.CancelLearn();
            btn_midilearn.Text = "Anlernen";
            btn_midilearn.Variant = ButtonVariant.Default;
            updateMidi();
        }

        private void btn_midiclear_Click(object? sender, EventArgs e)
        {
            cfg.Midi = null;
            updateMidi();
        }

        #endregion

        #region Aktion

        private void setMode(ButtonAction action)
        {
            cfg.Action = action;

            btn_modeAudio.Variant = action == ButtonAction.Audio ? ButtonVariant.Accent : ButtonVariant.Default;
            btn_modeTts.Variant = action == ButtonAction.Tts ? ButtonVariant.Accent : ButtonVariant.Default;
            btn_modeNone.Variant = action == ButtonAction.None ? ButtonVariant.Accent : ButtonVariant.Default;

            lbl_audio.Visible = txt_audiofile.Visible = btn_browse.Visible = action == ButtonAction.Audio;
            cmb_texts.Visible = txt_ttstext.Visible = action == ButtonAction.Tts;
            lbl_none.Visible = action == ButtonAction.None;
            btn_test.Enabled = action != ButtonAction.None;
            cardAction.Invalidate();
        }

        private void btn_modeAudio_Click(object sender, EventArgs e) => setMode(ButtonAction.Audio);
        private void btn_modeTts_Click(object sender, EventArgs e) => setMode(ButtonAction.Tts);
        private void btn_modeNone_Click(object sender, EventArgs e) => setMode(ButtonAction.None);

        private void txt_audiofile_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                cfg.AudioFile = txt_audiofile.Text.Trim().Trim('"');
            }
        }

        private void txt_ttstext_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                cfg.TtsText = txt_ttstext.Text;
            }
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Audio-Datei auswählen",
                Filter = "Audio-Dateien|*.wav;*.mp3;*.m4a;*.aac;*.wma;*.flac;*.aif;*.aiff|Alle Dateien|*.*"
            };
            if (File.Exists(cfg.AudioFile))
            {
                dialog.InitialDirectory = Path.GetDirectoryName(cfg.AudioFile);
            }
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            txt_audiofile.Text = dialog.FileName;
            if (isDefaultTitle())
            {
                txt_title.Text = Path.GetFileNameWithoutExtension(dialog.FileName);
            }
        }

        private void cmb_texts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading || cmb_texts.SelectedItem is not SavedText saved)
            {
                return;
            }

            txt_ttstext.Text = saved.Text;
            if (isDefaultTitle())
            {
                txt_title.Text = saved.Name;
            }
        }

        private bool isDefaultTitle()
        {
            return string.IsNullOrWhiteSpace(cfg.Title) || cfg.Title == ButtonConfig.DefaultTitle(id);
        }

        private void btn_test_Click(object sender, EventArgs e)
        {
            if (cfg.Action != ButtonAction.None)
            {
                preview(cfg.Clone());
            }
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            stop();
        }

        #endregion

        #region Darstellung

        private void txt_title_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                cfg.Title = txt_title.Text;
                updatePreview();
            }
        }

        private void sld_fontsize_ValueChanged(object sender, EventArgs e)
        {
            lbl_fontsizevalue.Text = $"{sld_fontsize.Value} pt";
            if (!loading)
            {
                cfg.FontSize = sld_fontsize.Value;
                updatePreview();
            }
        }

        private void setColor(string? color)
        {
            cfg.Color = color;
            updatePreview();
        }

        private void btn_customcolor_Click(object? sender, EventArgs e)
        {
            using var dialog = new ColorDialog { FullOpen = true };
            Color current = cfg.GetColor();
            if (!current.IsEmpty)
            {
                dialog.Color = current;
            }
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                setColor(ColorTranslator.ToHtml(Color.FromArgb(dialog.Color.R, dialog.Color.G, dialog.Color.B)));
            }
        }

        private void setIcon(string? glyph)
        {
            cfg.IconGlyph = glyph;
            if (glyph != null)
            {
                // Ein Bild hat Vorrang vor dem Icon, daher beim Wählen eines Icons entfernen
                cfg.ImageFile = null;
            }
            updatePreview();
        }

        private void btn_image_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Bild auswählen",
                Filter = "Bilder|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.ico|Alle Dateien|*.*"
            };
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            using (Image? test = AppConfig.LoadImage(dialog.FileName))
            {
                if (test == null)
                {
                    MessageBox.Show(this, "Die Datei konnte nicht als Bild geladen werden.", "SoundBoard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            cfg.ImageFile = dialog.FileName;
            cfg.IconGlyph = null;
            updatePreview();
        }

        private void btn_imageclear_Click(object sender, EventArgs e)
        {
            cfg.ImageFile = null;
            updatePreview();
        }

        private void updatePreview()
        {
            btn_preview.Text = cfg.Title ?? "";
            Font oldFont = btn_preview.Font;
            btn_preview.Font = new Font("Segoe UI Semibold", cfg.FontSize);
            if (oldFont != Font)
            {
                oldFont.Dispose();
            }
            btn_preview.CustomColor = cfg.GetColor();
            btn_preview.IconGlyph = cfg.IconGlyph;

            if (loadedImageFile != cfg.ImageFile)
            {
                Image? oldImage = btn_preview.Image;
                btn_preview.Image = AppConfig.LoadImage(cfg.ImageFile);
                oldImage?.Dispose();
                loadedImageFile = cfg.ImageFile;
            }

            lbl_imagefile.Text = string.IsNullOrEmpty(cfg.ImageFile) ? "Kein Bild" : Path.GetFileName(cfg.ImageFile);
            btn_imageclear.Enabled = !string.IsNullOrEmpty(cfg.ImageFile);

            bool presetColor = false;
            foreach ((ModernButton button, string? color) in colorButtons)
            {
                bool active = string.Equals(color, cfg.Color, StringComparison.OrdinalIgnoreCase);
                button.IsActive = active;
                presetColor |= active;
            }
            btn_customcolor.IsActive = !presetColor;
            btn_customcolor.CustomColor = presetColor ? Color.Empty : cfg.GetColor();

            foreach ((ModernButton button, string? glyph) in iconButtons)
            {
                button.IsActive = string.IsNullOrEmpty(cfg.ImageFile) && glyph == cfg.IconGlyph;
            }
        }

        #endregion

        private void btn_reset_Click(object sender, EventArgs e)
        {
            cfg = ButtonConfig.CreateDefault(id);
            loadValues();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (cfg.Action == ButtonAction.Audio && !File.Exists(cfg.AudioFile))
            {
                MessageBox.Show(this, "Bitte eine vorhandene Audio-Datei auswählen.", "SoundBoard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cfg.Action == ButtonAction.Tts && string.IsNullOrWhiteSpace(cfg.TtsText))
            {
                MessageBox.Show(this, "Bitte einen Text für Text-to-Speech eingeben.", "SoundBoard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Nicht benötigte Felder leeren, damit die Konfiguration übersichtlich bleibt
            if (cfg.Action != ButtonAction.Audio)
            {
                cfg.AudioFile = null;
            }
            if (cfg.Action != ButtonAction.Tts)
            {
                cfg.TtsText = null;
            }

            if (!string.IsNullOrEmpty(cfg.ImageFile) && !AppConfig.IsImportedImage(cfg.ImageFile))
            {
                try
                {
                    cfg.ImageFile = AppConfig.ImportImage(cfg.ImageFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"Das Bild konnte nicht übernommen werden:\n{ex.Message}", "SoundBoard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void fButtonEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            midi.CancelLearn();
            stop();
            btn_preview.Image?.Dispose();
            btn_preview.Image = null;
        }
    }
}

namespace SoundBoard
{
    partial class fButtonEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            cardAction = new CardGroupBox();
            btn_modeAudio = new ModernButton();
            btn_modeTts = new ModernButton();
            btn_modeNone = new ModernButton();
            lbl_audio = new Label();
            txt_audiofile = new TextBox();
            btn_browse = new ModernButton();
            cmb_texts = new ModernComboBox();
            txt_ttstext = new TextBox();
            lbl_none = new MutedLabel();
            btn_test = new ModernButton();
            btn_stop = new ModernButton();
            cardPreview = new CardGroupBox();
            btn_preview = new ModernButton();
            cardLook = new CardGroupBox();
            lbl_title = new Label();
            txt_title = new TextBox();
            lbl_fontsize = new Label();
            lbl_fontsizevalue = new MutedLabel();
            sld_fontsize = new ModernSlider();
            lbl_image = new Label();
            btn_image = new ModernButton();
            btn_imageclear = new ModernButton();
            lbl_imagefile = new MutedLabel();
            lbl_color = new Label();
            flp_colors = new FlowLayoutPanel();
            lbl_icon = new Label();
            flp_icons = new FlowLayoutPanel();
            cardMidi = new CardGroupBox();
            lbl_miditrigger = new MutedLabel();
            btn_midilearn = new ModernButton();
            btn_midiclear = new ModernButton();
            btn_reset = new ModernButton();
            btn_cancel = new ModernButton();
            btn_ok = new ModernButton();
            toolTip1 = new ToolTip(components);
            cardAction.SuspendLayout();
            cardPreview.SuspendLayout();
            cardLook.SuspendLayout();
            cardMidi.SuspendLayout();
            SuspendLayout();
            //
            // cardAction
            //
            cardAction.Controls.Add(btn_modeAudio);
            cardAction.Controls.Add(btn_modeTts);
            cardAction.Controls.Add(btn_modeNone);
            cardAction.Controls.Add(lbl_audio);
            cardAction.Controls.Add(txt_audiofile);
            cardAction.Controls.Add(btn_browse);
            cardAction.Controls.Add(cmb_texts);
            cardAction.Controls.Add(txt_ttstext);
            cardAction.Controls.Add(lbl_none);
            cardAction.Controls.Add(btn_test);
            cardAction.Controls.Add(btn_stop);
            cardAction.Location = new Point(12, 12);
            cardAction.Name = "cardAction";
            cardAction.Size = new Size(480, 262);
            cardAction.TabIndex = 0;
            cardAction.TabStop = false;
            cardAction.Text = "Aktion";
            //
            // btn_modeAudio
            //
            btn_modeAudio.Location = new Point(16, 30);
            btn_modeAudio.Name = "btn_modeAudio";
            btn_modeAudio.Size = new Size(146, 32);
            btn_modeAudio.TabIndex = 0;
            btn_modeAudio.Text = "Audio-Datei";
            btn_modeAudio.UseVisualStyleBackColor = true;
            btn_modeAudio.Click += btn_modeAudio_Click;
            //
            // btn_modeTts
            //
            btn_modeTts.Location = new Point(168, 30);
            btn_modeTts.Name = "btn_modeTts";
            btn_modeTts.Size = new Size(146, 32);
            btn_modeTts.TabIndex = 1;
            btn_modeTts.Text = "Text-to-Speech";
            btn_modeTts.UseVisualStyleBackColor = true;
            btn_modeTts.Click += btn_modeTts_Click;
            //
            // btn_modeNone
            //
            btn_modeNone.Location = new Point(320, 30);
            btn_modeNone.Name = "btn_modeNone";
            btn_modeNone.Size = new Size(146, 32);
            btn_modeNone.TabIndex = 2;
            btn_modeNone.Text = "Keine Aktion";
            btn_modeNone.UseVisualStyleBackColor = true;
            btn_modeNone.Click += btn_modeNone_Click;
            //
            // lbl_audio
            //
            lbl_audio.AutoSize = true;
            lbl_audio.Location = new Point(16, 78);
            lbl_audio.Name = "lbl_audio";
            lbl_audio.Size = new Size(185, 15);
            lbl_audio.TabIndex = 3;
            lbl_audio.Text = "Audio-Datei (WAV, MP3, M4A, …)";
            //
            // txt_audiofile
            //
            txt_audiofile.BorderStyle = BorderStyle.None;
            txt_audiofile.Location = new Point(24, 107);
            txt_audiofile.Name = "txt_audiofile";
            txt_audiofile.PlaceholderText = "Pfad zur Audio-Datei";
            txt_audiofile.Size = new Size(318, 16);
            txt_audiofile.TabIndex = 4;
            txt_audiofile.TextChanged += txt_audiofile_TextChanged;
            //
            // btn_browse
            //
            btn_browse.Location = new Point(358, 100);
            btn_browse.Name = "btn_browse";
            btn_browse.Size = new Size(108, 30);
            btn_browse.TabIndex = 5;
            btn_browse.Text = "Durchsuchen …";
            btn_browse.UseVisualStyleBackColor = true;
            btn_browse.Click += btn_browse_Click;
            //
            // cmb_texts
            //
            cmb_texts.Location = new Point(16, 76);
            cmb_texts.Name = "cmb_texts";
            cmb_texts.Size = new Size(450, 28);
            cmb_texts.TabIndex = 6;
            cmb_texts.SelectedIndexChanged += cmb_texts_SelectedIndexChanged;
            //
            // txt_ttstext
            //
            txt_ttstext.AcceptsReturn = true;
            txt_ttstext.BorderStyle = BorderStyle.None;
            txt_ttstext.Location = new Point(24, 116);
            txt_ttstext.Multiline = true;
            txt_ttstext.Name = "txt_ttstext";
            txt_ttstext.PlaceholderText = "Text, der beim Klick gesprochen wird …";
            txt_ttstext.ScrollBars = ScrollBars.Vertical;
            txt_ttstext.Size = new Size(434, 88);
            txt_ttstext.TabIndex = 7;
            txt_ttstext.TextChanged += txt_ttstext_TextChanged;
            //
            // lbl_none
            //
            lbl_none.Location = new Point(16, 80);
            lbl_none.Name = "lbl_none";
            lbl_none.Size = new Size(450, 40);
            lbl_none.TabIndex = 8;
            lbl_none.Text = "Der Button ist nicht belegt und spielt nichts ab.";
            //
            // btn_test
            //
            btn_test.Location = new Point(16, 220);
            btn_test.Name = "btn_test";
            btn_test.Size = new Size(146, 30);
            btn_test.TabIndex = 9;
            btn_test.Text = "▶  Testen";
            btn_test.UseVisualStyleBackColor = true;
            btn_test.Click += btn_test_Click;
            //
            // btn_stop
            //
            btn_stop.Location = new Point(168, 220);
            btn_stop.Name = "btn_stop";
            btn_stop.Size = new Size(108, 30);
            btn_stop.TabIndex = 10;
            btn_stop.Text = "■  Stop";
            btn_stop.UseVisualStyleBackColor = true;
            btn_stop.Click += btn_stop_Click;
            //
            // cardPreview
            //
            cardPreview.Controls.Add(btn_preview);
            cardPreview.Location = new Point(504, 12);
            cardPreview.Name = "cardPreview";
            cardPreview.Size = new Size(264, 262);
            cardPreview.TabIndex = 1;
            cardPreview.TabStop = false;
            cardPreview.Text = "Vorschau";
            //
            // btn_preview
            //
            btn_preview.CornerRadius = 12;
            btn_preview.Location = new Point(26, 76);
            btn_preview.Name = "btn_preview";
            btn_preview.Size = new Size(211, 125);
            btn_preview.TabIndex = 0;
            btn_preview.TabStop = false;
            btn_preview.UseVisualStyleBackColor = true;
            btn_preview.Click += btn_test_Click;
            //
            // cardLook
            //
            cardLook.Controls.Add(lbl_title);
            cardLook.Controls.Add(txt_title);
            cardLook.Controls.Add(lbl_fontsize);
            cardLook.Controls.Add(lbl_fontsizevalue);
            cardLook.Controls.Add(sld_fontsize);
            cardLook.Controls.Add(lbl_image);
            cardLook.Controls.Add(btn_image);
            cardLook.Controls.Add(btn_imageclear);
            cardLook.Controls.Add(lbl_imagefile);
            cardLook.Controls.Add(lbl_color);
            cardLook.Controls.Add(flp_colors);
            cardLook.Controls.Add(lbl_icon);
            cardLook.Controls.Add(flp_icons);
            cardLook.Location = new Point(12, 286);
            cardLook.Name = "cardLook";
            cardLook.Size = new Size(756, 280);
            cardLook.TabIndex = 2;
            cardLook.TabStop = false;
            cardLook.Text = "Darstellung";
            //
            // lbl_title
            //
            lbl_title.AutoSize = true;
            lbl_title.Location = new Point(16, 30);
            lbl_title.Name = "lbl_title";
            lbl_title.Size = new Size(74, 15);
            lbl_title.TabIndex = 0;
            lbl_title.Text = "Beschriftung";
            //
            // txt_title
            //
            txt_title.BorderStyle = BorderStyle.None;
            txt_title.Location = new Point(24, 59);
            txt_title.Name = "txt_title";
            txt_title.PlaceholderText = "Text auf dem Button";
            txt_title.Size = new Size(330, 16);
            txt_title.TabIndex = 1;
            txt_title.TextChanged += txt_title_TextChanged;
            //
            // lbl_fontsize
            //
            lbl_fontsize.AutoSize = true;
            lbl_fontsize.Location = new Point(16, 96);
            lbl_fontsize.Name = "lbl_fontsize";
            lbl_fontsize.Size = new Size(72, 15);
            lbl_fontsize.TabIndex = 2;
            lbl_fontsize.Text = "Schriftgröße";
            //
            // lbl_fontsizevalue
            //
            lbl_fontsizevalue.Location = new Point(282, 96);
            lbl_fontsizevalue.Name = "lbl_fontsizevalue";
            lbl_fontsizevalue.Size = new Size(80, 15);
            lbl_fontsizevalue.TabIndex = 3;
            lbl_fontsizevalue.Text = "12 pt";
            lbl_fontsizevalue.TextAlign = ContentAlignment.TopRight;
            //
            // sld_fontsize
            //
            sld_fontsize.Location = new Point(8, 114);
            sld_fontsize.Maximum = 28;
            sld_fontsize.Minimum = 8;
            sld_fontsize.Name = "sld_fontsize";
            sld_fontsize.Size = new Size(362, 34);
            sld_fontsize.TabIndex = 4;
            sld_fontsize.Value = 12;
            sld_fontsize.ValueChanged += sld_fontsize_ValueChanged;
            //
            // lbl_image
            //
            lbl_image.AutoSize = true;
            lbl_image.Location = new Point(16, 162);
            lbl_image.Name = "lbl_image";
            lbl_image.Size = new Size(32, 15);
            lbl_image.TabIndex = 5;
            lbl_image.Text = "Bild";
            //
            // btn_image
            //
            btn_image.Location = new Point(16, 186);
            btn_image.Name = "btn_image";
            btn_image.Size = new Size(140, 30);
            btn_image.TabIndex = 6;
            btn_image.Text = "Bild wählen …";
            btn_image.UseVisualStyleBackColor = true;
            btn_image.Click += btn_image_Click;
            //
            // btn_imageclear
            //
            btn_imageclear.Location = new Point(162, 186);
            btn_imageclear.Name = "btn_imageclear";
            btn_imageclear.Size = new Size(100, 30);
            btn_imageclear.TabIndex = 7;
            btn_imageclear.Text = "Entfernen";
            btn_imageclear.UseVisualStyleBackColor = true;
            btn_imageclear.Click += btn_imageclear_Click;
            //
            // lbl_imagefile
            //
            lbl_imagefile.AutoEllipsis = true;
            lbl_imagefile.Location = new Point(16, 224);
            lbl_imagefile.Name = "lbl_imagefile";
            lbl_imagefile.Size = new Size(346, 40);
            lbl_imagefile.TabIndex = 8;
            lbl_imagefile.Text = "Kein Bild";
            //
            // lbl_color
            //
            lbl_color.AutoSize = true;
            lbl_color.Location = new Point(384, 30);
            lbl_color.Name = "lbl_color";
            lbl_color.Size = new Size(36, 15);
            lbl_color.TabIndex = 9;
            lbl_color.Text = "Farbe";
            //
            // flp_colors
            //
            flp_colors.Location = new Point(384, 52);
            flp_colors.Name = "flp_colors";
            flp_colors.Size = new Size(360, 76);
            flp_colors.TabIndex = 10;
            //
            // lbl_icon
            //
            lbl_icon.AutoSize = true;
            lbl_icon.Location = new Point(384, 134);
            lbl_icon.Name = "lbl_icon";
            lbl_icon.Size = new Size(30, 15);
            lbl_icon.TabIndex = 11;
            lbl_icon.Text = "Icon";
            //
            // flp_icons
            //
            flp_icons.Location = new Point(384, 156);
            flp_icons.Name = "flp_icons";
            flp_icons.Size = new Size(360, 116);
            flp_icons.TabIndex = 12;
            //
            // cardMidi
            //
            cardMidi.Controls.Add(lbl_miditrigger);
            cardMidi.Controls.Add(btn_midilearn);
            cardMidi.Controls.Add(btn_midiclear);
            cardMidi.Location = new Point(12, 578);
            cardMidi.Name = "cardMidi";
            cardMidi.Size = new Size(756, 72);
            cardMidi.TabIndex = 3;
            cardMidi.TabStop = false;
            cardMidi.Text = "MIDI-Auslöser";
            //
            // lbl_miditrigger
            //
            lbl_miditrigger.AutoEllipsis = true;
            lbl_miditrigger.Location = new Point(16, 36);
            lbl_miditrigger.Name = "lbl_miditrigger";
            lbl_miditrigger.Size = new Size(490, 18);
            lbl_miditrigger.TabIndex = 0;
            lbl_miditrigger.Text = "Nicht belegt";
            //
            // btn_midilearn
            //
            btn_midilearn.Location = new Point(516, 28);
            btn_midilearn.Name = "btn_midilearn";
            btn_midilearn.Size = new Size(124, 32);
            btn_midilearn.TabIndex = 1;
            btn_midilearn.Text = "Anlernen";
            btn_midilearn.UseVisualStyleBackColor = true;
            btn_midilearn.Click += btn_midilearn_Click;
            //
            // btn_midiclear
            //
            btn_midiclear.Location = new Point(646, 28);
            btn_midiclear.Name = "btn_midiclear";
            btn_midiclear.Size = new Size(94, 32);
            btn_midiclear.TabIndex = 2;
            btn_midiclear.Text = "Entfernen";
            btn_midiclear.UseVisualStyleBackColor = true;
            btn_midiclear.Click += btn_midiclear_Click;
            //
            // btn_reset
            //
            btn_reset.Location = new Point(12, 662);
            btn_reset.Name = "btn_reset";
            btn_reset.Size = new Size(140, 32);
            btn_reset.TabIndex = 3;
            btn_reset.Text = "Zurücksetzen";
            btn_reset.UseVisualStyleBackColor = true;
            btn_reset.Click += btn_reset_Click;
            //
            // btn_cancel
            //
            btn_cancel.DialogResult = DialogResult.Cancel;
            btn_cancel.Location = new Point(552, 662);
            btn_cancel.Name = "btn_cancel";
            btn_cancel.Size = new Size(104, 32);
            btn_cancel.TabIndex = 4;
            btn_cancel.Text = "Abbrechen";
            btn_cancel.UseVisualStyleBackColor = true;
            //
            // btn_ok
            //
            btn_ok.Location = new Point(664, 662);
            btn_ok.Name = "btn_ok";
            btn_ok.Size = new Size(104, 32);
            btn_ok.TabIndex = 5;
            btn_ok.Text = "Übernehmen";
            btn_ok.UseVisualStyleBackColor = true;
            btn_ok.Variant = ButtonVariant.Accent;
            btn_ok.Click += btn_ok_Click;
            //
            // fButtonEdit
            //
            AcceptButton = btn_ok;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btn_cancel;
            ClientSize = new Size(780, 706);
            Controls.Add(cardAction);
            Controls.Add(cardPreview);
            Controls.Add(cardLook);
            Controls.Add(cardMidi);
            Controls.Add(btn_reset);
            Controls.Add(btn_cancel);
            Controls.Add(btn_ok);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fButtonEdit";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "SoundBoard - Button Editieren";
            FormClosing += fButtonEdit_FormClosing;
            cardAction.ResumeLayout(false);
            cardAction.PerformLayout();
            cardPreview.ResumeLayout(false);
            cardLook.ResumeLayout(false);
            cardLook.PerformLayout();
            cardMidi.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private CardGroupBox cardAction;
        private ModernButton btn_modeAudio;
        private ModernButton btn_modeTts;
        private ModernButton btn_modeNone;
        private Label lbl_audio;
        private TextBox txt_audiofile;
        private ModernButton btn_browse;
        private ModernComboBox cmb_texts;
        private TextBox txt_ttstext;
        private MutedLabel lbl_none;
        private ModernButton btn_test;
        private ModernButton btn_stop;
        private CardGroupBox cardPreview;
        private ModernButton btn_preview;
        private CardGroupBox cardLook;
        private Label lbl_title;
        private TextBox txt_title;
        private Label lbl_fontsize;
        private MutedLabel lbl_fontsizevalue;
        private ModernSlider sld_fontsize;
        private Label lbl_image;
        private ModernButton btn_image;
        private ModernButton btn_imageclear;
        private MutedLabel lbl_imagefile;
        private Label lbl_color;
        private FlowLayoutPanel flp_colors;
        private Label lbl_icon;
        private FlowLayoutPanel flp_icons;
        private CardGroupBox cardMidi;
        private MutedLabel lbl_miditrigger;
        private ModernButton btn_midilearn;
        private ModernButton btn_midiclear;
        private ModernButton btn_reset;
        private ModernButton btn_cancel;
        private ModernButton btn_ok;
        private ToolTip toolTip1;
    }
}

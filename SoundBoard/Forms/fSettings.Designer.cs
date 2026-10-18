namespace SoundBoard
{
    partial class fSettings
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
            cardPlayback = new CardGroupBox();
            lbl_voicecaption = new Label();
            cmb_voice = new ModernComboBox();
            lbl_volumecaption = new Label();
            lbl_volume = new MutedLabel();
            sld_volume = new ModernSlider();
            cardMidi = new CardGroupBox();
            lbl_devicecaption = new Label();
            cmb_mididevice = new ModernComboBox();
            btn_midirefresh = new ModernButton();
            lbl_midistatus = new MutedLabel();
            lbl_stopcaption = new Label();
            lbl_stoptrigger = new MutedLabel();
            btn_stoplearn = new ModernButton();
            btn_stopclear = new ModernButton();
            btn_close = new ModernButton();
            cardPlayback.SuspendLayout();
            cardMidi.SuspendLayout();
            SuspendLayout();
            //
            // cardPlayback
            //
            cardPlayback.Controls.Add(lbl_voicecaption);
            cardPlayback.Controls.Add(cmb_voice);
            cardPlayback.Controls.Add(lbl_volumecaption);
            cardPlayback.Controls.Add(lbl_volume);
            cardPlayback.Controls.Add(sld_volume);
            cardPlayback.Location = new Point(12, 12);
            cardPlayback.Name = "cardPlayback";
            cardPlayback.Size = new Size(536, 160);
            cardPlayback.TabIndex = 0;
            cardPlayback.TabStop = false;
            cardPlayback.Text = "Wiedergabe";
            //
            // lbl_voicecaption
            //
            lbl_voicecaption.AutoSize = true;
            lbl_voicecaption.Location = new Point(16, 30);
            lbl_voicecaption.Name = "lbl_voicecaption";
            lbl_voicecaption.Size = new Size(150, 15);
            lbl_voicecaption.TabIndex = 0;
            lbl_voicecaption.Text = "Stimme für Text-to-Speech";
            //
            // cmb_voice
            //
            cmb_voice.Location = new Point(16, 52);
            cmb_voice.Name = "cmb_voice";
            cmb_voice.Size = new Size(504, 28);
            cmb_voice.TabIndex = 1;
            cmb_voice.SelectionChangeCommitted += cmb_voice_SelectionChangeCommitted;
            //
            // lbl_volumecaption
            //
            lbl_volumecaption.AutoSize = true;
            lbl_volumecaption.Location = new Point(16, 98);
            lbl_volumecaption.Name = "lbl_volumecaption";
            lbl_volumecaption.Size = new Size(61, 15);
            lbl_volumecaption.TabIndex = 2;
            lbl_volumecaption.Text = "Lautstärke";
            //
            // lbl_volume
            //
            lbl_volume.Location = new Point(440, 98);
            lbl_volume.Name = "lbl_volume";
            lbl_volume.Size = new Size(80, 15);
            lbl_volume.TabIndex = 3;
            lbl_volume.Text = "100 %";
            lbl_volume.TextAlign = ContentAlignment.TopRight;
            //
            // sld_volume
            //
            sld_volume.Location = new Point(8, 116);
            sld_volume.Name = "sld_volume";
            sld_volume.Size = new Size(520, 34);
            sld_volume.TabIndex = 4;
            sld_volume.Value = 100;
            sld_volume.ValueChanged += sld_volume_ValueChanged;
            //
            // cardMidi
            //
            cardMidi.Controls.Add(lbl_devicecaption);
            cardMidi.Controls.Add(cmb_mididevice);
            cardMidi.Controls.Add(btn_midirefresh);
            cardMidi.Controls.Add(lbl_midistatus);
            cardMidi.Controls.Add(lbl_stopcaption);
            cardMidi.Controls.Add(lbl_stoptrigger);
            cardMidi.Controls.Add(btn_stoplearn);
            cardMidi.Controls.Add(btn_stopclear);
            cardMidi.Location = new Point(12, 184);
            cardMidi.Name = "cardMidi";
            cardMidi.Size = new Size(536, 196);
            cardMidi.TabIndex = 1;
            cardMidi.TabStop = false;
            cardMidi.Text = "MIDI";
            //
            // lbl_devicecaption
            //
            lbl_devicecaption.AutoSize = true;
            lbl_devicecaption.Location = new Point(16, 30);
            lbl_devicecaption.Name = "lbl_devicecaption";
            lbl_devicecaption.Size = new Size(64, 15);
            lbl_devicecaption.TabIndex = 0;
            lbl_devicecaption.Text = "MIDI-Gerät";
            //
            // cmb_mididevice
            //
            cmb_mididevice.Location = new Point(16, 52);
            cmb_mididevice.Name = "cmb_mididevice";
            cmb_mididevice.Size = new Size(372, 28);
            cmb_mididevice.TabIndex = 1;
            cmb_mididevice.SelectionChangeCommitted += cmb_mididevice_SelectionChangeCommitted;
            //
            // btn_midirefresh
            //
            btn_midirefresh.Location = new Point(396, 50);
            btn_midirefresh.Name = "btn_midirefresh";
            btn_midirefresh.Size = new Size(124, 32);
            btn_midirefresh.TabIndex = 2;
            btn_midirefresh.Text = "Aktualisieren";
            btn_midirefresh.UseVisualStyleBackColor = true;
            btn_midirefresh.Click += btn_midirefresh_Click;
            //
            // lbl_midistatus
            //
            lbl_midistatus.AutoEllipsis = true;
            lbl_midistatus.Location = new Point(16, 90);
            lbl_midistatus.Name = "lbl_midistatus";
            lbl_midistatus.Size = new Size(504, 32);
            lbl_midistatus.TabIndex = 3;
            lbl_midistatus.Text = "MIDI: aus";
            //
            // lbl_stopcaption
            //
            lbl_stopcaption.AutoSize = true;
            lbl_stopcaption.Location = new Point(16, 134);
            lbl_stopcaption.Name = "lbl_stopcaption";
            lbl_stopcaption.Size = new Size(95, 15);
            lbl_stopcaption.TabIndex = 4;
            lbl_stopcaption.Text = "Wiedergabe STOP";
            //
            // lbl_stoptrigger
            //
            lbl_stoptrigger.AutoEllipsis = true;
            lbl_stoptrigger.Location = new Point(16, 156);
            lbl_stoptrigger.Name = "lbl_stoptrigger";
            lbl_stoptrigger.Size = new Size(290, 15);
            lbl_stoptrigger.TabIndex = 5;
            lbl_stoptrigger.Text = "Nicht belegt";
            //
            // btn_stoplearn
            //
            btn_stoplearn.Location = new Point(314, 140);
            btn_stoplearn.Name = "btn_stoplearn";
            btn_stoplearn.Size = new Size(110, 32);
            btn_stoplearn.TabIndex = 6;
            btn_stoplearn.Text = "Anlernen";
            btn_stoplearn.UseVisualStyleBackColor = true;
            btn_stoplearn.Click += btn_stoplearn_Click;
            //
            // btn_stopclear
            //
            btn_stopclear.Location = new Point(430, 140);
            btn_stopclear.Name = "btn_stopclear";
            btn_stopclear.Size = new Size(90, 32);
            btn_stopclear.TabIndex = 7;
            btn_stopclear.Text = "Entfernen";
            btn_stopclear.UseVisualStyleBackColor = true;
            btn_stopclear.Click += btn_stopclear_Click;
            //
            // btn_close
            //
            btn_close.DialogResult = DialogResult.Cancel;
            btn_close.Location = new Point(444, 392);
            btn_close.Name = "btn_close";
            btn_close.Size = new Size(104, 32);
            btn_close.TabIndex = 2;
            btn_close.Text = "Schließen";
            btn_close.UseVisualStyleBackColor = true;
            btn_close.Variant = ButtonVariant.Accent;
            //
            // fSettings
            //
            AcceptButton = btn_close;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btn_close;
            ClientSize = new Size(560, 436);
            Controls.Add(cardPlayback);
            Controls.Add(cardMidi);
            Controls.Add(btn_close);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fSettings";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "SoundBoard - Einstellungen";
            FormClosing += fSettings_FormClosing;
            cardPlayback.ResumeLayout(false);
            cardPlayback.PerformLayout();
            cardMidi.ResumeLayout(false);
            cardMidi.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private CardGroupBox cardPlayback;
        private Label lbl_voicecaption;
        private ModernComboBox cmb_voice;
        private Label lbl_volumecaption;
        private MutedLabel lbl_volume;
        private ModernSlider sld_volume;
        private CardGroupBox cardMidi;
        private Label lbl_devicecaption;
        private ModernComboBox cmb_mididevice;
        private ModernButton btn_midirefresh;
        private MutedLabel lbl_midistatus;
        private Label lbl_stopcaption;
        private MutedLabel lbl_stoptrigger;
        private ModernButton btn_stoplearn;
        private ModernButton btn_stopclear;
        private ModernButton btn_close;
    }
}

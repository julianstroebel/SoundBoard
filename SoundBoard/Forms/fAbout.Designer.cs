namespace SoundBoard
{
    partial class fAbout
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
            lbl_title = new Label();
            lbl_version = new MutedLabel();
            lbl_info = new Label();
            lnk_github = new LinkLabel();
            cardLibraries = new CardGroupBox();
            lbl_company = new MutedLabel();
            btn_close = new ModernButton();
            SuspendLayout();
            //
            // lbl_title
            //
            lbl_title.AutoSize = true;
            lbl_title.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_title.Location = new Point(20, 16);
            lbl_title.Name = "lbl_title";
            lbl_title.Size = new Size(187, 40);
            lbl_title.TabIndex = 0;
            lbl_title.Text = "SoundBoard";
            //
            // lbl_version
            //
            lbl_version.AutoSize = true;
            lbl_version.Location = new Point(24, 58);
            lbl_version.Name = "lbl_version";
            lbl_version.Size = new Size(63, 15);
            lbl_version.TabIndex = 1;
            lbl_version.Text = "Version 1.1";
            //
            // lbl_info
            //
            lbl_info.Location = new Point(24, 88);
            lbl_info.Name = "lbl_info";
            lbl_info.Size = new Size(472, 50);
            lbl_info.TabIndex = 2;
            lbl_info.Text = "Soundboard für Veranstaltungen: 16 frei belegbare Buttons für Audio-Dateien und Text-to-Speech, natürliche Offline-Sprachausgabe und Steuerung per MIDI-Controller.";
            //
            // lnk_github
            //
            lnk_github.AutoSize = true;
            lnk_github.Location = new Point(24, 144);
            lnk_github.Name = "lnk_github";
            lnk_github.Size = new Size(260, 15);
            lnk_github.TabIndex = 3;
            lnk_github.TabStop = true;
            lnk_github.Text = "GitHub: github.com/julianstroebel/SoundBoard";
            lnk_github.LinkClicked += lnk_github_LinkClicked;
            //
            // cardLibraries
            //
            cardLibraries.Location = new Point(12, 176);
            cardLibraries.Name = "cardLibraries";
            cardLibraries.Size = new Size(496, 322);
            cardLibraries.TabIndex = 4;
            cardLibraries.TabStop = false;
            cardLibraries.Text = "Verwendete Bibliotheken und Tools";
            //
            // lbl_company
            //
            lbl_company.AutoSize = true;
            lbl_company.Location = new Point(16, 518);
            lbl_company.Name = "lbl_company";
            lbl_company.Size = new Size(115, 15);
            lbl_company.TabIndex = 5;
            lbl_company.Text = "Ströbel Eventtechnik";
            //
            // btn_close
            //
            btn_close.DialogResult = DialogResult.Cancel;
            btn_close.Location = new Point(404, 510);
            btn_close.Name = "btn_close";
            btn_close.Size = new Size(104, 32);
            btn_close.TabIndex = 6;
            btn_close.Text = "Schließen";
            btn_close.UseVisualStyleBackColor = true;
            btn_close.Variant = ButtonVariant.Accent;
            //
            // fAbout
            //
            AcceptButton = btn_close;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btn_close;
            ClientSize = new Size(520, 554);
            Controls.Add(lbl_title);
            Controls.Add(lbl_version);
            Controls.Add(lbl_info);
            Controls.Add(lnk_github);
            Controls.Add(cardLibraries);
            Controls.Add(lbl_company);
            Controls.Add(btn_close);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fAbout";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "SoundBoard - Über";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbl_title;
        private MutedLabel lbl_version;
        private Label lbl_info;
        private LinkLabel lnk_github;
        private CardGroupBox cardLibraries;
        private MutedLabel lbl_company;
        private ModernButton btn_close;
    }
}

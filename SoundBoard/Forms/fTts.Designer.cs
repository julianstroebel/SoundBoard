namespace SoundBoard
{
    partial class fTts
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
            card = new CardGroupBox();
            cmb_texts = new ModernComboBox();
            txt_tts = new TextBox();
            btn_play = new ModernButton();
            btn_stop = new ModernButton();
            btn_save = new ModernButton();
            btn_delete = new ModernButton();
            btn_assign = new ModernButton();
            card.SuspendLayout();
            SuspendLayout();
            //
            // card
            //
            card.Controls.Add(cmb_texts);
            card.Controls.Add(txt_tts);
            card.Controls.Add(btn_play);
            card.Controls.Add(btn_stop);
            card.Controls.Add(btn_save);
            card.Controls.Add(btn_delete);
            card.Controls.Add(btn_assign);
            card.Location = new Point(12, 12);
            card.Name = "card";
            card.Size = new Size(496, 252);
            card.TabIndex = 0;
            card.TabStop = false;
            card.Text = "Text-to-Speech";
            //
            // cmb_texts
            //
            cmb_texts.Location = new Point(16, 28);
            cmb_texts.Name = "cmb_texts";
            cmb_texts.Size = new Size(320, 28);
            cmb_texts.TabIndex = 0;
            cmb_texts.SelectedIndexChanged += cmb_texts_SelectedIndexChanged;
            //
            // txt_tts
            //
            txt_tts.AcceptsReturn = true;
            txt_tts.BorderStyle = BorderStyle.None;
            txt_tts.Location = new Point(24, 72);
            txt_tts.Multiline = true;
            txt_tts.Name = "txt_tts";
            txt_tts.PlaceholderText = "Text eingeben, der gesprochen werden soll …";
            txt_tts.ScrollBars = ScrollBars.Vertical;
            txt_tts.Size = new Size(304, 160);
            txt_tts.TabIndex = 1;
            //
            // btn_play
            //
            btn_play.Location = new Point(348, 28);
            btn_play.Name = "btn_play";
            btn_play.Size = new Size(132, 54);
            btn_play.TabIndex = 2;
            btn_play.Text = "▶  Abspielen";
            btn_play.UseVisualStyleBackColor = true;
            btn_play.Variant = ButtonVariant.Accent;
            btn_play.Click += btn_play_Click;
            //
            // btn_stop
            //
            btn_stop.Location = new Point(348, 88);
            btn_stop.Name = "btn_stop";
            btn_stop.Size = new Size(132, 32);
            btn_stop.TabIndex = 3;
            btn_stop.Text = "■  Stop";
            btn_stop.UseVisualStyleBackColor = true;
            btn_stop.Click += btn_stop_Click;
            //
            // btn_save
            //
            btn_save.Location = new Point(348, 126);
            btn_save.Name = "btn_save";
            btn_save.Size = new Size(132, 32);
            btn_save.TabIndex = 4;
            btn_save.Text = "Text speichern";
            btn_save.UseVisualStyleBackColor = true;
            btn_save.Click += btn_save_Click;
            //
            // btn_delete
            //
            btn_delete.Location = new Point(348, 164);
            btn_delete.Name = "btn_delete";
            btn_delete.Size = new Size(132, 32);
            btn_delete.TabIndex = 5;
            btn_delete.Text = "Text löschen";
            btn_delete.UseVisualStyleBackColor = true;
            btn_delete.Click += btn_delete_Click;
            //
            // btn_assign
            //
            btn_assign.Location = new Point(348, 202);
            btn_assign.Name = "btn_assign";
            btn_assign.Size = new Size(132, 32);
            btn_assign.TabIndex = 6;
            btn_assign.Text = "Auf Button legen …";
            btn_assign.UseVisualStyleBackColor = true;
            btn_assign.Click += btn_assign_Click;
            //
            // fTts
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(520, 276);
            Controls.Add(card);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fTts";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "Text-to-Speech";
            card.ResumeLayout(false);
            card.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private CardGroupBox card;
        private ModernComboBox cmb_texts;
        private TextBox txt_tts;
        private ModernButton btn_play;
        private ModernButton btn_stop;
        private ModernButton btn_save;
        private ModernButton btn_delete;
        private ModernButton btn_assign;
    }
}

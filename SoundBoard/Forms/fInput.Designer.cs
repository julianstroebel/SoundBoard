namespace SoundBoard
{
    partial class fInput
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
            lbl_prompt = new Label();
            txt_value = new TextBox();
            btn_cancel = new ModernButton();
            btn_ok = new ModernButton();
            card.SuspendLayout();
            SuspendLayout();
            //
            // card
            //
            card.Controls.Add(lbl_prompt);
            card.Controls.Add(txt_value);
            card.Location = new Point(12, 12);
            card.Name = "card";
            card.Size = new Size(376, 86);
            card.TabIndex = 0;
            card.TabStop = false;
            //
            // lbl_prompt
            //
            lbl_prompt.AutoSize = true;
            lbl_prompt.Location = new Point(16, 16);
            lbl_prompt.Name = "lbl_prompt";
            lbl_prompt.Size = new Size(42, 15);
            lbl_prompt.TabIndex = 0;
            lbl_prompt.Text = "Name:";
            //
            // txt_value
            //
            txt_value.BorderStyle = BorderStyle.None;
            txt_value.Location = new Point(24, 48);
            txt_value.Name = "txt_value";
            txt_value.Size = new Size(328, 16);
            txt_value.TabIndex = 1;
            //
            // btn_cancel
            //
            btn_cancel.DialogResult = DialogResult.Cancel;
            btn_cancel.Location = new Point(176, 110);
            btn_cancel.Name = "btn_cancel";
            btn_cancel.Size = new Size(100, 32);
            btn_cancel.TabIndex = 2;
            btn_cancel.Text = "Abbrechen";
            btn_cancel.UseVisualStyleBackColor = true;
            //
            // btn_ok
            //
            btn_ok.DialogResult = DialogResult.OK;
            btn_ok.Location = new Point(288, 110);
            btn_ok.Name = "btn_ok";
            btn_ok.Size = new Size(100, 32);
            btn_ok.TabIndex = 1;
            btn_ok.Text = "OK";
            btn_ok.UseVisualStyleBackColor = true;
            btn_ok.Variant = ButtonVariant.Accent;
            //
            // fInput
            //
            AcceptButton = btn_ok;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btn_cancel;
            ClientSize = new Size(400, 154);
            Controls.Add(card);
            Controls.Add(btn_cancel);
            Controls.Add(btn_ok);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fInput";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Eingabe";
            card.ResumeLayout(false);
            card.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private CardGroupBox card;
        private Label lbl_prompt;
        private TextBox txt_value;
        private ModernButton btn_cancel;
        private ModernButton btn_ok;
    }
}

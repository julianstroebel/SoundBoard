namespace SoundBoard
{
    partial class fMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            dateiToolStripMenuItem = new ToolStripMenuItem();
            btn_einstellungen = new ToolStripMenuItem();
            btn_close = new ToolStripMenuItem();
            bearbeitenToolStripMenuItem = new ToolStripMenuItem();
            btn_editmodetoggle = new ToolStripMenuItem();
            hilfeToolStripMenuItem = new ToolStripMenuItem();
            btn_about = new ToolStripMenuItem();
            btn_themetoggle = new ToolStripMenuItem();
            btn_stopmenu = new ToolStripMenuItem();
            btn_ttsmenu = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            lbl_editmode = new ToolStripStatusLabel();
            lbl_status = new ToolStripStatusLabel();
            lbl_midi = new ToolStripStatusLabel();
            groupBox1 = new CardGroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            button1 = new ModernButton();
            button2 = new ModernButton();
            button3 = new ModernButton();
            button4 = new ModernButton();
            button5 = new ModernButton();
            button6 = new ModernButton();
            button7 = new ModernButton();
            button8 = new ModernButton();
            button9 = new ModernButton();
            button10 = new ModernButton();
            button11 = new ModernButton();
            button12 = new ModernButton();
            button13 = new ModernButton();
            button14 = new ModernButton();
            button15 = new ModernButton();
            button16 = new ModernButton();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            //
            // menuStrip1
            //
            menuStrip1.Items.AddRange(new ToolStripItem[] { dateiToolStripMenuItem, bearbeitenToolStripMenuItem, hilfeToolStripMenuItem, btn_themetoggle, btn_stopmenu, btn_ttsmenu });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(925, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            //
            // dateiToolStripMenuItem
            //
            dateiToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { btn_einstellungen, btn_close });
            dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            dateiToolStripMenuItem.Size = new Size(46, 20);
            dateiToolStripMenuItem.Text = "Datei";
            //
            // btn_einstellungen
            //
            btn_einstellungen.Name = "btn_einstellungen";
            btn_einstellungen.ShortcutKeys = Keys.Control | Keys.Oemcomma;
            btn_einstellungen.ShortcutKeyDisplayString = "Strg+,";
            btn_einstellungen.Size = new Size(200, 22);
            btn_einstellungen.Text = "Einstellungen …";
            btn_einstellungen.Click += btn_einstellungen_Click;
            //
            // btn_close
            //
            btn_close.Name = "btn_close";
            btn_close.Size = new Size(200, 22);
            btn_close.Text = "Beenden";
            btn_close.Click += btn_close_Click;
            //
            // bearbeitenToolStripMenuItem
            //
            bearbeitenToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { btn_editmodetoggle });
            bearbeitenToolStripMenuItem.Name = "bearbeitenToolStripMenuItem";
            bearbeitenToolStripMenuItem.Size = new Size(75, 20);
            bearbeitenToolStripMenuItem.Text = "Bearbeiten";
            //
            // btn_editmodetoggle
            //
            btn_editmodetoggle.Name = "btn_editmodetoggle";
            btn_editmodetoggle.ShortcutKeys = Keys.Control | Keys.E;
            btn_editmodetoggle.Size = new Size(210, 22);
            btn_editmodetoggle.Text = "Editierungsmodus";
            btn_editmodetoggle.Click += btn_editmodetoggle_Click;
            //
            // hilfeToolStripMenuItem
            //
            hilfeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { btn_about });
            hilfeToolStripMenuItem.Name = "hilfeToolStripMenuItem";
            hilfeToolStripMenuItem.Size = new Size(44, 20);
            hilfeToolStripMenuItem.Text = "Hilfe";
            //
            // btn_about
            //
            btn_about.Name = "btn_about";
            btn_about.Size = new Size(99, 22);
            btn_about.Text = "Über";
            btn_about.Click += btn_about_Click;
            //
            // btn_themetoggle
            //
            btn_themetoggle.Alignment = ToolStripItemAlignment.Right;
            btn_themetoggle.Name = "btn_themetoggle";
            btn_themetoggle.Size = new Size(100, 20);
            btn_themetoggle.Text = "Dark Mode";
            btn_themetoggle.ToolTipText = "Zwischen Light und Dark Mode wechseln";
            btn_themetoggle.Click += btn_themetoggle_Click;
            //
            // btn_stopmenu
            //
            btn_stopmenu.Alignment = ToolStripItemAlignment.Right;
            btn_stopmenu.Name = "btn_stopmenu";
            btn_stopmenu.Size = new Size(140, 20);
            btn_stopmenu.Text = "Wiedergabe STOP";
            btn_stopmenu.ToolTipText = "Wiedergabe stoppen (Esc)";
            btn_stopmenu.Click += btn_stopmenu_Click;
            //
            // btn_ttsmenu
            //
            btn_ttsmenu.Alignment = ToolStripItemAlignment.Right;
            btn_ttsmenu.Name = "btn_ttsmenu";
            btn_ttsmenu.Size = new Size(120, 20);
            btn_ttsmenu.Text = "Text-to-Speech";
            btn_ttsmenu.ToolTipText = "Fenster für Text-to-Speech öffnen (Strg+T)";
            btn_ttsmenu.Click += btn_ttsmenu_Click;
            //
            // statusStrip1
            //
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, lbl_editmode, lbl_status, lbl_midi });
            statusStrip1.Location = new Point(0, 598);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(925, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            //
            // toolStripStatusLabel1
            //
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(182, 17);
            toolStripStatusLabel1.Text = "Version 1.0 - Ströbel Eventtechnik";
            //
            // lbl_editmode
            //
            lbl_editmode.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_editmode.ForeColor = Color.Red;
            lbl_editmode.Name = "lbl_editmode";
            lbl_editmode.Size = new Size(141, 17);
            lbl_editmode.Text = "Editierungsmodus aktiv – Button anklicken zum Bearbeiten";
            //
            // lbl_status
            //
            lbl_status.Name = "lbl_status";
            lbl_status.Size = new Size(400, 17);
            lbl_status.Spring = true;
            lbl_status.TextAlign = ContentAlignment.MiddleRight;
            //
            // lbl_midi
            //
            lbl_midi.Margin = new Padding(12, 3, 0, 2);
            lbl_midi.Name = "lbl_midi";
            lbl_midi.Size = new Size(60, 17);
            lbl_midi.Text = "MIDI: aus";
            //
            // groupBox1
            //
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(tableLayoutPanel1);
            groupBox1.Location = new Point(12, 36);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(10, 6, 10, 10);
            groupBox1.Size = new Size(901, 552);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Direktwahl Buttons";
            //
            // tableLayoutPanel1
            //
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(button1, 0, 0);
            tableLayoutPanel1.Controls.Add(button2, 1, 0);
            tableLayoutPanel1.Controls.Add(button3, 2, 0);
            tableLayoutPanel1.Controls.Add(button4, 3, 0);
            tableLayoutPanel1.Controls.Add(button5, 0, 1);
            tableLayoutPanel1.Controls.Add(button6, 1, 1);
            tableLayoutPanel1.Controls.Add(button7, 2, 1);
            tableLayoutPanel1.Controls.Add(button8, 3, 1);
            tableLayoutPanel1.Controls.Add(button9, 0, 2);
            tableLayoutPanel1.Controls.Add(button10, 1, 2);
            tableLayoutPanel1.Controls.Add(button11, 2, 2);
            tableLayoutPanel1.Controls.Add(button12, 3, 2);
            tableLayoutPanel1.Controls.Add(button13, 0, 3);
            tableLayoutPanel1.Controls.Add(button14, 1, 3);
            tableLayoutPanel1.Controls.Add(button15, 2, 3);
            tableLayoutPanel1.Controls.Add(button16, 3, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(10, 22);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Size = new Size(881, 520);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // button16
            // 
            button16.Dock = DockStyle.Fill;
            button16.Margin = new Padding(3);
            button16.Name = "button16";
            button16.TabIndex = 15;
            button16.Text = "Button 16";
            button16.UseVisualStyleBackColor = true;
            button16.Click += button16_Click;
            // 
            // button15
            // 
            button15.Dock = DockStyle.Fill;
            button15.Margin = new Padding(3);
            button15.Name = "button15";
            button15.TabIndex = 14;
            button15.Text = "Button 15";
            button15.UseVisualStyleBackColor = true;
            button15.Click += button15_Click;
            // 
            // button14
            // 
            button14.Dock = DockStyle.Fill;
            button14.Margin = new Padding(3);
            button14.Name = "button14";
            button14.TabIndex = 13;
            button14.Text = "Button 14";
            button14.UseVisualStyleBackColor = true;
            button14.Click += button14_Click;
            // 
            // button13
            // 
            button13.Dock = DockStyle.Fill;
            button13.Margin = new Padding(3);
            button13.Name = "button13";
            button13.TabIndex = 12;
            button13.Text = "Button 13";
            button13.UseVisualStyleBackColor = true;
            button13.Click += button13_Click;
            // 
            // button12
            // 
            button12.Dock = DockStyle.Fill;
            button12.Margin = new Padding(3);
            button12.Name = "button12";
            button12.TabIndex = 11;
            button12.Text = "Button 12";
            button12.UseVisualStyleBackColor = true;
            button12.Click += button12_Click;
            // 
            // button11
            // 
            button11.Dock = DockStyle.Fill;
            button11.Margin = new Padding(3);
            button11.Name = "button11";
            button11.TabIndex = 10;
            button11.Text = "Button 11";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click;
            // 
            // button10
            // 
            button10.Dock = DockStyle.Fill;
            button10.Margin = new Padding(3);
            button10.Name = "button10";
            button10.TabIndex = 9;
            button10.Text = "Button 10";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // button9
            // 
            button9.Dock = DockStyle.Fill;
            button9.Margin = new Padding(3);
            button9.Name = "button9";
            button9.TabIndex = 8;
            button9.Text = "Button 9";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button8
            // 
            button8.Dock = DockStyle.Fill;
            button8.Margin = new Padding(3);
            button8.Name = "button8";
            button8.TabIndex = 7;
            button8.Text = "Button 8";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button7
            // 
            button7.Dock = DockStyle.Fill;
            button7.Margin = new Padding(3);
            button7.Name = "button7";
            button7.TabIndex = 6;
            button7.Text = "Button 7";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button6
            // 
            button6.Dock = DockStyle.Fill;
            button6.Margin = new Padding(3);
            button6.Name = "button6";
            button6.TabIndex = 5;
            button6.Text = "Button 6";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button5
            // 
            button5.Dock = DockStyle.Fill;
            button5.Margin = new Padding(3);
            button5.Name = "button5";
            button5.TabIndex = 4;
            button5.Text = "Button 5";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button4
            // 
            button4.Dock = DockStyle.Fill;
            button4.Margin = new Padding(3);
            button4.Name = "button4";
            button4.TabIndex = 3;
            button4.Text = "Button 4";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.Dock = DockStyle.Fill;
            button3.Margin = new Padding(3);
            button3.Name = "button3";
            button3.TabIndex = 2;
            button3.Text = "Button 3";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Fill;
            button2.Margin = new Padding(3);
            button2.Name = "button2";
            button2.TabIndex = 1;
            button2.Text = "Button 2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.Margin = new Padding(3);
            button1.Name = "button1";
            button1.TabIndex = 0;
            button1.Text = "Button 1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            //
            // fMain
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(925, 620);
            Controls.Add(groupBox1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            KeyPreview = true;
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(640, 480);
            Name = "fMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SoundBoard";
            FormClosing += fMain_FormClosing;
            Load += fMain_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem dateiToolStripMenuItem;
        private ToolStripMenuItem btn_einstellungen;
        private ToolStripMenuItem btn_close;
        private ToolStripMenuItem bearbeitenToolStripMenuItem;
        private ToolStripMenuItem btn_editmodetoggle;
        private ToolStripMenuItem hilfeToolStripMenuItem;
        private ToolStripMenuItem btn_about;
        private ToolStripMenuItem btn_themetoggle;
        private ToolStripMenuItem btn_stopmenu;
        private ToolStripMenuItem btn_ttsmenu;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel lbl_editmode;
        private ToolStripStatusLabel lbl_status;
        private ToolStripStatusLabel lbl_midi;
        private CardGroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private ModernButton button1;
        private ModernButton button2;
        private ModernButton button3;
        private ModernButton button4;
        private ModernButton button5;
        private ModernButton button6;
        private ModernButton button7;
        private ModernButton button8;
        private ModernButton button9;
        private ModernButton button10;
        private ModernButton button11;
        private ModernButton button12;
        private ModernButton button13;
        private ModernButton button14;
        private ModernButton button15;
        private ModernButton button16;
    }
}

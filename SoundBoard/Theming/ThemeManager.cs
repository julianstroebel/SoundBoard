using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace SoundBoard
{
    /// <summary>
    ///  Wendet das aktuelle Theme rekursiv auf Forms und Controls an.
    /// </summary>
    internal static class ThemeManager
    {
        public static Theme Current { get; set; } = Theme.Light;

        public static void Apply(Control root)
        {
            Theme t = Current;

            switch (root)
            {
                case Form form:
                    form.BackColor = t.Back;
                    form.ForeColor = t.Text;
                    WhenHandle(form, () => setDarkTitleBar(form.Handle, t.IsDark));
                    break;

                case ToolStrip strip:
                    applyToolStrip(strip, t);
                    break;

                case CardGroupBox card:
                    card.BackColor = t.Card;
                    card.ForeColor = t.Text;
                    break;

                case ModernButton:
                    break;

                case Button button:
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = t.Border;
                    button.FlatAppearance.MouseOverBackColor = t.SurfaceHover;
                    button.FlatAppearance.MouseDownBackColor = t.SurfacePressed;
                    button.BackColor = t.Surface;
                    button.ForeColor = t.Text;
                    break;

                case TextBox textBox:
                    // In Karten zeichnet die CardGroupBox ein abgerundetes Eingabefeld
                    textBox.BorderStyle = textBox.Parent is CardGroupBox ? BorderStyle.None : BorderStyle.FixedSingle;
                    textBox.BackColor = t.Input;
                    textBox.ForeColor = t.Text;
                    WhenHandle(textBox, () => SetWindowTheme(textBox.Handle, t.IsDark ? "DarkMode_Explorer" : "Explorer", null));
                    break;

                case ModernComboBox modernComboBox:
                    modernComboBox.BackColor = t.Input;
                    modernComboBox.ForeColor = t.Text;
                    WhenHandle(modernComboBox, () => SetWindowTheme(modernComboBox.Handle, t.IsDark ? "DarkMode_Explorer" : "Explorer", null));
                    break;

                case ComboBox comboBox:
                    comboBox.FlatStyle = FlatStyle.Flat;
                    comboBox.BackColor = t.Input;
                    comboBox.ForeColor = t.Text;
                    WhenHandle(comboBox, () => SetWindowTheme(comboBox.Handle, t.IsDark ? "DarkMode_CFD" : "CFD", null));
                    break;

                case Panel panel:
                    panel.BackColor = panel.Parent?.BackColor ?? t.Back;
                    break;

                case LinkLabel linkLabel:
                    linkLabel.BackColor = Color.Transparent;
                    linkLabel.ForeColor = t.Text;
                    linkLabel.LinkColor = t.Accent;
                    linkLabel.ActiveLinkColor = t.AccentPressed;
                    linkLabel.VisitedLinkColor = t.Accent;
                    linkLabel.LinkBehavior = LinkBehavior.HoverUnderline;
                    break;

                case MutedLabel mutedLabel:
                    mutedLabel.BackColor = Color.Transparent;
                    mutedLabel.ForeColor = t.TextMuted;
                    break;

                case Label label:
                    label.BackColor = Color.Transparent;
                    label.ForeColor = t.Text;
                    break;
            }

            foreach (Control child in root.Controls)
            {
                Apply(child);
            }

            root.Invalidate(true);
        }

        public static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = Math.Max(1, radius * 2);
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static void applyToolStrip(ToolStrip strip, Theme t)
        {
            var renderer = new ThemedToolStripRenderer(t);
            strip.Renderer = renderer;
            strip.BackColor = t.Bar;
            strip.ForeColor = t.Text;
            applyItems(strip.Items, renderer, t);
        }

        private static void applyItems(ToolStripItemCollection items, ToolStripRenderer renderer, Theme t)
        {
            foreach (ToolStripItem item in items)
            {
                item.ForeColor = t.Text;
                if (item is ToolStripDropDownItem dropDownItem)
                {
                    dropDownItem.DropDown.Renderer = renderer;
                    dropDownItem.DropDown.BackColor = t.Bar;
                    applyItems(dropDownItem.DropDownItems, renderer, t);
                }
            }
        }

        private static void WhenHandle(Control control, Action action)
        {
            if (control.IsHandleCreated)
            {
                action();
                return;
            }

            void handler(object? s, EventArgs e)
            {
                control.HandleCreated -= handler;
                action();
            }
            control.HandleCreated += handler;
        }

        private static void setDarkTitleBar(IntPtr hwnd, bool dark)
        {
            const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
            int value = dark ? 1 : 0;
            DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, sizeof(int));
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string? pszSubAppName, string? pszSubIdList);
    }
}

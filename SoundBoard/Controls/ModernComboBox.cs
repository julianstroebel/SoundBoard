using System.Drawing.Drawing2D;

namespace SoundBoard
{
    /// <summary>
    ///  Auswahlliste im Stil der Eingabefelder: abgerundet, ohne Systemrahmen, Liste in Theme-Farben.
    ///  Einträge, die keine Strings sind, gelten als echte Auswahl; ein String-Eintrag (z.B. Hinweis an
    ///  Position 0) wird gedämpft dargestellt.
    /// </summary>
    public class ModernComboBox : ComboBox
    {
        private const int WM_PAINT = 0x000F;
        private bool hover;

        public ModernComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
            FlatStyle = FlatStyle.Flat;
            ItemHeight = 22;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Theme t = ThemeManager.Current;
            if (e.Index < 0)
            {
                return;
            }

            bool selected = (e.State & DrawItemState.Selected) != 0 && (e.State & DrawItemState.ComboBoxEdit) == 0;
            using (var brush = new SolidBrush(selected ? t.SurfaceHover : t.Input))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            object? item = Items[e.Index];
            Rectangle textRect = Rectangle.Inflate(e.Bounds, -LogicalToDeviceUnits(6), 0);
            TextRenderer.DrawText(e.Graphics, GetItemText(item), Font, textRect, item is string ? t.TextMuted : t.Text,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_PAINT)
            {
                using Graphics g = Graphics.FromHwnd(Handle);
                paintBox(g);
            }
        }

        /// <summary>
        ///  Übermalt das geschlossene Feld komplett mit eigenem Look.
        /// </summary>
        private void paintBox(Graphics g)
        {
            Theme t = ThemeManager.Current;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? t.Card);

            bool active = Focused || DroppedDown;
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = ThemeManager.RoundedRect(rect, LogicalToDeviceUnits(6)))
            using (var brush = new SolidBrush(hover && Enabled && !active ? blendHover(t) : t.Input))
            using (var pen = new Pen(active ? t.Accent : t.Border, active ? 2f : 1f))
            {
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }

            int chevronWidth = LogicalToDeviceUnits(28);
            var textRect = new Rectangle(LogicalToDeviceUnits(8), 0, Width - chevronWidth - LogicalToDeviceUnits(8), Height);
            object? item = SelectedItem;
            Color textColor = !Enabled || item is string || item == null ? t.TextMuted : t.Text;
            TextRenderer.DrawText(g, item == null ? "" : GetItemText(item), Font, textRect, textColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            using var chevronFont = new Font(IconFont.Family, LogicalToDeviceUnits(10), GraphicsUnit.Pixel);
            var chevronRect = new Rectangle(Width - chevronWidth, 0, chevronWidth, Height);
            TextRenderer.DrawText(g, "", chevronFont, chevronRect, t.TextMuted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        private static Color blendHover(Theme t) => t.IsDark ? t.Surface : t.Back;

        protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hover = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }
        protected override void OnDropDown(EventArgs e) { Invalidate(); base.OnDropDown(e); }
        protected override void OnDropDownClosed(EventArgs e) { Invalidate(); base.OnDropDownClosed(e); }
        protected override void OnSelectedIndexChanged(EventArgs e) { Invalidate(); base.OnSelectedIndexChanged(e); }
        protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }
    }
}

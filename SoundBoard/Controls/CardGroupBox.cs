using System.Drawing.Drawing2D;

namespace SoundBoard
{
    /// <summary>
    ///  GroupBox im Karten-Look: abgerundete Fläche mit dezentem Rahmen und Titel.
    ///  Rahmenlose TextBoxen darin bekommen ein abgerundetes Eingabefeld gezeichnet.
    /// </summary>
    public class CardGroupBox : GroupBox
    {
        private Font? titleFont;

        public CardGroupBox()
        {
            SetStyle(ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            titleFont?.Dispose();
            titleFont = null;
            base.OnFontChanged(e);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control is TextBox textBox)
            {
                textBox.GotFocus += textBox_Changed;
                textBox.LostFocus += textBox_Changed;
                textBox.VisibleChanged += textBox_Changed;
                textBox.LocationChanged += textBox_Changed;
                textBox.SizeChanged += textBox_Changed;
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (e.Control is TextBox textBox)
            {
                textBox.GotFocus -= textBox_Changed;
                textBox.LostFocus -= textBox_Changed;
                textBox.VisibleChanged -= textBox_Changed;
                textBox.LocationChanged -= textBox_Changed;
                textBox.SizeChanged -= textBox_Changed;
            }
        }

        private void textBox_Changed(object? sender, EventArgs e) => Invalidate();

        /// <summary>
        ///  Fläche des gezeichneten Eingabefelds um eine rahmenlose TextBox.
        /// </summary>
        private Rectangle inputFrame(TextBox textBox)
        {
            return Rectangle.Inflate(textBox.Bounds, LogicalToDeviceUnits(8), LogicalToDeviceUnits(6));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            // Klick auf den Rand des Eingabefelds fokussiert die TextBox
            foreach (TextBox textBox in Controls.OfType<TextBox>())
            {
                if (textBox.Visible && textBox.BorderStyle == BorderStyle.None && inputFrame(textBox).Contains(e.Location))
                {
                    textBox.Focus();
                    return;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Theme t = ThemeManager.Current;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? t.Back);

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = ThemeManager.RoundedRect(rect, LogicalToDeviceUnits(10)))
            using (var brush = new SolidBrush(BackColor))
            using (var pen = new Pen(t.Border))
            {
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }

            if (!string.IsNullOrEmpty(Text))
            {
                titleFont ??= new Font(Font, FontStyle.Bold);
                TextRenderer.DrawText(g, Text.ToUpperInvariant(), titleFont,
                    new Point(LogicalToDeviceUnits(14), LogicalToDeviceUnits(4)), t.TextMuted);
            }

            foreach (TextBox textBox in Controls.OfType<TextBox>())
            {
                if (!textBox.Visible || textBox.BorderStyle != BorderStyle.None)
                {
                    continue;
                }

                using GraphicsPath path = ThemeManager.RoundedRect(inputFrame(textBox), LogicalToDeviceUnits(6));
                using var brush = new SolidBrush(textBox.BackColor);
                using var pen = new Pen(textBox.Focused ? t.Accent : t.Border, textBox.Focused ? 2f : 1f);
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                titleFont?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

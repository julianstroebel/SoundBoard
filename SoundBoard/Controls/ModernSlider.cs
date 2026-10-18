using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace SoundBoard
{
    /// <summary>
    ///  Schieberegler im Windows-11-Stil als Ersatz für die TrackBar.
    /// </summary>
    [DefaultEvent(nameof(ValueChanged))]
    public class ModernSlider : Control
    {
        private int minimum = 0;
        private int maximum = 100;
        private int value = 0;
        private bool hover;
        private bool dragging;

        public event EventHandler? ValueChanged;

        public ModernSlider()
        {
            SetStyle(ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.Selectable, true);
            TabStop = true;
            Cursor = Cursors.Hand;
            Size = new Size(200, 32);
        }

        [Category("Behavior"), DefaultValue(0)]
        public int Minimum
        {
            get => minimum;
            set { minimum = value; maximum = Math.Max(maximum, value); Value = this.value; Invalidate(); }
        }

        [Category("Behavior"), DefaultValue(100)]
        public int Maximum
        {
            get => maximum;
            set { maximum = value; minimum = Math.Min(minimum, value); Value = this.value; Invalidate(); }
        }

        [Category("Behavior"), DefaultValue(0)]
        public int Value
        {
            get => value;
            set
            {
                int clamped = Math.Clamp(value, minimum, maximum);
                if (clamped == this.value)
                {
                    return;
                }
                this.value = clamped;
                Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private int thumbRadius => LogicalToDeviceUnits(10);

        private float ratio => maximum == minimum ? 0f : (float)(value - minimum) / (maximum - minimum);

        protected override void OnPaint(PaintEventArgs e)
        {
            Theme t = ThemeManager.Current;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? t.Card);

            int r = thumbRadius;
            int trackHeight = LogicalToDeviceUnits(4);
            int cy = Height / 2;
            int left = r;
            int right = Width - r;
            float thumbX = left + (right - left) * ratio;

            using (var track = new Pen(t.SurfacePressed, trackHeight) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                g.DrawLine(track, left, cy, right, cy);
            }
            using (var fill = new Pen(Enabled ? t.Accent : t.TextMuted, trackHeight) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                g.DrawLine(fill, left, cy, thumbX, cy);
            }

            var outer = new RectangleF(thumbX - r, cy - r, r * 2, r * 2);
            using (var brush = new SolidBrush(t.Card))
            using (var pen = new Pen(t.Border))
            {
                g.FillEllipse(brush, outer);
                g.DrawEllipse(pen, outer);
            }

            float inner = r * (dragging ? 0.45f : hover ? 0.7f : 0.55f);
            using (var brush = new SolidBrush(Enabled ? t.Accent : t.TextMuted))
            {
                g.FillEllipse(brush, thumbX - inner, cy - inner, inner * 2, inner * 2);
            }

            if (Focused && ShowFocusCues)
            {
                using var focus = new Pen(t.Accent) { DashStyle = DashStyle.Dot };
                g.DrawRectangle(focus, 0, 0, Width - 1, Height - 1);
            }
        }

        private void setFromX(int x)
        {
            int r = thumbRadius;
            float pos = Math.Clamp((float)(x - r) / Math.Max(1, Width - 2 * r), 0f, 1f);
            Value = minimum + (int)Math.Round(pos * (maximum - minimum));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            Focus();
            dragging = true;
            setFromX(e.X);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (dragging)
            {
                setFromX(e.X);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            dragging = false;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            Value += Math.Sign(e.Delta) * Math.Max(1, (maximum - minimum) / 20);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            return keyData is Keys.Left or Keys.Right or Keys.Up or Keys.Down || base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            int large = Math.Max(1, (maximum - minimum) / 10);
            switch (e.KeyCode)
            {
                case Keys.Left: case Keys.Down: Value -= 1; break;
                case Keys.Right: case Keys.Up: Value += 1; break;
                case Keys.PageDown: Value -= large; break;
                case Keys.PageUp: Value += large; break;
                case Keys.Home: Value = minimum; break;
                case Keys.End: Value = maximum; break;
            }
        }

        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }
    }
}

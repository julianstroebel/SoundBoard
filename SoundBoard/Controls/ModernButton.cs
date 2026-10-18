using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace SoundBoard
{
    public enum ButtonVariant
    {
        Default,
        Accent,
        Danger
    }

    /// <summary>
    ///  Flacher Button mit abgerundeten Ecken, Hover-/Pressed-Zuständen sowie optionalem Icon oder Bild.
    /// </summary>
    public class ModernButton : Button
    {
        private ButtonVariant variant = ButtonVariant.Default;
        private Color customColor = Color.Empty;
        private int cornerRadius = 8;
        private string? iconGlyph;
        private bool isActive;
        private bool hover;
        private bool pressed;

        public ModernButton()
        {
            SetStyle(ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Cursor = Cursors.Hand;
        }

        [Category("Appearance"), DefaultValue(ButtonVariant.Default)]
        public ButtonVariant Variant
        {
            get => variant;
            set { variant = value; Invalidate(); }
        }

        /// <summary>
        ///  Eigene Füllfarbe (z.B. pro Soundbutton). Color.Empty = Theme-Farbe.
        /// </summary>
        [Category("Appearance")]
        public Color CustomColor
        {
            get => customColor;
            set { customColor = value; Invalidate(); }
        }

        private bool ShouldSerializeCustomColor() => !customColor.IsEmpty;
        private void ResetCustomColor() => CustomColor = Color.Empty;

        [Category("Appearance"), DefaultValue(8)]
        public int CornerRadius
        {
            get => cornerRadius;
            set { cornerRadius = Math.Max(0, value); Invalidate(); }
        }

        /// <summary>
        ///  Zeichen aus der Windows-Symbolschrift (siehe <see cref="IconFont"/>). Ein Bild hat Vorrang.
        /// </summary>
        [Category("Appearance"), DefaultValue(null)]
        public string? IconGlyph
        {
            get => iconGlyph;
            set { iconGlyph = value; Invalidate(); }
        }

        /// <summary>
        ///  Hebt den Button mit einem Rahmen hervor (z.B. während er abspielt oder ausgewählt ist).
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsActive
        {
            get => isActive;
            set { isActive = value; Invalidate(); }
        }

        protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hover = false; pressed = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { pressed = true; Invalidate(); }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            pressed = false;
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Theme t = ThemeManager.Current;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Color parentBack = Parent?.BackColor ?? t.Back;
            g.Clear(parentBack);

            (Color fill, Color text) = resolveColors(t);
            if (!Enabled)
            {
                fill = blend(fill, parentBack, 0.5f);
                text = t.TextMuted;
            }

            int radius = LogicalToDeviceUnits(cornerRadius);
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = ThemeManager.RoundedRect(rect, radius))
            using (var brush = new SolidBrush(fill))
            {
                g.FillPath(brush, path);

                if (variant == ButtonVariant.Default && customColor.IsEmpty)
                {
                    using var border = new Pen(t.Border);
                    g.DrawPath(border, path);
                }
            }

            if (isActive || (Focused && ShowFocusCues))
            {
                int width = LogicalToDeviceUnits(isActive ? 3 : 2);
                Color ring = variant == ButtonVariant.Default && customColor.IsEmpty ? t.Accent : t.Text;
                Rectangle inner = Rectangle.Inflate(rect, -width / 2 - 1, -width / 2 - 1);
                using GraphicsPath ringPath = ThemeManager.RoundedRect(inner, Math.Max(1, radius - width / 2));
                using var pen = new Pen(ring, width);
                g.DrawPath(pen, ringPath);
            }

            drawContent(g, text);
        }

        private void drawContent(Graphics g, Color textColor)
        {
            int pad = Math.Min(LogicalToDeviceUnits(10), Math.Min(Width, Height) / 5);
            Rectangle content = Rectangle.Inflate(ClientRectangle, -pad, -pad);
            Image? image = Image;
            bool hasIcon = image != null || !string.IsNullOrEmpty(iconGlyph);
            bool hasText = !string.IsNullOrEmpty(Text);
            const TextFormatFlags textFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                | TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis;

            Rectangle iconRect = content;
            Rectangle textRect = content;
            if (hasIcon && hasText)
            {
                int textHeight = TextRenderer.MeasureText(g, Text, Font, new Size(content.Width, int.MaxValue), textFlags).Height;
                textHeight = Math.Min(textHeight, content.Height / 2);
                int gap = pad / 2;
                iconRect = new Rectangle(content.X, content.Y, content.Width, content.Height - textHeight - gap);
                textRect = new Rectangle(content.X, iconRect.Bottom + gap, content.Width, textHeight);
            }

            if (image != null)
            {
                float scale = Math.Min((float)iconRect.Width / image.Width, (float)iconRect.Height / image.Height);
                var size = new SizeF(image.Width * scale, image.Height * scale);
                var dest = new RectangleF(iconRect.X + (iconRect.Width - size.Width) / 2, iconRect.Y + (iconRect.Height - size.Height) / 2, size.Width, size.Height);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, dest);
            }
            else if (hasIcon)
            {
                float size = Math.Max(8f, Math.Min(iconRect.Width, iconRect.Height) * 0.8f);
                using var iconFont = new Font(IconFont.Family, size, GraphicsUnit.Pixel);
                TextRenderer.DrawText(g, iconGlyph, iconFont, iconRect, textColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }

            if (hasText)
            {
                TextRenderer.DrawText(g, Text, Font, textRect, textColor, textFlags);
            }
        }

        private (Color fill, Color text) resolveColors(Theme t)
        {
            if (!customColor.IsEmpty)
            {
                Color c = pressed ? blend(customColor, Color.Black, 0.2f)
                        : hover ? blend(customColor, Color.White, 0.12f)
                        : customColor;
                return (c, customColor.GetBrightness() > 0.6f ? Color.Black : Color.White);
            }

            return variant switch
            {
                ButtonVariant.Accent => (pressed ? t.AccentPressed : hover ? t.AccentHover : t.Accent, t.TextOnAccent),
                ButtonVariant.Danger => (pressed ? t.DangerPressed : hover ? t.DangerHover : t.Danger, t.TextOnAccent),
                _ => (pressed ? t.SurfacePressed : hover ? t.SurfaceHover : t.Surface, t.Text),
            };
        }

        private static Color blend(Color a, Color b, float amount)
        {
            return Color.FromArgb(
                (int)(a.R + (b.R - a.R) * amount),
                (int)(a.G + (b.G - a.G) * amount),
                (int)(a.B + (b.B - a.B) * amount));
        }
    }
}

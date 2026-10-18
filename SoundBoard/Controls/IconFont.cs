using System.Drawing.Text;

namespace SoundBoard
{
    /// <summary>
    ///  Symbol-Schrift von Windows für Button-Icons.
    /// </summary>
    internal static class IconFont
    {
        public static readonly FontFamily Family = resolve();

        public static readonly string[] Glyphs =
        [
            "", // Musik
            "", // Lautsprecher
            "", // Mikrofon
            "", // Glocke
            "", // Stern
            "", // Herz
            "", // Warnung
            "", // Play
            "", // Haken
            "", // Kreuz
            "", // Flagge
            "", // Blitz
            "", // Smiley
            "", // Nachricht
            "", // Personen
            "", // Welt
            "", // Gamepad
            "", // Info
        ];

        public const string Sun = "";
        public const string Moon = "";
        public const string Speech = "";

        /// <summary>
        ///  Zeichnet ein Symbol als Bitmap, z.B. für Menüeinträge.
        /// </summary>
        public static Bitmap Render(string glyph, int size, Color color)
        {
            var bitmap = new Bitmap(size, size);
            using Graphics g = Graphics.FromImage(bitmap);
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            using var font = new Font(Family, size * 0.8f, GraphicsUnit.Pixel);
            using var brush = new SolidBrush(color);
            using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(glyph, font, brush, new RectangleF(0, 0, size, size), format);
            return bitmap;
        }

        /// <summary>
        ///  Gefülltes Stop-Quadrat (das Stop-Symbol der Symbolschrift ist nur ein Umriss).
        /// </summary>
        public static Bitmap RenderStop(int size, Color color)
        {
            var bitmap = new Bitmap(size, size);
            using Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int inset = size / 4;
            var rect = new Rectangle(inset, inset, size - 2 * inset, size - 2 * inset);
            using var path = ThemeManager.RoundedRect(rect, Math.Max(1, size / 10));
            using var brush = new SolidBrush(color);
            g.FillPath(brush, path);
            return bitmap;
        }

        private static FontFamily resolve()
        {
            using var fonts = new InstalledFontCollection();
            foreach (string name in new[] { "Segoe Fluent Icons", "Segoe MDL2 Assets" })
            {
                if (fonts.Families.Any(f => f.Name == name))
                {
                    return new FontFamily(name);
                }
            }
            return FontFamily.GenericSansSerif;
        }
    }
}

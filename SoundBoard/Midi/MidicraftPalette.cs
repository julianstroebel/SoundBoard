namespace SoundBoard
{
    /// <summary>
    ///  Farbpalette der MIDICRAFT-Controller (z.B. PUSH²). Die LED-Farbe wird per Note On auf der Note
    ///  des Bedienelements gesetzt, die Velocity wählt die Farbe (Bedienungsanleitung v3.0.0, Abschnitt 6.5):
    ///  0 = aus, 1–48 = Vollfarben auf dem Farbkreis, 49–96 = Pastellfarben, 97 = Weiß.
    ///  Echte RGB-Werte nimmt das Gerät nicht an, daher wird auf den nächstliegenden Farbton abgebildet.
    /// </summary>
    internal static class MidicraftPalette
    {
        public const int Off = 0;
        public const int White = 97;

        private const int HueSteps = 48;
        private const int FullColorStart = 1;
        private const int PastelStart = 49;

        /// <summary>
        ///  Liefert die Velocity für die Palettenfarbe, die dem RGB-Wert am nächsten kommt.
        /// </summary>
        public static int FromColor(Color color)
        {
            double r = color.R / 255.0, g = color.G / 255.0, b = color.B / 255.0;
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double saturation = max == 0 ? 0 : (max - min) / max;

            // Grau- und Weißtöne haben keinen Farbton
            if (saturation < 0.15)
            {
                return White;
            }

            int step = (int)Math.Round(color.GetHue() / (360.0 / HueSteps)) % HueSteps;
            return (saturation < 0.55 ? PastelStart : FullColorStart) + step;
        }
    }
}

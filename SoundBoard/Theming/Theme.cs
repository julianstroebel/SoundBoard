namespace SoundBoard
{
    /// <summary>
    ///  Farbpalette für die Oberfläche.
    /// </summary>
    internal sealed class Theme
    {
        public required bool IsDark { get; init; }

        public required Color Back { get; init; }
        public required Color EditBack { get; init; }
        public required Color Bar { get; init; }
        public required Color Card { get; init; }
        public required Color Border { get; init; }
        public required Color Input { get; init; }

        public required Color Surface { get; init; }
        public required Color SurfaceHover { get; init; }
        public required Color SurfacePressed { get; init; }

        public required Color Accent { get; init; }
        public required Color AccentHover { get; init; }
        public required Color AccentPressed { get; init; }

        public required Color Danger { get; init; }
        public required Color DangerHover { get; init; }
        public required Color DangerPressed { get; init; }

        public required Color Text { get; init; }
        public required Color TextMuted { get; init; }
        public required Color TextOnAccent { get; init; }

        public static readonly Theme Light = new()
        {
            IsDark = false,
            Back = ColorTranslator.FromHtml("#F3F4F7"),
            EditBack = ColorTranslator.FromHtml("#FCE4E6"),
            Bar = ColorTranslator.FromHtml("#FFFFFF"),
            Card = ColorTranslator.FromHtml("#FFFFFF"),
            Border = ColorTranslator.FromHtml("#E1E4EA"),
            Input = ColorTranslator.FromHtml("#F8F9FB"),
            Surface = ColorTranslator.FromHtml("#EEF0F4"),
            SurfaceHover = ColorTranslator.FromHtml("#E2E6EC"),
            SurfacePressed = ColorTranslator.FromHtml("#D5DAE2"),
            Accent = ColorTranslator.FromHtml("#3B6CF6"),
            AccentHover = ColorTranslator.FromHtml("#2F5DE0"),
            AccentPressed = ColorTranslator.FromHtml("#2550C8"),
            Danger = ColorTranslator.FromHtml("#E5484D"),
            DangerHover = ColorTranslator.FromHtml("#D63C41"),
            DangerPressed = ColorTranslator.FromHtml("#C2343A"),
            Text = ColorTranslator.FromHtml("#1B1E24"),
            TextMuted = ColorTranslator.FromHtml("#6B7280"),
            TextOnAccent = Color.White,
        };

        public static readonly Theme Dark = new()
        {
            IsDark = true,
            Back = ColorTranslator.FromHtml("#16181D"),
            EditBack = ColorTranslator.FromHtml("#33191F"),
            Bar = ColorTranslator.FromHtml("#1B1D23"),
            Card = ColorTranslator.FromHtml("#1F2229"),
            Border = ColorTranslator.FromHtml("#2E323B"),
            Input = ColorTranslator.FromHtml("#16181D"),
            Surface = ColorTranslator.FromHtml("#2A2E37"),
            SurfaceHover = ColorTranslator.FromHtml("#343944"),
            SurfacePressed = ColorTranslator.FromHtml("#3E4450"),
            Accent = ColorTranslator.FromHtml("#4F7DF9"),
            AccentHover = ColorTranslator.FromHtml("#638DFF"),
            AccentPressed = ColorTranslator.FromHtml("#416CE0"),
            Danger = ColorTranslator.FromHtml("#E5484D"),
            DangerHover = ColorTranslator.FromHtml("#F05A5F"),
            DangerPressed = ColorTranslator.FromHtml("#C9393E"),
            Text = ColorTranslator.FromHtml("#E8EAEE"),
            TextMuted = ColorTranslator.FromHtml("#9AA1AD"),
            TextOnAccent = Color.White,
        };
    }
}

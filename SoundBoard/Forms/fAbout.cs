namespace SoundBoard
{
    public partial class fAbout : Form
    {
        /// <summary>
        ///  Verwendete Bibliotheken und externe Tools (Name, Beschreibung, Link).
        /// </summary>
        private static readonly (string name, string description, string url)[] libraries =
        [
            (".NET 10 und Windows Forms", "Laufzeitumgebung und Oberfläche", "https://github.com/dotnet/winforms"),
            ("NAudio", "Audio-Wiedergabe und MIDI-Eingang", "https://github.com/naudio/NAudio"),
            ("Windows-Sprachausgabe", "Integrierte Offline-Stimmen (Windows.Media.SpeechSynthesis)", "https://learn.microsoft.com/uwp/api/windows.media.speechsynthesis"),
            ("Piper", "Neuronale Offline-Sprachausgabe (optional)", "https://github.com/rhasspy/piper"),
            ("Piper-Stimme „Thorsten“", "Deutsche Stimme für Piper", "https://huggingface.co/rhasspy/piper-voices/tree/v1.0.0/de/de_DE/thorsten"),
            ("Segoe Fluent Icons", "Symbole für Buttons und Menü", "https://learn.microsoft.com/windows/apps/design/style/segoe-fluent-icons-font"),
        ];

        public fAbout()
        {
            InitializeComponent();

            lbl_version.Text = string.IsNullOrEmpty(AppInfo.ReleaseDate)
                ? $"Version {AppInfo.Version}"
                : $"Version {AppInfo.Version} · {AppInfo.ReleaseDate}";
            lbl_company.Text = AppInfo.Company;

            // Nur die Adresse ist Link, nicht der Vorspann "GitHub: "
            lnk_github.LinkArea = new LinkArea("GitHub: ".Length, lnk_github.Text.Length - "GitHub: ".Length);

            buildLibraries();
        }

        private void buildLibraries()
        {
            int y = 32;
            foreach ((string name, string description, string url) in libraries)
            {
                var link = new LinkLabel
                {
                    AutoSize = true,
                    Location = new Point(16, y),
                    Text = name,
                    Tag = url
                };
                link.LinkClicked += library_LinkClicked;

                var info = new MutedLabel
                {
                    AutoEllipsis = true,
                    Location = new Point(16, y + 20),
                    Size = new Size(464, 16),
                    Text = description
                };

                cardLibraries.Controls.Add(link);
                cardLibraries.Controls.Add(info);
                y += 47;
            }
        }

        private void library_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            if (sender is LinkLabel { Tag: string url })
            {
                AppInfo.OpenUrl(url);
            }
        }

        private void lnk_github_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            AppInfo.OpenUrl(AppInfo.GitHubUrl);
        }
    }
}

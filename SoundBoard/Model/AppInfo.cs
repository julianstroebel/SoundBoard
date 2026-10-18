using System.Diagnostics;
using System.Reflection;

namespace SoundBoard
{
    /// <summary>
    ///  Programminformationen. Version und Datum kommen aus der SoundBoard.csproj.
    /// </summary>
    internal static class AppInfo
    {
        public const string Company = "Ströbel Eventtechnik";
        public const string GitHubUrl = "https://github.com/julianstroebel/SoundBoard";

        private static readonly Assembly assembly = typeof(AppInfo).Assembly;

        /// <summary>
        ///  Version als "Major.Minor", z.B. "1.1".
        /// </summary>
        public static string Version
        {
            get
            {
                Version version = assembly.GetName().Version ?? new Version(1, 0);
                return $"{version.Major}.{version.Minor}";
            }
        }

        public static string ReleaseDate =>
            assembly.GetCustomAttributes<AssemblyMetadataAttribute>().FirstOrDefault(a => a.Key == "ReleaseDate")?.Value ?? "";

        /// <summary>
        ///  Öffnet eine Webseite im Standardbrowser.
        /// </summary>
        public static void OpenUrl(string url)
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}

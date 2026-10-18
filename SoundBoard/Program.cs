namespace SoundBoard
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Benutzereinstellungen werden pro Programmversion gespeichert;
            // nach einem Update einmalig die Werte der Vorversion übernehmen
            if (Properties.Settings.Default.upgraderequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.upgraderequired = false;
                Properties.Settings.Default.Save();
            }

            Application.Run(new fMain());
        }
    }
}
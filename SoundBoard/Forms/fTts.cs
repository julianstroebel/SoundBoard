namespace SoundBoard
{
    /// <summary>
    ///  Kleines Fenster zum Sprechen, Speichern und Zuweisen von Texten.
    /// </summary>
    public partial class fTts : Form
    {
        private readonly AppConfig config;
        private readonly Action saveConfig;
        private readonly Action<string> speak;
        private readonly Action stop;
        private readonly Action<int, string, string> assignText;

        /// <param name="assignText">Legt einen Text auf einen Button (Button-Nr., Text, Vorschlag für die Beschriftung).</param>
        internal fTts(AppConfig config, Action saveConfig, Action<string> speak, Action stop, Action<int, string, string> assignText)
        {
            InitializeComponent();

            this.config = config;
            this.saveConfig = saveConfig;
            this.speak = speak;
            this.stop = stop;
            this.assignText = assignText;

            refreshTexts(null);
        }

        private void btn_play_Click(object? sender, EventArgs e)
        {
            speak(txt_tts.Text);
        }

        private void btn_stop_Click(object? sender, EventArgs e)
        {
            stop();
        }

        private void refreshTexts(SavedText? select)
        {
            cmb_texts.Items.Clear();
            cmb_texts.Items.Add("Gespeicherte Texte …");
            foreach (SavedText text in config.Texts.OrderBy(t => t.Name, StringComparer.CurrentCultureIgnoreCase))
            {
                cmb_texts.Items.Add(text);
            }

            if (select != null)
            {
                cmb_texts.SelectedItem = select;
            }
            else
            {
                cmb_texts.SelectedIndex = 0;
            }
        }

        private void cmb_texts_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmb_texts.SelectedItem is SavedText text)
            {
                txt_tts.Text = text.Text;
            }
        }

        private void btn_save_Click(object? sender, EventArgs e)
        {
            string text = txt_tts.Text.Trim();
            if (text.Length == 0)
            {
                txt_tts.Focus();
                return;
            }

            string suggestion = (cmb_texts.SelectedItem as SavedText)?.Name ?? ShortName(text);
            string? name = fInput.Ask(this, "Text speichern", "Name für den gespeicherten Text:", suggestion);
            if (name == null)
            {
                return;
            }

            SavedText? saved = config.Texts.FirstOrDefault(t => string.Equals(t.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (saved != null)
            {
                if (saved.Text != text && confirm($"„{saved.Name}“ existiert bereits. Überschreiben?") != DialogResult.Yes)
                {
                    return;
                }
                saved.Text = text;
            }
            else
            {
                saved = new SavedText { Name = name, Text = text };
                config.Texts.Add(saved);
            }

            saveConfig();
            refreshTexts(saved);
        }

        private void btn_delete_Click(object? sender, EventArgs e)
        {
            if (cmb_texts.SelectedItem is not SavedText saved)
            {
                txt_tts.Clear();
                return;
            }

            if (confirm($"Gespeicherten Text „{saved.Name}“ löschen?") != DialogResult.Yes)
            {
                return;
            }

            config.Texts.Remove(saved);
            saveConfig();
            refreshTexts(null);
            txt_tts.Clear();
        }

        private void btn_assign_Click(object? sender, EventArgs e)
        {
            string text = txt_tts.Text.Trim();
            if (text.Length == 0)
            {
                txt_tts.Focus();
                return;
            }

            string title = cmb_texts.SelectedItem is SavedText saved && saved.Text == text ? saved.Name : ShortName(text);
            var menu = new ContextMenuStrip();
            for (int id = 1; id <= AppConfig.ButtonCount; id++)
            {
                int buttonId = id;
                string? current = config.Buttons[id - 1].Title;
                string label = string.IsNullOrEmpty(current) || current == ButtonConfig.DefaultTitle(id) ? $"Button {id}" : $"Button {id}  ·  {current}";
                menu.Items.Add(label, null, (_, _) => assignText(buttonId, text, title));
            }
            ThemeManager.Apply(menu);
            menu.Closed += (_, _) => BeginInvoke(menu.Dispose);
            menu.Show(btn_assign, new Point(0, btn_assign.Height));
        }

        internal static string ShortName(string text)
        {
            string line = text.ReplaceLineEndings(" ").Trim();
            return line.Length <= 30 ? line : line[..30].TrimEnd() + "…";
        }

        private DialogResult confirm(string message)
        {
            return MessageBox.Show(this, message, "SoundBoard", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}

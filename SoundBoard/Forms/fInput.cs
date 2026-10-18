namespace SoundBoard
{
    /// <summary>
    ///  Einfacher Eingabedialog für einen Namen o.ä.
    /// </summary>
    public partial class fInput : Form
    {
        public fInput()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  Zeigt den Dialog an. Liefert den eingegebenen Text oder null bei Abbruch / leerer Eingabe.
        /// </summary>
        public static string? Ask(IWin32Window owner, string title, string prompt, string value)
        {
            using var form = new fInput();
            form.Text = title;
            form.lbl_prompt.Text = prompt;
            form.txt_value.Text = value;
            form.txt_value.SelectAll();
            ThemeManager.Apply(form);

            if (form.ShowDialog(owner) != DialogResult.OK || string.IsNullOrWhiteSpace(form.txt_value.Text))
            {
                return null;
            }
            return form.txt_value.Text.Trim();
        }
    }
}

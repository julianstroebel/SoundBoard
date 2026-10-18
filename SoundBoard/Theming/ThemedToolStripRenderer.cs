namespace SoundBoard
{
    /// <summary>
    ///  Flacher Renderer für MenuStrip / StatusStrip passend zum Theme.
    /// </summary>
    internal sealed class ThemedToolStripRenderer : ToolStripProfessionalRenderer
    {
        private readonly Theme theme;

        public ThemedToolStripRenderer(Theme theme) : base(new ThemedColorTable(theme))
        {
            this.theme = theme;
            RoundedEdges = false;
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is ToolStripDropDown)
            {
                using var pen = new Pen(theme.Border);
                e.Graphics.DrawRectangle(pen, 0, 0, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1);
            }
            else if (e.ToolStrip is StatusStrip)
            {
                using var pen = new Pen(theme.Border);
                e.Graphics.DrawLine(pen, 0, 0, e.AffectedBounds.Width, 0);
            }
            else
            {
                using var pen = new Pen(theme.Border);
                e.Graphics.DrawLine(pen, 0, e.AffectedBounds.Height - 1, e.AffectedBounds.Width, e.AffectedBounds.Height - 1);
            }
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected && !e.Item.Pressed)
            {
                return;
            }

            var rect = new Rectangle(Point.Empty, e.Item.Size);
            rect.Inflate(-2, -1);
            Color color = e.Item.Pressed && e.Item.Owner is MenuStrip ? theme.SurfacePressed : theme.SurfaceHover;
            using var brush = new SolidBrush(color);
            using var path = ThemeManager.RoundedRect(rect, 4);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillPath(brush, path);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = theme.TextMuted;
            base.OnRenderArrow(e);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (!e.Item.Enabled)
            {
                e.TextColor = theme.TextMuted;
            }
            base.OnRenderItemText(e);
        }

        private sealed class ThemedColorTable(Theme t) : ProfessionalColorTable
        {
            public override Color MenuStripGradientBegin => t.Bar;
            public override Color MenuStripGradientEnd => t.Bar;
            public override Color StatusStripGradientBegin => t.Bar;
            public override Color StatusStripGradientEnd => t.Bar;
            public override Color ToolStripDropDownBackground => t.Bar;
            public override Color ToolStripBorder => t.Border;
            public override Color MenuBorder => t.Border;
            public override Color MenuItemBorder => Color.Transparent;
            public override Color MenuItemSelected => t.SurfaceHover;
            public override Color ImageMarginGradientBegin => t.Bar;
            public override Color ImageMarginGradientMiddle => t.Bar;
            public override Color ImageMarginGradientEnd => t.Bar;
            public override Color SeparatorDark => t.Border;
            public override Color SeparatorLight => t.Border;
            public override Color CheckBackground => t.Accent;
            public override Color CheckSelectedBackground => t.AccentHover;
            public override Color CheckPressedBackground => t.AccentPressed;
        }
    }
}

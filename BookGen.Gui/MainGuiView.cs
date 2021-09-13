using BookGen.Gui.Views;

namespace BookGen.Gui
{
    internal class MainGuiView : ViewBase<IMainViewController>
    {
        public MainGuiView(IMainViewController controller, IConsoleUi ui) 
            : base(controller, ui)
        {
        }

        public override void DrawView()
        {
            AddText(new TextElement
            {
                Left = 2,
                Top = 0,
                Text = "  ____              _     ____            \r\n"
                     + " | __ )  ___   ___ | | __/ ___| ___ _ __  \r\n"
                     + " |  _ \\ / _ \\ / _ \\| |/ / |  _ / _ \\ '_ \\ \r\n"
                     + " | |_) | (_) | (_) |   <| |_| |  __/ | | |\r\n"
                     + " |____/ \\___/ \\___/|_| \\_\\____|\\___|_| |_|\r\n"
            });
            AddText(new TextElement
            {
                Left = 2,
                Top = 6,
                Text = $"Work directory: {Controller.WorkDir}"
            });
            AddText(new TextElement
            {
                Left = 2,
                Top = 8,
                Text = "Build:"
            });
            AddButton(new ButtonElement
            {
                Left = 4,
                Top = 9,
                Text = "Validate config",
                ClickSuspendsUi = true,
                OnClick = Controller.ValidateConfig
            });
            AddButton(new ButtonElement
            {
                Left = 4,
                Top = 10,
                Text = "Clean output directory",
                ClickSuspendsUi = true,
                OnClick = Controller.CleanOutDir
            });
            AddButton(new ButtonElement
            {
                Left = 4,
                Top = 11,
                Text = "Build test website",
                ClickSuspendsUi = true,
                OnClick = Controller.BuildTest
            });
            AddButton(new ButtonElement
            {
                Left = 4,
                Top = 12,
                Text = "Build release website",
                OnClick = Controller.BuildRelease
            });
            AddButton(new ButtonElement
            {
                Left = 4,
                Top = 13,
                Text = "Build printable html",
                ClickSuspendsUi = true,
                OnClick = Controller.BuildPrint
            });
            AddButton(new ButtonElement
            {
                Left = 4,
                Top = 14,
                Text = "Build E-pub",
                ClickSuspendsUi = true,
                OnClick = Controller.BuildEpub
            });
            AddButton(new ButtonElement
            {
                Left = 4,
                Top = 15,
                Text = "Build E-pub",
                ClickSuspendsUi = true,
                OnClick = Controller.BuildWordpress
            });
            AddText(new TextElement
            {
                Left = 2,
                Top = 18,
                Text = "General:"
            });
            AddButton(new ButtonElement
            {
                Left = 4,
                Top = 20,
                Text = "Help",
            });
            AddButton(new ButtonElement
            {
                Left = 4,
                Top = 21,
                ClickSuspendsUi = true,
                Text = "Exit program",
                OnClick = () => Ui?.ExitApp(),
            });
        }
    }
}

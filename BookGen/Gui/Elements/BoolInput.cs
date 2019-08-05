using BookGen.Gui.Renderering;

namespace BookGen.Gui.Elements
{
    internal class BoolInput : ConsoleUiElement, IHaveContent
    {
        public string Content { get; set; }

        public bool Value { get; set; }

        public override void Render(ITerminalRenderer target)
        {
            target.Text(Content, Foreground, Background, TextFormat.Default);
            target.Text(" [Y/N]: ", Foreground, Background, TextFormat.BoldBright);
            char input = target.ReadChar();
            Value = input == 'Y' || input == 'y';
        }
    }
}

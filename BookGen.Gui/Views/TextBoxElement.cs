namespace BookGen.Gui.Views
{
    internal sealed record TextBoxElement : TextElement
    {
        public bool IsReadonly { get; init; }
        public float Width { get; init; }
    }
}

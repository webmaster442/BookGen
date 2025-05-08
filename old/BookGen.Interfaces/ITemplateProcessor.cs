namespace BookGen.Interfaces
{
    public interface ITemplateProcessor : IContent
    {
        string TemplateContent { get; set; }
        string Render();
    }
}

namespace BookGen.FormulaEdit.AppLogic;

internal sealed class DocumentState
{
    public string CurrentFileName { get; set; }
    public bool IsDirty { get; private set; }

    public bool HasFileName => !string.IsNullOrEmpty(CurrentFileName);

    public DocumentState()
    {
        IsDirty = false;
        CurrentFileName = string.Empty;
    }

    public void NewCreated()
    {
        IsDirty = false;
        CurrentFileName = string.Empty;
    }

    public void Opened(string fileName)
    {
        IsDirty = false;
        CurrentFileName = fileName;
    }

    public void Saved()
    {
        IsDirty = false;
    }

    public void SavedAs(string fileName)
    {
        IsDirty = false;
        CurrentFileName = fileName;
    }

    public void Modifified()
    {
        IsDirty = true;
    }
}

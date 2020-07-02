namespace BookGen.Ui.ArgumentParser
{
    public abstract class ArgumentsBase
    {
        public string[] Files { get; internal set; }

        public ArgumentsBase()
        {
            Files = new string[0];
        }
    }
}

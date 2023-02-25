namespace BookGen.Cli
{
    public interface IResolver
    {
        public object Resolve(Type type);
    }
}

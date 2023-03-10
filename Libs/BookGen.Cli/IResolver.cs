namespace BookGen.Cli
{
    public interface IResolver
    {
        bool CanResolve(Type type);
        public object Resolve(Type type);
    }
}

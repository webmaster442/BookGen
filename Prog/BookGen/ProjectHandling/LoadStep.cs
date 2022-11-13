namespace BookGen.ProjectHandling
{
    internal abstract class LoadStep
    {
        protected LoadStep(LoadState state, ILog log)
        {
            State = state;
            Log = log;
        }

        public LoadState State { get; }

        public ILog Log { get; }

        public virtual bool CanExecute() => true;

        public abstract bool Execute();
    }
}

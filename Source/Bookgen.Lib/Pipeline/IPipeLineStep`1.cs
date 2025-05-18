namespace Bookgen.Lib.Pipeline;

public interface IPipeLineStep<TState> : IPipeLineStep
{
    TState State { get; }
}
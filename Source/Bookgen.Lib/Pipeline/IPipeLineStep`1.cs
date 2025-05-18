namespace Bookgen.Lib.Pipeline;

public interface IPipeLineStep<out TState> : IPipeLineStep
{
    TState State { get; }
}
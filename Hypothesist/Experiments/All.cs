namespace Hypothesist.Experiments;

internal sealed class All<T>(Predicate<T> match) : IExperiment<T>
{
    private readonly List<T> _matched = [];

    void IObserver<T>.OnCompleted()
    {
    }

    void IObserver<T>.OnNext(T value)
    {
        if (!match(value))
        {
            throw new HypothesisInvalidException<T>("I expected all samples to match, but one did not.", _matched,  new[] { value });
        }

        _matched.Add(value);
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    void IObserver<T>.OnError(Exception error) => throw new NotImplementedException();

    bool IExperiment<T>.Done => false;
}
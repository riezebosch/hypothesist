namespace Hypothesist.Experiments;

public class First<T>(Predicate<T> match) : IExperiment<T>
{
    void IObserver<T>.OnCompleted() => 
        throw new HypothesisInvalidException<T>("Expected first sample to match but none received", Array.Empty<T>(), Array.Empty<T>());

    void IObserver<T>.OnNext(T value)
    {
        if (!match(value))
        {
            throw new HypothesisInvalidException<T>("I expected the first sample to match, but it did not.", Array.Empty<T>(),  new[] { value });
        }

        Done = true;
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    void IObserver<T>.OnError(Exception error) => throw new NotImplementedException();

    public bool Done { get; private set; }
}
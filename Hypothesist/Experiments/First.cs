using System;
using System.Linq;

namespace Hypothesist.Experiments;

public class First<T> : IExperiment<T>
{
    private readonly Predicate<T> _match;

    public First(Predicate<T> match) => 
        _match = match;

    void IObserver<T>.OnCompleted() => 
        throw new HypothesisInvalidException<T>("Expected first sample to match but none received", Enumerable.Empty<T>(), Enumerable.Empty<T>());

    void IObserver<T>.OnNext(T value)
    {
        if (!_match(value))
        {
            throw new HypothesisInvalidException<T>("I expected the first sample to match, but it did not.", Enumerable.Empty<T>(),  new[] { value });
        }

        Done = true;
    }

    #if NETSTANDARD2_0
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    void IObserver<T>.OnError(Exception error) => throw new NotImplementedException();
    #endif

    public bool Done { get; private set; }
}
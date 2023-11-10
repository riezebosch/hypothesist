using System;
using System.Collections.Generic;

namespace Hypothesist.Experiments;

internal sealed class All<T> : IExperiment<T>
{
    private readonly Predicate<T> _match;
    private readonly List<T> _matched = new();

    public All(Predicate<T> match) => 
        _match = match;

    void IObserver<T>.OnCompleted()
    {
    }

    void IObserver<T>.OnNext(T value)
    {
        if (!_match(value))
        {
            throw new HypothesisInvalidException<T>("I expected all samples to match, but one did not.", _matched,  new[] { value });
        }

        _matched.Add(value);
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    void IObserver<T>.OnError(Exception error) => throw new NotImplementedException();

    bool IExperiment<T>.Done => false;
}
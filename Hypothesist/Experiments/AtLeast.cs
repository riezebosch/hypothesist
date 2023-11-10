using System;
using System.Collections.Generic;

namespace Hypothesist.Experiments;

internal class AtLeast<T> : IExperiment<T>
{
    private readonly Predicate<T> _match;
    private readonly int _occurrences;
    private readonly List<T> _matched = new();
    private readonly List<T> _unmatched = new();


    public AtLeast(Predicate<T> match, int occurrences) => 
        (_match, _occurrences) = (match, occurrences);

    void IObserver<T>.OnCompleted()
    {
        if (!Done)
        {
            throw new HypothesisInvalidException<T>($"I expected at least {_occurrences} matches but found only {_matched.Count}.", _matched, _unmatched);
        }
    }
    
    void IObserver<T>.OnNext(T value)
    {
        (_match(value) ? _matched : _unmatched).Add(value);
        Done = _matched.Count >= _occurrences;
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    void IObserver<T>.OnError(Exception error) => throw new NotImplementedException();

    public bool Done { get; private set; }
}
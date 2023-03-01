using System;
using System.Collections.Generic;

namespace Hypothesist.Experiments;

public class Exactly<T> : IExperiment<T>
{
    private readonly Predicate<T> _match;
    private readonly int _occurrences;

    private readonly List<T> _matched = new();
    private readonly List<T> _unmatched = new();


    public Exactly(Predicate<T> match, int occurrences) => 
        (_match, _occurrences) = (match, occurrences);

    void IObserver<T>.OnCompleted()
    {
        if (_matched.Count != _occurrences)
        {
            throw new InvalidException<T>($"I Expected exactly {_occurrences} matches but found {_matched.Count}.", _matched, _unmatched);
        }
    }

    void IObserver<T>.OnNext(T value) => 
        (_match(value) ? _matched : _unmatched).Add(value);

    #if NETSTANDARD2_0
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    void IObserver<T>.OnError(Exception error) => throw new NotImplementedException();
    #endif

    bool IExperiment<T>.Done => false;
}
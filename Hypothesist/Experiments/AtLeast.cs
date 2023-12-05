namespace Hypothesist.Experiments;

internal class AtLeast<T>(Predicate<T> match, int occurrences) : IExperiment<T>
{
    private readonly List<T> _matched = [];
    private readonly List<T> _unmatched = [];
    
    void IObserver<T>.OnCompleted()
    {
        if (!Done)
        {
            throw new HypothesisInvalidException<T>($"I expected at least {occurrences} matches but found only {_matched.Count}.", _matched, _unmatched);
        }
    }
    
    void IObserver<T>.OnNext(T value)
    {
        (match(value) ? _matched : _unmatched).Add(value);
        Done = _matched.Count >= occurrences;
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    void IObserver<T>.OnError(Exception error) => throw new NotImplementedException();

    public bool Done { get; private set; }
}
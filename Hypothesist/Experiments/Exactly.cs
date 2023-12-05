namespace Hypothesist.Experiments;

public class Exactly<T>(Predicate<T> match, int occurrences) : IExperiment<T>
{
    private readonly List<T> _matched = [];
    private readonly List<T> _unmatched = [];
    
    void IObserver<T>.OnCompleted()
    {
        if (_matched.Count != occurrences)
        {
            throw new HypothesisInvalidException<T>($"I Expected exactly {occurrences} matches but found {_matched.Count}.", _matched, _unmatched);
        }
    }

    void IObserver<T>.OnNext(T value) => 
        (match(value) ? _matched : _unmatched).Add(value);

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    void IObserver<T>.OnError(Exception error) => throw new NotImplementedException();

    bool IExperiment<T>.Done => false;
}
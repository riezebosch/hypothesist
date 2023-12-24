namespace Hypothesist.Experiments;

public class AtMost<T>(Predicate<T> match, int occurrences) : IExperiment<T>
{
    private readonly List<T> _matched = new();
    private readonly List<T> _unmatched = new();

    void IObserver<T>.OnCompleted()
    {
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    void IObserver<T>.OnError(Exception error) => throw new NotImplementedException();

    void IObserver<T>.OnNext(T value)
    {
        (match(value) ? _matched : _unmatched).Add(value);
        if (_matched.Count > occurrences)
        {
            throw new HypothesisInvalidException<T>(
                $"I expected at most {occurrences} samples to match, but the received yet another one.", _matched,
                _unmatched);
        }
    }

    bool IExperiment<T>.Done => false;
}
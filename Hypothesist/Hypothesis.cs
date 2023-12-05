using Hypothesist.Experiments;

namespace Hypothesist;

public static class Hypothesis
{
    public static IHypothesis<T> For<T>() =>
        new Hypothesis<T>();
    public static IHypothesis<T> Any<T>(this IHypothesis<T> hypothesis, Predicate<T> match) =>
        hypothesis.Add(new AtLeast<T>(match, 1));
    public static IHypothesis<T> All<T>(this IHypothesis<T> hypothesis, Predicate<T> match) =>
        hypothesis.Add(new All<T>(match));
    public static IHypothesis<T> First<T>(this IHypothesis<T> hypothesis, Predicate<T> match) =>
        hypothesis.Add(new First<T>(match));
    public static IHypothesis<T> Single<T>(this IHypothesis<T> hypothesis, Predicate<T> match) =>
        hypothesis.Add(new Exactly<T>(match, 1));
    public static IHypothesis<T> Exactly<T>(this IHypothesis<T> hypothesis, int occurrences, Predicate<T> match) =>
        hypothesis.Add(new Exactly<T>(match, occurrences));
    public static IHypothesis<T> AtLeast<T>(this IHypothesis<T> hypothesis, int occurrences, Predicate<T> match) =>
        hypothesis.Add(new AtLeast<T>(match, occurrences));
    public static IHypothesis<T> Any<T>(this IHypothesis<T> hypothesis) =>
        hypothesis.Any(_ => true);
}

internal class Hypothesis<T> : IHypothesis<T>
{
    private readonly Channel<T> _channel = Channel.CreateUnbounded<T>();
    private readonly List<IExperiment<T>> _experiments = new();

    async Task IHypothesis<T>.Validate(TimeSpan period, CancellationToken token)
    {
        try
        {
            await foreach (var sample in _channel.Reader.ReadAllAsync(token).Sliding(period, token))
            {
                _experiments.ForEach(x => x.OnNext(sample));
                if (_experiments.All(x => x.Done))
                {
                    break;
                }
            }
        }
        catch (OperationCanceledException)
        {
            _experiments.ForEach(x => x.OnCompleted());
        }
    }

    async Task IHypothesis<T>.Test(T sample, CancellationToken token) =>
        await _channel.Writer.WriteAsync(sample, token);

    public IHypothesis<T> Add(IExperiment<T> observer)
    {
        _experiments.Add(observer);
        return this;
    }
}
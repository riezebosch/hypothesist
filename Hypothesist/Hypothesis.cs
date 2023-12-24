using Hypothesist.Builders;
using Hypothesist.Experiments;

namespace Hypothesist;

public static class Hypothesis
{
    public static On<T> On<T>(IAsyncEnumerable<T> observer) => new(observer);

    /// <summary>
    /// Use <see cref="Observer{T}"/> and <see cref="On{T}"/> instead.
    /// </summary>
    [Obsolete("Use Observer.For<T>() and Hypothesis.On(observer).")]
    public static Old.For<T> For<T>() => new();
}

public class Hypothesis<T>(IAsyncEnumerable<T> observer, IExperiment<T> experiment)
{
    public async Task Validate(CancellationToken token = default)
    {
        await foreach (var sample in observer.WithCancellation(token))
        {
            experiment.OnNext(sample);
            if (experiment.Done)
            {
                break;
            }
        }

        experiment.OnCompleted();
    }
}
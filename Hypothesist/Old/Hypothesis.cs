using Hypothesist.Experiments;

namespace Hypothesist.Old;

[Obsolete]
public class Hypothesis<T>(IExperiment<T> experiment) : Observer<T>
{
    public Task Test(T data) => Add(data);
    public Task Validate(TimeSpan seconds, CancellationToken token = default) =>
        new Hypothesist.Hypothesis<T>(this.WithTimeout(seconds, seconds, token).UntilCancelled(token), experiment).Validate(token);
}
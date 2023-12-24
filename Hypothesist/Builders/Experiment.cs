using Hypothesist.Experiments;

namespace Hypothesist.Builders;

public class Experiment<T>(IAsyncEnumerable<T> observer, Func<Predicate<T>, IExperiment<T>> experiment)
{
    public Hypothesis<T> Match(Predicate<T> match) => 
        new(observer, experiment(match));

    public Hypothesis<T> Match(T match) => 
        Match(t => Equals(t, match));
    
    public Hypothesis<T> Match() => 
        Match(_ => true);
}
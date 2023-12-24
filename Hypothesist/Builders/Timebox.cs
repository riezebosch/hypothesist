using Hypothesist.Experiments;

namespace Hypothesist.Builders;

public class Timebox<T>(IAsyncEnumerable<T> observer)
{
    public Experiment<T> Any() =>
        new(observer, match => new AtLeast<T>(match, 1));
    public Experiment<T> All() =>
        new(observer, match => new All<T>(match));
    public Experiment<T> First() =>
        new(observer, match => new First<T>(match));
    public Experiment<T> Single() =>
        new(observer, match => new Exactly<T>(match, 1));
    public Experiment<T> Exactly(int occurrences) =>
        new(observer, match => new Exactly<T>(match, occurrences));
    public Experiment<T> AtLeast(int occurrences) =>
        new(observer, match => new AtLeast<T>(match, occurrences));
    public Experiment<T> AtMost(int occurrences) => 
        new(observer, match => new AtMost<T>(match, occurrences));
}
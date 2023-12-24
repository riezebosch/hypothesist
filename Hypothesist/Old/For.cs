using Hypothesist.Experiments;

namespace Hypothesist.Old;

[Obsolete]
public class For<T>
{
    public Hypothesis<T> Any(Predicate<T> match) =>
        new(new AtLeast<T>(match, 1));
    public Hypothesis<T> All(Predicate<T> match) =>
        new(new All<T>(match));
    public Hypothesis<T> First(Predicate<T> match) =>
        new(new First<T>(match));
    public Hypothesis<T> Single(Predicate<T> match) =>
        new(new Exactly<T>(match, 1));
    public Hypothesis<T> Exactly(int occurrences, Predicate<T> match) =>
        new(new Exactly<T>(match, occurrences));
    public Hypothesis<T> AtLeast(int occurrences, Predicate<T> match) =>
        new(new AtLeast<T>(match, occurrences));
    public Hypothesis<T> Any() =>
        Any(_ => true);
}
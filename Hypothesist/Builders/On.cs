namespace Hypothesist.Builders;

public class On<T>(IAsyncEnumerable<T> observer)
{
    public Timebox<T> Timebox(TimeSpan duration) => new(observer.Timebox(duration));
}
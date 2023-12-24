using System.Diagnostics;

namespace Hypothesist.Tests;

public class Timebox
{
    [Fact]
    public async Task Within()
    {
        var observer = Observer.For<string>();
        var hypothesis = Hypothesis
            .On(observer)
            .Timebox(2.Seconds())
            .All()
            .Match("a");

        var sw = Stopwatch.StartNew();
        _ = observer.AddSlowly("a", "a", "a", "a", "b");
        await hypothesis.Validate();

        sw.Elapsed
            .Should().BeGreaterThan(2.Seconds())
            .And.BeLessThan(5.Seconds());
    }
    
    [Fact]
    public async Task Cancel()
    {
        var sw = Stopwatch.StartNew();
        using var tcs = new CancellationTokenSource(2.Seconds());
        var hypothesis = Hypothesis
            .On(Observer.For<string>())
            .Timebox(1.Minutes())
            .All()
            .Match();

        await hypothesis.Validate(tcs.Token);
        sw.Elapsed.Should().BeLessThan(5.Seconds());
    }
}
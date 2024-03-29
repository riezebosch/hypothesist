using System.Diagnostics;

namespace Hypothesist.Tests.Experiments;

public static class AtLeast
{
    [Fact]
    public static async Task Valid()
    {
        var observer = Observer.For<string>();

        await observer.Add("a");
        await observer.Add("a");
            
        var sw = Stopwatch.StartNew();
        await Hypothesis
            .On(observer)
            .Timebox(20.Minutes())
            .AtLeast(2)
            .Match(x => x == "a")
            .Validate();
        
        sw.Elapsed.Should().BeLessThan(1.Seconds());
    }
        
    [Fact]
    public static async Task None()
    {
        var observer = Observer.For<string>();
        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .AtLeast(2)
            .Match("a")
            .Validate();
        
        await act.Should()
            .ThrowAsync<HypothesisInvalidException<string>>()
            .WithMessage("*at least 2*");
    }
        
    [Fact]
    public static async Task Less()
    {
        var observer = Observer.For<string>();
        await observer.Add("a");
            
        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .AtLeast(2)
            .Match("a")
            .Validate();
        
        var ex = await act.Should()
            .ThrowAsync<HypothesisInvalidException<string>>();

        ex.Which
            .Matched
            .Should()
            .BeEquivalentTo("a");
    }
        
    [Fact]
    public static async Task Unmatched()
    {
        var observer = Observer.For<string>();
        await observer.Add("b");
            
        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .AtLeast(2)
            .Match("a")
            .Validate();
        
        var ex = await act.Should()
            .ThrowAsync<HypothesisInvalidException<string>>();

        ex.Which
            .Unmatched
            .Should()
            .BeEquivalentTo("b");
    }
}
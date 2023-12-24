namespace Hypothesist.Tests.Experiments;

public static class AtMost
{
    [Fact]
    public static async Task Valid()
    {
        var observer = Observer.For<string>();
        await observer.Add("a");
        await Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .AtMost(1)
            .Match("a")
            .Validate();
    }
        
    [Fact]
    public static Task None()
    {
        var observer = Observer.For<string>();
        return Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .AtMost(2)
            .Match("a")
            .Validate();
    }
        
    [Fact]
    public static async Task More()
    {
        var observer = Observer.For<string>();

        await observer.Add("b");
        await observer.Add("a");
        await observer.Add("a");
            
        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .AtMost(1)
            .Match("a")
            .Validate();
        
        var ex = await act.Should()
            .ThrowAsync<HypothesisInvalidException<string>>()
            .WithMessage("*at most 1*");
        
        ex.Which
            .Matched
            .Should()
            .BeEquivalentTo("a", "a");

        ex.Which
            .Unmatched
            .Should()
            .BeEquivalentTo("b");
    }
        
    [Fact]
    public static async Task Unmatched()
    {
        var observer = Observer.For<string>();
        await observer.Add("b");
        await Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .AtMost(0)
            .Match("a")
            .Validate();
    }
}
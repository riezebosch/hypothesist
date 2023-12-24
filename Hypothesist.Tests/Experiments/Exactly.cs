namespace Hypothesist.Tests.Experiments;

public static class Exactly
{
    [Fact]
    public static async Task Valid()
    {
        var observer = Observer.For<string>();

        await observer.Add("a");
        await observer.Add("a");
            
        await Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Exactly(2)
            .Match("a")
            .Validate();
    }
        
    [Fact]
    public static async Task None()
    {
        var observer = Observer.For<string>();
        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Exactly(2)
            .Match("a")
            .Validate();
        await act.Should()
            .ThrowAsync<HypothesisInvalidException<string>>()
            .WithMessage("*exactly*");
    }
        
    [Fact]
    public static async Task Less()
    {
        var observer = Observer.For<string>();
        await observer.Add("a");
            
        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Exactly(2)
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
    public static async Task More()
    {
        var observer = Observer.For<string>();

        await observer.Add("a");
        await observer.Add("a");
        await observer.Add("a");
            
        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Exactly(2)
            .Match("a")
            .Validate();
        
        var ex = await act.Should()
            .ThrowAsync<HypothesisInvalidException<string>>();

        ex.Which
            .Matched
            .Should()
            .BeEquivalentTo("a", "a", "a");
    }
        
    [Fact]
    public static async Task Unmatched()
    {
        var observer = Observer.For<string>();
        await observer.Add("b");
            
        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Exactly(2)
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
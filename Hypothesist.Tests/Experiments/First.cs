namespace Hypothesist.Tests.Experiments;

public class First
{
    [Fact]
    public Task Valid()
    {
        var observer = Observer.For<string>();
        return Task.WhenAll(observer.Add("a"),
            Hypothesis
                .On(observer)
                .Timebox(12.Seconds())
                .First()
                .Match("a")
                .Validate());
    }

    [Fact]
    public async Task None()
    {
        var observer = Observer.For<string>();
        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .First()
            .Match()
            .Validate();
        await act
            .Should()
            .ThrowAsync<HypothesisInvalidException<string>>()
            .WithMessage("*but none received*");
    }
        
    [Fact]
    public async Task Invalid()
    {
        var observer = Observer.For<string>();

        await observer.Add("b");
        await observer.Add("a");
            
        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .First()
            .Match("a")
            .Validate();
        
        var ex = await act
            .Should()
            .ThrowAsync<HypothesisInvalidException<string>>()
            .WithMessage("*first*");

        ex.Which
            .Matched
            .Should()
            .BeEmpty();

        ex.Which
            .Unmatched
            .Should()
            .BeEquivalentTo("b");
    }
}
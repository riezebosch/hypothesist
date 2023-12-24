namespace Hypothesist.Tests.Experiments;

public class Any
{
    [Fact]
    public Task Valid()
    {
        var observer = Observer.For<string>();
        return Task.WhenAll(observer.Add("a"), Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Any()
            .Match(x => x == "a")
            .Validate());
    }
        
    [Fact]
    public async Task Invalid()
    {
        var observer = Observer.For<string>();
        await observer
            .Add("b");

        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Any()
            .Match("a")
            .Validate();
        var ex = await act
            .Should()
            .ThrowAsync<HypothesisInvalidException<string>>();

        ex.Which
            .Unmatched
            .Should()
            .BeEquivalentTo("b");
    }
        
    [Fact]
    public async Task Next()
    {
        var observer = Observer.For<string>();

        await observer.Add("a");
        await observer.Add("b");

        await Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Any()
            .Match("b")
            .Validate();
    }
}
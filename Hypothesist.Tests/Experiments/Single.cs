namespace Hypothesist.Tests.Experiments;

public class Single
{
    [Fact]
    public Task Valid()
    {
        var observer = Observer.For<string>();
        return Task.WhenAll(observer.Add("a"), Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Single()
            .Match("a")
            .Validate());
    }
        
    [Fact]
    public Task Others()
    {
        var observer = Observer.For<string>();
        var hypothesis = Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Single()
            .Match("a");

        return Task.WhenAll(
            observer.Add("b"),
            observer.Add("d"),
            observer.Add("a"),
            observer.Add("e"),
            observer.Add("f"),
            hypothesis.Validate());
    }
        
    [Fact]
    public async Task None()
    {
        var observer = Observer.For<string>();
        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Single()
            .Match("a")
            .Validate();
        
        var ex = await act
            .Should()
            .ThrowAsync<HypothesisInvalidException<string>>();

        ex.Which
            .Unmatched
            .Should()
            .BeEmpty();
    }
        
    [Fact]
    public async Task Invalid()
    {
        var observer = Observer.For<string>();
        await observer.Add("b");
            
        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Single()
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
    public async Task More()
    {
        var observer = Observer.For<string>();

        await observer.Add("a");
        await observer.Add("b");
        await observer.Add("a");
            
        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Single()
            .Match("a")
            .Validate();
        
        var ex = await act
            .Should()
            .ThrowAsync<HypothesisInvalidException<string>>();

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
    public async Task FailFast()
    {
        var observer = Observer.For<string>();
        var validate = Hypothesis
            .On(observer.WithTimeout(1.Seconds()))
            .Timebox(1.Seconds())
            .Single()
            .Match("a")
            .Validate();
        
        var first = await Task.WhenAny(observer.AddSlowly("a", "a", "a", "a"), validate);
        first.Should().Be(validate);
    }
}
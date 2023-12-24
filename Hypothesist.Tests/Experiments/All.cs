namespace Hypothesist.Tests.Experiments;

public class All
{
    [Fact]
    public Task Empty() => 
        Hypothesis
            .On(Observer.For<string>())
            .Timebox(1.Seconds())
            .All()
            .Match()
            .Validate();
        
    [Fact]
    public Task Valid()
    {
        var observer = Observer.For<string>();
        var hypothesis = Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .All()
            .Match("a");

        return Task.WhenAll(observer.Add("a"), hypothesis.Validate());
    }

    [Fact]
    public async Task Invalid()
    {
        var observer = Observer.For<string>();

        await observer.Add("a");
        await observer.Add("b");
            
        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .All()
            .Match(s => s == "a")
            .Validate();
        
        var ex = await act
            .Should()
            .ThrowAsync<HypothesisInvalidException<string>>();

        ex.WithMessage("""
                       *all samples*one did not.
                       Matched:
                       * a
                       Unmatched:
                       * b
                       """.ReplaceLineEndings());

        ex.Which
            .Matched
            .Should()
            .BeEquivalentTo("a");
            
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

        var act = () => Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .All()
            .Match("a")
            .Validate();
        
        var ex = await act
            .Should()
            .ThrowAsync<HypothesisInvalidException<string>>();

        ex.Which
            .Matched
            .Should()
            .BeEquivalentTo("a");

        ex.Which
            .Unmatched
            .Should()
            .BeEquivalentTo("b");
    }
}
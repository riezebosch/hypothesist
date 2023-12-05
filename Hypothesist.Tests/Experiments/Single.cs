namespace Hypothesist.Tests.Experiments;

public class Single
{
    [Fact]
    public async Task Valid()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .Single(x => x == "a");

        await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate(1.Seconds()));
    }
        
    [Fact]
    public async Task Others()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .Single(x => x == "a");

        await Task.WhenAll(hypothesis.Test("b"),
            hypothesis.Test("d"),
            hypothesis.Test("a"),
            hypothesis.Test("e"),
            hypothesis.Test("f"),
            hypothesis.Validate(1.Seconds()));
    }
        
    [Fact]
    public async Task None()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .Single(x => x == "a");

        var act = () => hypothesis.Validate(1.Seconds());
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
        var hypothesis = Hypothesis
            .For<string>()
            .Single(x => x == "a");
            
        await hypothesis.Test("b");
            
        var act = () => hypothesis.Validate(1.Seconds());
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
        var hypothesis = Hypothesis
            .For<string>()
            .Single(x => x == "a");
            
            
        await hypothesis.Test("a");
        await hypothesis.Test("b");
        await hypothesis.Test("a");
            
        var act = () => hypothesis.Validate(1.Seconds());
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
        var hypothesis = Hypothesis
            .For<string>()
            .Single(x => x == "a");
                
        var validate = hypothesis.Validate(1.Seconds());
        var first = await Task.WhenAny(hypothesis.TestSlowly("a", "a", "a", "a"), validate);
            
        first.Should().Be(validate);
    }
}
namespace Hypothesist.Tests.Experiments;

public static class Exactly
{
    [Fact]
    public static async Task Valid()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .Exactly(2, x => x == "a");

        await hypothesis.Test("a");
        await hypothesis.Test("a");
            
        await hypothesis
            .Validate(2.Seconds());
    }
        
    [Fact]
    public static async Task None()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .Exactly(2, x => x == "a");

        var act = () => hypothesis.Validate(1.Seconds());
        await act.Should()
            .ThrowAsync<HypothesisInvalidException<string>>()
            .WithMessage("*exactly*");
    }
        
    [Fact]
    public static async Task Less()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .Exactly(2, x => x == "a");
            
        await hypothesis.Test("a");
            
        var act = () => hypothesis.Validate(1.Seconds());
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
        var hypothesis = Hypothesis
            .For<string>()
            .Exactly(2, x => x == "a");
            
        await hypothesis.Test("a");
        await hypothesis.Test("a");
        await hypothesis.Test("a");
            
        var act = () => hypothesis.Validate(1.Seconds());
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
        var hypothesis = Hypothesis
            .For<string>()
            .Exactly(2, x => x == "a");
            
        await hypothesis.Test("b");
            
        var act = () => hypothesis.Validate(1.Seconds());
        var ex = await act.Should()
            .ThrowAsync<HypothesisInvalidException<string>>();

        ex.Which
            .Unmatched
            .Should()
            .BeEquivalentTo("b");
    }
}
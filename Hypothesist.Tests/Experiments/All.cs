using System.Diagnostics;

namespace Hypothesist.Tests.Experiments;

public class All
{
    [Fact]
    public async Task Empty() => 
        await Hypothesis
            .For<string>()
            .All(x => x == "a")
            .Validate(1.Seconds());
        
    [Fact]
    public async Task Valid()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .All(x => x == "a");

        await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate(1.Seconds()));
    }

    [Fact]
    public async Task Invalid()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .All(y => y == "a");

        await hypothesis.Test("a");
        await hypothesis.Test("b");
            
        var act = () => hypothesis.Validate(1.Seconds());
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
        var hypothesis = Hypothesis
            .For<string>()
            .All(y => y == "a");
            
        await hypothesis.Test("a");
        await hypothesis.Test("b");

        var act = () => hypothesis.Validate(1.Seconds());
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
        
    [Fact]
    public async Task Sliding()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .All(y => y == "a");

        var act = () => Task.WhenAll(
            hypothesis.TestSlowly("a", "a", "a", "a", "b"), 
            hypothesis.Validate(2.Seconds()));
        await act
            .Should()
            .ThrowAsync<HypothesisInvalidException<string>>();
    }
        
    [Fact]
    public async Task Cancel()
    {
        using var tcs = new CancellationTokenSource(2.Seconds());
        var hypothesis = Hypothesis
            .For<string>()
            .All(_ => true);

        var sw = Stopwatch.StartNew();
        await hypothesis.Validate(1.Minutes(), tcs.Token);
        
        sw.Elapsed.Should().BeLessThan(5.Seconds());
    }
}
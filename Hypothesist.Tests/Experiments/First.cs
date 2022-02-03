using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Hypothesist.Tests.Experiments;

public class First
{
    [Fact]
    public async Task Valid()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .First(x => x == "a");

        await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate(1.Seconds()));
    }
        
    [Fact]
    public async Task None()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .First(_ => true);

        var act = () => hypothesis.Validate(1.Seconds());
        await act
            .Should()
            .ThrowAsync<InvalidException<string>>()
            .WithMessage("*none*");
    }
        
    [Fact]
    public async Task Invalid()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .First(y => y == "a");
            
        await hypothesis.Test("b");
        await hypothesis.Test("a");
            
        var act = () => hypothesis.Validate(1.Seconds());
        var ex = await act
            .Should()
            .ThrowAsync<InvalidException<string>>()
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
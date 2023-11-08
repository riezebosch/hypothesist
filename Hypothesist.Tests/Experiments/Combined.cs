using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Hypothesist.Tests.Experiments;

public static class Combined
{
    [Fact]
    public static async Task Match()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .Any(x => x == "a")
            .All(x => x != "b")
            .Single(x => x == "c");

        await hypothesis.Test("a");
        await hypothesis.Test("c");

        await hypothesis.Validate(2.Seconds());
    }
        
    [Fact]
    public static async Task Invalid()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .Any(x => x == "a")
            .All(x => x != "b");

        await hypothesis.Test("a");
        await hypothesis.Test("b");

        var act = () => hypothesis.Validate(2.Seconds());
        await act.Should()
            .ThrowAsync<HypothesisInvalidException<string>>();
    }
}
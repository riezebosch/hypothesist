namespace Hypothesist.Tests;

public static class InvalidExceptionTests
{
    [Fact]
    public static void Message()
    {
        var exception = new HypothesisInvalidException<string>(
            "expectations not met", 
            new[] { "this is matched" },
            new[] { "this is unmatched" });

        exception
            .Message
            .Should()
            .Be("""
                expectations not met
                Matched:
                * this is matched
                Unmatched:
                * this is unmatched

                """.ReplaceLineEndings());
    }
    
    [Fact]
    public static void None()
    {
        var exception = new HypothesisInvalidException<string>(
            "expectations not met", 
            Array.Empty<string>(),
            Array.Empty<string>());

        exception
            .Message
            .Should()
            .Be("""
                expectations not met
                Matched:
                   <none>
                Unmatched:
                   <none>

                """.ReplaceLineEndings());
    }
    
    [Fact]
    public static void Null()
    {
        var exception = new HypothesisInvalidException<string>(
            "expectations not met", 
            new []{ "1", null },
            Array.Empty<string>());

        exception
            .Message
            .Should()
            .Be("""
                expectations not met
                Matched:
                * 1
                * null
                Unmatched:
                   <none>

                """.ReplaceLineEndings());
    }
}
namespace Hypothesist.Tests.Experiments;

[Obsolete]
public class Old
{
    [Fact]
    public Task All()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .All(x => x == "a");

        return Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate(1.Seconds()));
    }
    
    [Fact]
    public Task Any()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .Any(x => x == "a");

        return Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate(1.Seconds()));
    }
    
    [Fact]
    public Task Anything()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .Any();

        return Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate(1.Seconds()));
    }
    
    [Fact]
    public static async Task AtLeast()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .AtLeast(2, x => x == "a");

        await hypothesis.Test("a");
        await hypothesis.Test("a");
            
        await hypothesis
            .Validate(20.Minutes());
    }
    
    [Fact]
    public static async Task Exactly()
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
    public Task First()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .First(x => x == "a");

        return Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate(1.Seconds()));
    }
    
    [Fact]
    public Task Single()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .Single(x => x == "a");

        return Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate(1.Seconds()));
    }

    [Fact]
    public static Task Sliding()
    {
        var hypothesis = Hypothesis
            .For<string>()
            .Any(x => x == "b");

        return Task.WhenAll(hypothesis.AddSlowly("a", "a", "a", "a", "a", "b"), hypothesis.Validate(2.Seconds()));
    }
}
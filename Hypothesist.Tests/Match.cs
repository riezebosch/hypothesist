namespace Hypothesist.Tests;

public class Match
{
    [Fact]
    public async Task True()
    {
        var observer = Observer.For<Guid>();
        await observer.Add(Guid.NewGuid());
        await Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Any()
            .Match()
            .Validate();
    }
    
    [Fact]
    public async Task Predicate()
    {
        var observer = Observer.For<string>();
        await observer.Add("apple");
        await Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Any()
            .Match(s => s[0] == 'a')
            .Validate();
    }
        
    [Fact]
    public async Task Value()
    {
        var observer = Observer.For<string>();
        await observer.Add("apple");
        await Hypothesis
            .On(observer)
            .Timebox(1.Seconds())
            .Any()
            .Match("apple")
            .Validate();
    }
}
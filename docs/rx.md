# Rx

The `IAsyncEnumerable` provides a possible infinite stream of data. In
order to validate if *all* items match or to fail an *any*  items matches 
when none do you need to introduce a time constraint to indicate an end to the stream.

```csharp
var observer = Observer.For<string>();
await observer.Add("a");

var result = await observer
    .Timebox(5.Seconds())
    .AllAsync(x => x == "a"); // <-- from: System.Linq.Async

result
    .Should()
    .BeTrue(); // <-- from: FluentAssertions
```

Here `.Timebox(TimeSpan)` from `Hypothesist`
is used to introduce the time constraint in the (hypothetical) infinite stream
of data. This method combined both the `.WithTimeout(TimeSpan)` with `.UntilCancelled()`
from this library.

Although this approach works fine, you lack the context you are used to when using
`FluentAssertions`. That additional context of items that didn't match is provided
when using the `Hypothesis.On()` for validation. 
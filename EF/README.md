[![nuget](https://img.shields.io/nuget/v/Hypothesist.EF.svg)](https://www.nuget.org/packages/Hypothesist.EF/)

# Hypothesist.EF

Use [Hypothesist](https://nuget.org/packages/hypothesist) to observe and validate entity events on an [Entity Framework](https://learn.microsoft.com/en-us/ef/) context.

## Arrange

```c#
await using var context = new TestContext(...);
var observer = context.ObserverFor<Item>();
```

## Act

```c#
context.Items.Add(new Item(0));
await context.SaveChangesAsync();
```

## Assert

```c#
await Hypothesis
    .On(observer)
    .Timebox(2.Seconds())
    .Any()
    .Match(new Item(1))
    .Validate();
```

## Filter

If you need the state change of the entity in your validation, 
specify the filter on the observer:

```csharp
var observer = context.ObserverFor<Item>(EntityState.Added);
```

or:

```csharp
var observer = context.ObserverFor<Item>(args => args.State == EntityState.Added);
```

## Remark

Don't use this for normal straightforward in-process entity framework events where you can just use
normal async/await operations. Use this as a validation entry-point for out-of-process interactions
from a test with your system-under-test!

A great example is with [dapr](https://dapr.io), where:

1. your test invokes the sidecar
2. the sidecar sends a message to a [pub/sub broker](https://docs.dapr.io/developing-applications/building-blocks/pubsub/pubsub-overview/)
3. the broker puts a request on your API endpoint
4. the API endpoint invokes some business logic
5. the business logic ultimately stores something in a database

Here you leverage the pluggable component of the database context to validate that the expected entity event
 occured. Another option is to hook into the middleware of [ASP.NET](https://learn.microsoft.com/en-us/aspnet/core/),
for example using the [ASP.NET adapter](../AspNet).
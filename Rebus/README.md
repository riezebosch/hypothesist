[![nuget](https://img.shields.io/nuget/v/Hypothesist.Rebus.svg)](https://www.nuget.org/packages/Hypothesist.Rebus/)

# Hypothesist.Rebus

Use [Hypothesist](https://nuget.org/packages/hypothesist) to validate received messages as a [Rebus](https://github.com/rebus-org/) handler.

## Arrange

```c#
var expected = new Message(1234);
var observer = new Observer<Message>();
```

```c#
using var activator = new BuiltinHandlerActivator()
    .Register(observer.AsHandler); // <-- here's the magic

var bus = Configure.With(activator)
    .Transport(t => t.UseRabbitMq("...")
    .Start();

await bus.Subscribe<Message>();
```

## Act

```c#
await bus.Publish(new Message(1234)); // <-- from the system under test
```

## Assert

```c#
await Hypothesis
    .On(observer)
    .Timebox(2.Seconds())
    .Any()
    .Match(expected)
    .Validate();
```

Slightly more convenient then the inline handler method: `activator.Handle<Message>(m => observer.Observe(m))`.
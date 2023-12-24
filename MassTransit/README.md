[![nuget](https://img.shields.io/nuget/v/Hypothesist.MassTransit.svg)](https://www.nuget.org/packages/Hypothesist.MassTransit/)

# Hypothesist.MassTransit

Use [Hypothesist](https://nuget.org/packages/hypothesist) to validate received messages as a [MassTransit](https://masstransit-project.com) consumer. 

## Arrange

```c#
var expected = new Message(1234); // <-- records are awesome!
var observer = Observer.For<Message>();
```

```c#
var bus = Bus.Factory
    .CreateUsingRabbitMq(cfg =>
    {
        cfg.ReceiveEndpoint("...", e =>
        {
            e.Consumer(observer.AsConsumer);
        });
    });
await bus.StartAsync();
```

## Act

```c#
var endpoint = await bus.GetPublishSendEndpoint<Message>();
await endpoint.Send(message);
```

## Assert

```c#
await hypothesis
    .On(observer)
    .Timebox(10.Seconds())
    .Any()
    .Match(expected)
    .Validate();
```
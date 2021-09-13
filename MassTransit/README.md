# Hypothesist.MassTransit

Use [Hypothesist](https://nuget.org/packages/hypothesist) to validate received messages as a [MassTransit](https://masstransit-project.com) consumer. 

## Arrange

```c#
var message = new Message(1234); // <-- records are awesome!
var hypothesis = Hypothesis
    .For<Message>()
    .Any(x => x == message);
```

```c#
var bus = Bus.Factory
    .CreateUsingRabbitMq(cfg =>
    {
        cfg.ReceiveEndpoint("...", e =>
        {
            e.Consumer(hypothesis.AsConsumer);
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
await hypothesis.Validate(10.Seconds());
```
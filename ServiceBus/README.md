# Hypothesist: Azure ServiceBus adapter 

Easily test your ServiceBus integration by hooking up your hypothesis test into a service bus processor and receiver.

```csharp
// Arrange
var observer = new Observer<ServiceBusReceivedMessage>();

await using var client = new ServiceBusClient(..., new DefaultAzureCredential());
await using var processor = await observer.For(client.CreateProcessor(...));

// Act
await sut.Something(); // publish something to servicebus

// Assert
await hypothesis
    .On(observer)
    .Timebox(10.Seconds())
    .Any()
    .Match(m => m.Body.ToString() == "data")
    .Validate();
```

and for the receiver:

```csharp
await using var receiver = await observer.For(client.CreateReceiver(...));
```

See the [docs](https://github.com/riezebosch/hypothesist) to get a better understanding of what hypothesist is and can do for you.
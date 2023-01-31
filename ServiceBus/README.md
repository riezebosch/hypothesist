# Hypothesist: Azure ServiceBus adapter 

Easily test your ServiceBus integration by hooking up your hypothesis test into a service bus processor and receiver.

```csharp
// Arrange
var hypothesis = Hypothesis
    .For<ServiceBusReceivedMessage>()
    .Any(m => m.Body.ToString() == "data");

await using var client = new ServiceBusClient(..., new DefaultAzureCredential());
await using var processor = await client.CreateProcessor(...).Test(hypothesis);

// Act
await sut.Something(); // publish something to servicebus

// Assert
await hypothesis.Validate(10.Seconds());
```

and for the receiver:

```csharp
await using var receiver = await client.CreateReceiver(...).Test(hypothesis);
```
Checkout the [docs](https://github.com/riezebosch/hypothesist) to get a better understanding of what hypothesist can do for you.
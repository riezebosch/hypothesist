# Hypothesist: Azure ServiceBus adapter 

Easily test your ServiceBus integration by hooking up your hypothesis test into a service bus processor.

```csharp
// Arrange
var hypothesis = Hypothesis
    .For<ServiceBusReceivedMessage>()
    .Any(m => m.Body.ToString() == "data");

await using var client = new ServiceBusClient("...", new DefaultAzureCredential());
await using var processor = client.CreateProcessor("...");
await processor.Test(hypothesis);

// Act
await sut.Something(); // publishes something to ServiceBus

// Assert
await hypothesis.Validate(10.Seconds());
```

Checkout the [docs](https://github.com/riezebosch/hypothesist) to get a better understanding of what hypothesist can do for you.
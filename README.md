# Hypothesis

> The future assertion library for .NET.

This library is there to help you do assertions on events that are about to happen in the near future.
For example, when building integration tests for a subscriber on an asynchronous service bus.

Define your hypothesis:

```c#
var hypothesis = Future
    .Any<Data>(x => x.Should().Be(new Data { Value = 1234 }))
    .Within(TimeSpan.FromSeconds(10));
```

Test the hypothesis:

```c#
// injected into the system under test and invoked from another thread
var service = Substitute.For<IDemoService>();
service
    .When(x => x.Demo(Arg.Any<Data>()))
    .Do(x => hypothesis.Test(x.Arg<Data>()));
```

Validate the hypothesis:

```c#
await hypothesis.Validate();
```

Somewhere in between you fire off the eventing mechanism that ultimately invokes the injected service.

## Experiments

The two parts of the hypothesis are the experiment and a time constraint.

### Any

Validates that _at least one_ item matches the assertion, meaning the experiment stops when this item is observed.

### All

Validates that _all_ items that are observed during the experiment match the assertion.

Remark: having no items observed during the time window also means the hypothesis holds true;

### First

Validates that _the first_ item that is observed matches the assertion.

### Single

Validates that _exactly one_ item is observed and it matches the assertion.

## Time Constraint

Since an experiment can only run for a certain period you have to specify the time constraint.

### Within

Specify the duration between each observation, meaning this is a sliding window which is reset on each observation.

### Forever

Let the experiment run forever or until you cancel the supplied cancellation token.

## Assertion

The single requirement for the assertion you provide to the experiment is that it throws an exception when the observed item does not meet your expectations. 
Use the assertion library of your liking, e.g. FluentAssertions.
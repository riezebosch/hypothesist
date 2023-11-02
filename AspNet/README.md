[![nuget](https://img.shields.io/nuget/v/Hypothesist.AspNet.svg)](https://www.nuget.org/packages/Hypothesist.AspNet/)

# Hypothesist.AspNet

Use [Hypothesist](https://nuget.org/packages/hypothesist) to validate received requests (from an external invocation) via [ASP.NET middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/).

## Arrange

```c#
var hypothesis = Hypothesis
    .For<string>()
    .Any(x => x == "some-data");
```

```c#
var builder = WebApplication.CreateBuilder(args);

await using var app = builder.Build();
app.MapGet("/hello", (string data) => Results.Ok());
app.Use(hypothesis
    .Test()
    .FromRequest()
    .Select(request => request.Query["data"]!));
```

or:

```csharp
app.Use(hypothesis
    .Test()
    .FromRequest()
    .Body(body => JsonSerializer.DeserializeAsync<Guid>(body)));
```

**Remark**: the [order of middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-7.0#middleware-order)
is _very_ important! If you plugin the hypothesis _after_ the body is read by other middleware, you will receive an empty stream and thus no content.

and when there are more invocations to be expected on other routes:

```csharp
app.UseWhen(context => context.Request.Path == "/hello", then => then
    .Use(hypothesis
        .Test()
        .FromRequest()
        .Select(request => request.Query["data"]!)));
```

## Act

Invocation on the endpoint from some external program, like:

```bash
curl 'http://localhost:1234/hello?data=some-input'
```

## Assert

```c#
await hypothesis
    .Validate(2.Seconds());
```

[![nuget](https://img.shields.io/nuget/v/Hypothesist.AspNet.svg)](https://www.nuget.org/packages/Hypothesist.AspNet/)

# Hypothesist.AspNet

Use [Hypothesist](https://nuget.org/packages/hypothesist) to validate received requests (from an external invocation) via [ASP.NET middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/).

## Arrange

Define the hypothesis:

```c#
var observer = Observer.For<string>();
```

Insert the middleware to test from incoming requests:

```c#
var builder = WebApplication.CreateBuilder(args);

await using var app = builder.Build();
app.MapGet("/hello", (string data) => Results.Ok());
app.Use(observer
    .FromRequest()
    .With(request => request.Query["data"]!));
```

or read from the body:

```csharp
app.Use(observer
    .FromRequest()
    .Body(body => JsonSerializer.DeserializeAsync<Guid>(body)));
```

**Remark**: the [order of middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-7.0#middleware-order)
is _very_ important! If you plugin the hypothesis _after_ the body is read by other middleware, you will receive an empty stream and thus no content.

Only test for a specific route:

```csharp
app.UseWhen(context => context.Request.Path == "/hello", then => then
    .Use(observer
        .FromRequest()
        .With(request => request.Query["data"]!)));
```

or directly test from an [endpoint filter](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/min-api-filters?view=aspnetcore-7.0):

```csharp
app.MapPost("/hello", ([FromBody]Guid body) => Results.Ok(body))
    .AddEndpointFilter(observer
        .FromEndpoint()
        .With(context => context.GetArgument<Guid>(0)));
```

## Act

Invocation on the endpoint from some external program, like:

```bash
curl 'http://localhost:1234/hello?data=some-input'
```

## Assert

```c#
await Hypothesis
    .On(observer)
    .Timebox(2.Seconds());
    .Any()
    .Match("some-data")
    .Validate();
```

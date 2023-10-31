[![nuget](https://img.shields.io/nuget/v/Hypothesist.AspNet.svg)](https://www.nuget.org/packages/Hypothesist.AspNet/)

# Hypothesist.AspNet

Use [Hypothesist](https://nuget.org/packages/hypothesist) to validate received requests via [ASP.NET middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/).

## Arrange

```c#
var data = "some-input"; 
var hypothesis = Hypothesis
    .For<Message>()
    .Any(x => x == data);
```

```c#
var builder = WebApplication.CreateBuilder(args);

await using var app = builder.Build();
app.MapGet("/hello", () => Results.Ok());
app.UseDeveloperExceptionPage();
app.Use(hypothesis.AsMiddleware(request => request.Query["data"]!));
```

## Act

```bash
curl 'http://localhost:1234/hello?data=some-input'
```

## Assert

```c#
await hypothesis
    .Validate(2.Seconds());
```

using System.Net;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet.Tests;

public class Middleware
{
    private readonly Uri _url = new Uri("http://localhost:1234");

    [Fact]
    public async Task Request()
    {
        var input = Guid.NewGuid();
        var hypothesis = Hypothesis
            .For<Guid>()
            .Any(s => s == input);

        await using var app = Setup(_url);
        app.Use(hypothesis.AsMiddleware(request => Guid.Parse(request.Query["data"]!)));
        
        await app.StartAsync();
        await Test(_url, input);

        await hypothesis.Validate(3.Seconds());
    }
    
    [Fact]
    public async Task Async()
    {
        var input = Guid.NewGuid();
        var hypothesis = Hypothesis
            .For<string>()
            .Any(string.IsNullOrEmpty);

        await using var app = Setup(_url);
        app.Use(hypothesis.AsMiddleware(async request => await new StreamReader(request.Body).ReadToEndAsync()));
        
        await app.StartAsync();
        await Test(_url, input);

        await hypothesis.Validate(3.Seconds());
    }

    private static WebApplication Setup(Uri url)
    {
        var builder = WebApplication.CreateBuilder(new[] { $"--urls={url}" });
        var app = builder.Build();
        app.MapGet("/hello", () => Results.Ok());
        app.UseDeveloperExceptionPage();
        
        return app;
    }

    private static async Task Test(Uri url, Guid input)
    {
        using var client = new HttpClient { BaseAddress = url };
        using var response = await client.GetAsync($"hello?data={input}");
        response
            .StatusCode
            .Should()
            .Be(HttpStatusCode.OK, await response.Content.ReadAsStringAsync());
    }
}
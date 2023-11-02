using System.Text.Json;
using FluentAssertions;
using FluentAssertions.Extensions;
using Flurl.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hypothesist.AspNet.Tests;

public class FromRequest
{
    private readonly Uri _url = new("http://localhost:1234");

    [Fact]
    public async Task Select()
    {
        var input = Guid.NewGuid();
        var hypothesis = Hypothesis
            .For<Guid>()
            .Any(s => s == input);

        await using var app = Setup(_url);
        app.Use(hypothesis
            .Test()
            .FromRequest()
            .Select(request => Guid.Parse(request.Query["data"]!)));
        
        await app.StartAsync();
        await Test(_url, input);

        await hypothesis
            .Validate(3.Seconds());
    }
    
    [Fact]
    public async Task UseWhen()
    {
        var input = Guid.NewGuid();
        var hypothesis = Hypothesis
            .For<Guid>()
            .Any(s => s == input);

        await using var app = Setup(_url);
        app.UseWhen(context => context.Request.Path == "/hello", then => then
            .Use(hypothesis
                .Test()
                .FromRequest()
                .Select(request => Guid.Parse(request.Query["data"]!))));
    
        await app.StartAsync();
        await Test(_url, input);

        await hypothesis
            .Validate(3.Seconds());
    }
    
    [Fact]
    public async Task Body()
    {
        var input = Guid.NewGuid();
        var hypothesis = Hypothesis
            .For<Guid>()
            .Any(s => s == input);

        await using var app = Setup(_url);
        app.Use(hypothesis
            .Test()
            .FromRequest()
            .Body(stream => JsonSerializer.DeserializeAsync<Guid>(stream)));
        
        await app.StartAsync();
        await Test(_url, input);

        await hypothesis
            .Validate(3.Seconds());
    }

    private static WebApplication Setup(Uri url)
    {
        var builder = WebApplication.CreateBuilder(new[] { $"--urls={url}" });
        var app = builder.Build();
        app.UseDeveloperExceptionPage();
        
        app.MapPost("/hello", ([FromBody]Guid body) => Results.Ok(body));

        return app;
    }

    private static async Task Test(Uri url, Guid input)
    {
        using var response = await (url + $"hello?data={input}").PostJsonAsync(input);
        response.StatusCode.Should().Be(200);
        
        var result = await response.GetJsonAsync<Guid>();
        result.Should().Be(input);
    }
}
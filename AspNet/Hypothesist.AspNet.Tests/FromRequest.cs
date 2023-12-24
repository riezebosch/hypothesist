using System.Text.Json;
using FluentAssertions;
using FluentAssertions.Extensions;
using Flurl;
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
        var observer = Observer.For<Guid>();

        await using var app = Setup(_url);
        app.Use(observer
            .FromRequest()
            .Select(request => Guid.Parse(request.Query["data"]!)));
        
        await app.StartAsync();
        await Test(_url, input);

        await Hypothesis
            .On(observer)
            .Timebox(3.Seconds())
            .Any()
            .Match(input)
            .Validate();
    }
    
    [Fact]
    public async Task UseWhen()
    {
        var input = Guid.NewGuid();
        var observer = Observer.For<Guid>();

        await using var app = Setup(_url);
        app.UseWhen(context => context.Request.Path == "/hello", then => then
            .Use(observer
                .FromRequest()
                .Select(request => Guid.Parse(request.Query["data"]!))));
    
        await app.StartAsync();
        await Test(_url, input);

        await Hypothesis
            .On(observer)
            .Timebox(3.Seconds())
            .Any()
            .Match(input)
            .Validate();
    }
    
    [Fact]
    public async Task Body()
    {
        var input = Guid.NewGuid();
        var observer = Observer.For<Guid>();

        await using var app = Setup(_url);
        app.Use(observer
            .FromRequest()
            .Body(stream => JsonSerializer.DeserializeAsync<Guid>(stream)));
        
        await app.StartAsync();
        await Test(_url, input);

        await Hypothesis
            .On(observer)
            .Timebox(3.Seconds())
            .Any()
            .Match(input)
            .Validate();
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
        using var response = await url
            .AppendPathSegment("hello")
            .SetQueryParam("data", input)
            .PostJsonAsync(input);
        
        response.StatusCode.Should().Be(200);
        
        var result = await response.GetJsonAsync<Guid>();
        result.Should().Be(input);
    }
}
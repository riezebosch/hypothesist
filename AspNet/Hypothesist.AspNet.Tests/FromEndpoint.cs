using FluentAssertions.Extensions;
using Flurl;
using Flurl.Http;
using Hypothesist.AspNet.Endpoint;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hypothesist.AspNet.Tests;

public class FromEndpoint
{
    private readonly Uri _url = new("http://localhost:2345");

    [Fact]
    public async Task Select()
    {
        var input = Guid.NewGuid();
        var hypothesis = Hypothesis
            .For<Guid>()
            .Any(s => s == input);

        var builder = WebApplication.CreateBuilder(new[] { $"--urls={_url}" });
        await using var app = builder.Build();
        app.UseDeveloperExceptionPage();
        
        app.MapPost("/hello", ([FromBody]Guid body) => Results.Ok(body))
            .AddEndpointFilter(hypothesis
                .Test()
                .FromEndpoint()
                .Select(context => context.GetArgument<Guid>(0)));
        
        await app.StartAsync();
        _ = await _url.AppendPathSegment("hello").PostJsonAsync(input);

        await hypothesis.Validate(3.Seconds());
    }
    
    [Fact]
    public async Task When()
    {
        var hypothesis = Hypothesis
            .For<int>()
            .Any();

        var builder = WebApplication.CreateBuilder(new[] { $"--urls={_url}" });
        await using var app = builder.Build();
        app.UseDeveloperExceptionPage();

        var group = app.MapGroup("")
            .AddEndpointFilter(hypothesis
                .Test()
                .FromEndpoint()
                .Select(context => context.GetArgument<int>(0))
                .When(context => context.HttpContext.Request.Path == "/a"));
        
        group.MapPost("/a", ([FromBody]int body) => Results.Ok());
        group.MapPost("/b", ([FromBody]Guid body) => Results.Ok());
        
        await app.StartAsync();
        _ = await _url.AppendPathSegment("a").PostJsonAsync(10);
        _ = await _url.AppendPathSegment("b").PostJsonAsync(Guid.NewGuid());

        await hypothesis.Validate(3.Seconds());
    }
}
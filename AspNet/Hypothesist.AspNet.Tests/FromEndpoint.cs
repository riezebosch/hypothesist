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
        var observer = Observer.For<Guid>();

        var builder = WebApplication.CreateBuilder(new[] { $"--urls={_url}" });
        await using var app = builder.Build();
        app.UseDeveloperExceptionPage();
        app.MapPost("/hello", ([FromBody]Guid body) => Results.Ok(body))
            .AddEndpointFilter(observer
                .FromEndpoint()
                .With(context => context.GetArgument<Guid>(0)));
        
        await app.StartAsync();
        _ = await _url.AppendPathSegment("hello").PostJsonAsync(input);

        await Hypothesis
            .On(observer)
            .Timebox(3.Seconds())
            .Any()
            .Match(input)
            .Validate();
    }
    
    [Fact]
    public async Task When()
    {
        var observer = Observer.For<int>();

        var builder = WebApplication.CreateBuilder(new[] { $"--urls={_url}" });
        await using var app = builder.Build();
        app.UseDeveloperExceptionPage();

        var group = app.MapGroup("")
            .AddEndpointFilter(observer
                .FromEndpoint()
                .With(context => context.GetArgument<int>(0))
                .When(context => context.HttpContext.Request.Path == "/a"));
        
        group.MapPost("/a", ([FromBody]int body) => Results.Ok());
        group.MapPost("/b", ([FromBody]Guid body) => Results.Ok());
        
        await app.StartAsync();
        _ = await _url.AppendPathSegment("a").PostJsonAsync(10);
        _ = await _url.AppendPathSegment("b").PostJsonAsync(Guid.NewGuid());

        await Hypothesis
            .On(observer)
            .Timebox(3.Seconds())
            .Any()
            .Match(10)
            .Validate();
    }
}
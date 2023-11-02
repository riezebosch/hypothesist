using FluentAssertions.Extensions;
using Flurl.Http;
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
        await (_url + "hello").PostJsonAsync(input);

        await hypothesis.Validate(3.Seconds());
    }
}
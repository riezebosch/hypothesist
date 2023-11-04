#if NET7_0
using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet.Endpoint;

internal class When : IEndpointFilter
{
    private readonly IEndpointFilter _invoke;
    private readonly Predicate<EndpointFilterInvocationContext> _when;

    public When(Predicate<EndpointFilterInvocationContext> when, IEndpointFilter invoke)
    {
        _when = when;
        _invoke = invoke;
    }

    async ValueTask<object?> IEndpointFilter.InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) => 
        _when(context) 
            ? _invoke.InvokeAsync(context, next) 
            : await next(context);
}
#endif
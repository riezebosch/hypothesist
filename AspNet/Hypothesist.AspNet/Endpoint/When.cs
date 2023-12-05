#if NET7_0_OR_GREATER
using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet.Endpoint;

internal class When(Predicate<EndpointFilterInvocationContext> when, IEndpointFilter invoke)
    : IEndpointFilter
{
    ValueTask<object?> IEndpointFilter.InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) => 
        when(context) 
            ? invoke.InvokeAsync(context, next) 
            : next(context);
}
#endif
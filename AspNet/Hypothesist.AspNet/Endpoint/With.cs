#if NET7_0_OR_GREATER
using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet.Endpoint;

internal class With<T> : IEndpointFilter
{
    private readonly Func<EndpointFilterInvocationContext, Task> _sample;

    public With(Observer<T> observer, Func<EndpointFilterInvocationContext, T> select) => 
        _sample = context => observer.Add(select(context));

    async ValueTask<object?> IEndpointFilter.InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        await _sample(context);
        return await next(context);
    }
}
#endif
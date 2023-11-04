#if NET7_0
using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet.Endpoint;

internal class Select<T> : IEndpointFilter
{
    private readonly Func<EndpointFilterInvocationContext, Task> _test;

    public Select(IHypothesis<T> hypothesis, Func<EndpointFilterInvocationContext, T> select) => 
        _test = context => hypothesis.Test(select(context));

    async ValueTask<object?> IEndpointFilter.InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        await _test(context);
        return await next(context);
    }
}
#endif
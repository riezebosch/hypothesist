#if NET7_0
using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet.Endpoint;

public class When<T>
{
    private readonly From<T> _endpoint;
    private readonly Predicate<EndpointFilterInvocationContext> _when;

    public When(From<T> endpoint, Predicate<EndpointFilterInvocationContext> when)
    {
        _endpoint = endpoint;
        _when = when;
    }

    public Func<EndpointFilterInvocationContext, EndpointFilterDelegate, ValueTask<object?>> Select(Func<EndpointFilterInvocationContext, T> select) =>
        async (context, next) => 
            _when(context)
                ? await _endpoint.Select(select)(context, next) 
                : await next(context);
}
#endif
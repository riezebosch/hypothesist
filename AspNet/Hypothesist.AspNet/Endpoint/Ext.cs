#if NET7_0
using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet.Endpoint;

public static class Ext
{
    public static Func<EndpointFilterInvocationContext, EndpointFilterDelegate, ValueTask<object?>> When(
        this Func<EndpointFilterInvocationContext, EndpointFilterDelegate, ValueTask<object?>> filter,
        Predicate<EndpointFilterInvocationContext> when) => 
        (context, next) => when(context)
            ? filter(context, next)
            : next(context);
}
#endif
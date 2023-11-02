#if NET7_0
using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet;

public class FromEndpoint<T>
{
    private readonly IHypothesis<T> _hypothesis;

    public FromEndpoint(IHypothesis<T> hypothesis) => _hypothesis = hypothesis;

    public Func<EndpointFilterInvocationContext, EndpointFilterDelegate, ValueTask<object?>> Select(Func<EndpointFilterInvocationContext, T> select) =>
        async (context, next) =>
        {
            await _hypothesis.Test(select(context));
            return await next(context);
        };
}
#endif
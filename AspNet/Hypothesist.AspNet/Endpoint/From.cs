#if NET7_0
using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet.Endpoint;

public class From<T>
{
    private readonly IHypothesis<T> _hypothesis;

    public From(IHypothesis<T> hypothesis) => _hypothesis = hypothesis;

    public Func<EndpointFilterInvocationContext, EndpointFilterDelegate, ValueTask<object?>> Select(Func<EndpointFilterInvocationContext, T> select) =>
        async (context, next) =>
        {
            await _hypothesis.Test(select(context));
            return await next(context);
        };

    public When<T> When(Predicate<EndpointFilterInvocationContext> when) => new(this, when);
}
#endif
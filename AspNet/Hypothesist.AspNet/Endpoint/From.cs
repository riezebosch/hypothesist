#if NET7_0_OR_GREATER
using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet.Endpoint;

public class From<T>(IHypothesis<T> hypothesis)
{
    public IEndpointFilter Select(Func<EndpointFilterInvocationContext, T> select) => 
        new Select<T>(hypothesis, select);
}
#endif
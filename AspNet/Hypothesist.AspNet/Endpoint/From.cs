#if NET7_0_OR_GREATER
using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet.Endpoint;

public class From<T>(Observer<T> observer)
{
    public IEndpointFilter With(Func<EndpointFilterInvocationContext, T> select) => 
        new With<T>(observer, select);
}
#endif
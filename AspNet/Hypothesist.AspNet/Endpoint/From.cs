#if NET7_0
using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet.Endpoint;

public class From<T>
{
    private readonly IHypothesis<T> _hypothesis;

    public From(IHypothesis<T> hypothesis) => 
        _hypothesis = hypothesis;

    public IEndpointFilter Select(Func<EndpointFilterInvocationContext, T> select) => 
        new Select<T>(_hypothesis, select);
}
#endif
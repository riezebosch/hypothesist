using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet;

public class Test<T>
{
    private readonly IHypothesis<T> _hypothesis;

    public Test(IHypothesis<T> hypothesis) => _hypothesis = hypothesis;

    public FromRequest<T> FromRequest() => new(_hypothesis);
}
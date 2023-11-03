namespace Hypothesist.AspNet;

public class Test<T>
{
    private readonly IHypothesis<T> _hypothesis;

    public Test(IHypothesis<T> hypothesis) => _hypothesis = hypothesis;

    public Request.From<T> FromRequest() => new(_hypothesis);

    #if NET7_0
    public Endpoint.From<T> FromEndpoint() => new(_hypothesis);
    #endif
}
namespace Hypothesist.AspNet;

public class Test<T>(IHypothesis<T> hypothesis)
{
    public Request.From<T> FromRequest() => new(hypothesis);

    #if NET7_0_OR_GREATER
    public Endpoint.From<T> FromEndpoint() => new(hypothesis);
    #endif
}
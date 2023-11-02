namespace Hypothesist.AspNet;

public class Test<T>
{
    private readonly IHypothesis<T> _hypothesis;

    public Test(IHypothesis<T> hypothesis) => _hypothesis = hypothesis;

    public FromRequest<T> FromRequest() => new(_hypothesis);

    #if NET7_0
    public FromEndpoint<T> FromEndpoint() => new(_hypothesis);
    #endif
}
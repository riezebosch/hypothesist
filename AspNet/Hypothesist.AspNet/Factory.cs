namespace Hypothesist.AspNet;

public static class Factory
{
    public static Request.From<T> FromRequest<T>(this Observer<T> observer) => new(observer);

#if NET7_0_OR_GREATER
    public static Endpoint.From<T> FromEndpoint<T>(this Observer<T> observer) => new(observer);
#endif
}
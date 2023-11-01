using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet;

public static class Factory
{
    public static Func<HttpContext, RequestDelegate, Task> AsMiddleware<T>(this IHypothesis<T> hypothesis, Func<HttpRequest, T> select) =>
        async (context, next) =>
        {
            await next(context);
            await hypothesis.Test(select(context.Request));
        };
    
    public static Func<HttpContext, RequestDelegate, Task> AsMiddleware<T>(this IHypothesis<T> hypothesis, Func<HttpRequest, Task<T>> select) =>
        async (context, next) =>
        {
            await next(context);
            await hypothesis.Test(await select(context.Request));
        };
}
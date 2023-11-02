using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet;

public static class Factory
{
    public static Func<HttpContext, RequestDelegate, Task> TestFromRequest<T>(this IHypothesis<T> hypothesis, Func<HttpRequest, T> select) =>
        async (context, next) =>
        {
            await hypothesis.Test(select(context.Request));
            await next(context);
        };
    
    public static Func<HttpContext, RequestDelegate, Task> TestFromRequest<T>(this IHypothesis<T> hypothesis, Func<HttpRequest, Task<T>> select) =>
        async (context, next) =>
        {
            await hypothesis.Test(await select(context.Request));
            await next(context);
        };

    public static Func<HttpContext, RequestDelegate, Task> TestFromRequestBody<T>(this IHypothesis<T> hypothesis, Func<Stream, Task<T>> body) =>
        hypothesis.TestFromRequest(request => request.Stream(body));

    private static async Task<T> Stream<T>(this HttpRequest request, Func<Stream, Task<T>> select)
    {
        request.EnableBuffering();
        var body = await select(request.Body);
        request.Body.Position = 0;

        return body;
    }
}
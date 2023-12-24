using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet.Request;

public class From<T>(Observer<T> observer)
{
    public Func<HttpContext, RequestDelegate, Task> Select(Func<HttpRequest, T> select) =>
        async (context, next) =>
        {
            await observer.Add(select(context.Request));
            await next(context);
        };

    public Func<HttpContext, RequestDelegate, Task> Body(Func<Stream, ValueTask<T>> body) =>
        async (context, next) =>
        {
            await observer.Add(await Stream(context.Request, body));
            await next(context);
        };

    private static async ValueTask<T> Stream(HttpRequest request, Func<Stream, ValueTask<T>> select)
    {
        request.EnableBuffering();
        var body = await select(request.Body);
        request.Body.Position = 0;

        return body;
    }
}
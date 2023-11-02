using Microsoft.AspNetCore.Http;

namespace Hypothesist.AspNet;

public class FromRequest<T>
{
    private readonly IHypothesis<T> _hypothesis;

    public FromRequest(IHypothesis<T> hypothesis) => _hypothesis = hypothesis;

    public Func<HttpContext, RequestDelegate, Task> Select(Func<HttpRequest, T> select) =>
        async (context, next) =>
        {
            await _hypothesis.Test(select(context.Request));
            await next(context);
        };

    public Func<HttpContext, RequestDelegate, Task> Body(Func<Stream, ValueTask<T>> body) =>
        async (context, next) =>
        {
            await _hypothesis.Test(await Stream(context.Request, body));
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
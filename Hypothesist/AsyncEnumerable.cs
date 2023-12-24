using System.Runtime.CompilerServices;

namespace Hypothesist;

public static class AsyncEnumerable
{
    public static IAsyncEnumerable<T> WithTimeout<T>(this IAsyncEnumerable<T> items, TimeSpan timeout, CancellationToken token = default) =>
        items.WithTimeout(timeout, TimeSpan.Zero, token);
    
    public static async IAsyncEnumerable<T> WithTimeout<T>(this IAsyncEnumerable<T> items, TimeSpan initial, TimeSpan sliding, [EnumeratorCancellation] CancellationToken token = default)
    {
        using var source = CancellationTokenSource.CreateLinkedTokenSource(token);
        source.CancelAfter(initial);
            
        await foreach (var item in items.WithCancellation(source.Token))
        {
            yield return item;
            if (sliding != TimeSpan.Zero)
            {
                source.CancelAfter(sliding);
            }
        }
    }
    
    public static async IAsyncEnumerable<T> UntilCancelled<T>(this IAsyncEnumerable<T> items, [EnumeratorCancellation] CancellationToken token = default)
    {
        await using var enumerator = items.GetAsyncEnumerator(token);
        while (await enumerator.MoveNextAsync().CatchOperationCanceled())
        {
            yield return enumerator.Current;
        }
    }

    private static async Task<bool> CatchOperationCanceled(this ValueTask<bool> task)
    {
        try
        {
            return await task;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }

    public static IAsyncEnumerable<T> Timebox<T>(this IAsyncEnumerable<T> items, TimeSpan duration) => 
        items.WithTimeout(duration).UntilCancelled();
}
using System.Runtime.CompilerServices;

namespace Hypothesist;

internal static class EnumerableExtensions
{
    public static async IAsyncEnumerable<T> Sliding<T>(this IAsyncEnumerable<T> items, TimeSpan window, [EnumeratorCancellation]CancellationToken token)
    {
        using var source = CancellationTokenSource.CreateLinkedTokenSource(token);
        source.CancelAfter(window);
            
        await foreach (var item in items.WithCancellation(source.Token))
        {
            yield return item;
            source.CancelAfter(window);
        }
    }
}
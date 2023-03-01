#if !NETSTANDARD2_1_OR_GREATER
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;

namespace Hypothesist;

internal static class ChannelReaderExtensions
{
    /// <summary>Creates an <see cref="IAsyncEnumerable{T}"/> that enables reading all of the data from the channel.</summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use to cancel the enumeration.</param>
    /// <remarks>
    /// Each <see cref="IAsyncEnumerator{T}.MoveNextAsync"/> call that returns <c>true</c> will read the next item out of the channel.
    /// <see cref="IAsyncEnumerator{T}.MoveNextAsync"/> will return false once no more data is or will ever be available to read.
    /// </remarks>
    /// <returns>The created async enumerable.</returns>
    public static async IAsyncEnumerable<T> ReadAllAsync<T>(this ChannelReader<T> reader, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (await reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
        {
            while (reader.TryRead(out T item))
            {
                yield return item;
            }
        }
    }
}
#endif

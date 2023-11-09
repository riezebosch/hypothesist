#if !NETSTANDARD2_1_OR_GREATER
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;

namespace Hypothesist;

/// <summary>
/// source: https://github.com/dotnet/runtime/blob/95147163dac477da5177f5c5402ae9b93feb5c89/src/libraries/System.Threading.Channels/src/System/Threading/Channels/ChannelReader.netstandard21.cs
/// </summary>
internal static class ChannelReaderPolyfill
{
    public static async IAsyncEnumerable<T> ReadAllAsync<T>(this ChannelReader<T> reader, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (await reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
        {
            while (reader.TryRead(out var item))
            {
                yield return item;
            }
        }
    }
}
#endif

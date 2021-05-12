using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;

namespace Hypothesize
{
    internal static class Time
    {
        public static async IAsyncEnumerable<T> TimeConstraint<T>(this ChannelReader<T> reader, TimeSpan window, [EnumeratorCancellation] CancellationToken token)
        {
            using var source = CancellationTokenSource.CreateLinkedTokenSource(token);
            source.CancelAfter(window);
            
            await foreach (var item in reader.ReadAllAsync(source.Token))
            {
                yield return item;
                source.CancelAfter(window);
            }
        }
    }
}
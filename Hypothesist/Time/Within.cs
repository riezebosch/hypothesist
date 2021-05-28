using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;

namespace Hypothesist.Time
{
    internal class Within<T> : IConstraint<T>
    {
        private readonly TimeSpan _window;

        public Within(TimeSpan window) => 
            _window = window;

        async IAsyncEnumerable<T> IConstraint<T>.Read(ChannelReader<T> reader, [EnumeratorCancellation] CancellationToken token)
        {
            using var source = CancellationTokenSource.CreateLinkedTokenSource(token);
            source.CancelAfter(_window);
            
            await foreach (var item in reader.ReadAllAsync(source.Token))
            {
                yield return item;
                source.CancelAfter(_window);
            }
        }
    }
}
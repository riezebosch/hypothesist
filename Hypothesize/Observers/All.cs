using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Hypothesize.Observers
{
    internal sealed class All<T> : IObservers
    {
        private readonly Action<T> _assert;
        private readonly ChannelReader<T> _reader;

        public All(Action<T> assert, ChannelReader<T> reader)
        {
            _assert = assert;
            _reader = reader;
        }

        async Task IObservers.Observe(TimeSpan window, CancellationToken token)
        {
            using var source = CancellationTokenSource.CreateLinkedTokenSource(token);
            source.CancelAfter(window);

            try
            {
                await foreach (var item in _reader.ReadAllAsync(source.Token))
                {
                    _assert(item);
                    source.CancelAfter(window);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Hypothesize.Observers
{
    internal sealed class Single<T> : IObserve<T>
    {
        private readonly Action<T> _assert;

        public Single(Action<T> assert) =>
            _assert = assert;

        async Task IObserve<T>.Observe(ChannelReader<T> reader, TimeSpan window, CancellationToken token)
        {
            var valid = 0;
            try
            {
                using var source = CancellationTokenSource.CreateLinkedTokenSource(token);
                source.CancelAfter(window);

                await foreach (var message in reader.ReadAllAsync(source.Token))
                {
                    _assert(message);
                    valid++;

                    if (valid > 1)
                    {
                        throw new InvalidOperationException();
                    }
                    
                    source.CancelAfter(window);
                }
            }
            catch (OperationCanceledException)
            {
                if (valid == 0)
                {
                    throw new TimeoutException();
                }
            }
        }
    }
}
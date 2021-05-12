using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Hypothesize.Observers
{
    public class First<T> : IObserve<T>
    {
        private readonly Action<T> _assert;

        public First(Action<T> assert) => 
            _assert = assert;

        async Task IObserve<T>.Observe(ChannelReader<T> reader, TimeSpan window, CancellationToken token)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            cts.CancelAfter(window);
            
            try
            {
                _assert(await reader.ReadAsync(cts.Token));
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException();
            }
        }
    }
}
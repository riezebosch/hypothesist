using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Hypothesize.Observers
{
    internal sealed class Any<T> : IObservers
    {
        private readonly Action<T> _theorem;
        private readonly ChannelReader<T> _reader;

        public Any(Action<T> theorem, ChannelReader<T> reader)
        {
            _theorem = theorem;
            _reader = reader;
        }

        async Task IObservers.Observe(TimeSpan window, CancellationToken token)
        {
            var exceptions = new List<Exception>();

            try
            {
                using var source = CancellationTokenSource.CreateLinkedTokenSource(token);
                source.CancelAfter(window);

                await foreach (var message in _reader.ReadAllAsync(source.Token))
                {
                    try
                    {
                        _theorem(message);
                        return;
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }

                    source.CancelAfter(window);
                }
            }
            catch (OperationCanceledException)
            {
                throw exceptions.Any() 
                    ? new AggregateException(exceptions) 
                    : new TimeoutException();
            }
        }
    }
}
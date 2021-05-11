using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Prognosis.Theorems
{
    internal sealed class Any<T> : ITheorem<T>
    {
        private readonly Action<T> _theorem;
        private readonly Channel<T> _messages = Channel.CreateUnbounded<T>();

        public Any(Action<T> theorem) => 
            _theorem = theorem;

        async Task ITheorem.Within(TimeSpan window, CancellationToken token)
        {
            var exceptions = new List<Exception>();

            try
            {
                using var source = CancellationTokenSource.CreateLinkedTokenSource(token);
                source.CancelAfter(window);

                await foreach (var message in _messages.Reader.ReadAllAsync(source.Token))
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

        async Task ITheorem<T>.Prove(T item) => 
            await _messages.Writer.WriteAsync(item);
    }
}
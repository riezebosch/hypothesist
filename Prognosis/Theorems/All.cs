using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Prognosis.Theorems
{
    internal sealed class All<T> : ITheorem<T>
    {
        private readonly Action<T> _theorem;
        private readonly Channel<T> _messages = Channel.CreateUnbounded<T>();

        public All(Action<T> theorem) => 
            _theorem = theorem;

        async Task ITheorem.Within(TimeSpan window, CancellationToken token)
        {
            using var source = CancellationTokenSource.CreateLinkedTokenSource(token);
            source.CancelAfter(window);

            try
            {
                await foreach (var item in _messages.Reader.ReadAllAsync(source.Token))
                {
                    _theorem(item);
                    source.CancelAfter(window);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        async Task ITheorem<T>.Prove(T item) => 
            await _messages.Writer.WriteAsync(item);
    }
}
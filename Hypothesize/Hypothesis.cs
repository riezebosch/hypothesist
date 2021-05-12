using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Hypothesize.Observers;

namespace Hypothesize
{
    internal class Hypothesis<T> : IHypothesis<T>
    {
        private readonly Channel<T> _channel;
        private readonly IObserve<T> _observer;
        private readonly TimeSpan _window;
        private readonly CancellationToken _token;

        public Hypothesis(Channel<T> channel, IObserve<T> observer, TimeSpan window, CancellationToken token)
        {
            _channel = channel;
            _observer = observer;
            _window = window;
            _token = token;
        }

        public Task Validate() =>
            _observer.Observe(_channel.Reader, _window, _token);

        public async Task Test(T item) =>
            await _channel.Writer.WriteAsync(item, _token);
    }
}
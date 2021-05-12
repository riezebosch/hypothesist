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

        public Hypothesis(Channel<T> channel, IObserve<T> observer, TimeSpan window)
        {
            _channel = channel;
            _observer = observer;
            _window = window;
        }

        public Task Validate(CancellationToken token) =>
            _observer.Observe(_channel.Reader.TimeConstraint(_window, token));

        public async Task Test(T item, CancellationToken token) =>
            await _channel.Writer.WriteAsync(item, token);
    }
}
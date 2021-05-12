using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Hypothesize.Observers;

namespace Hypothesize
{
    internal class Hypothesis<T> : IHypothesis<T>
    {
        private readonly ChannelWriter<T> _writer;
        private readonly IObservers _observers;
        private readonly TimeSpan _window;
        private readonly CancellationToken _token;

        public Hypothesis(ChannelWriter<T> writer, IObservers observers, TimeSpan window, CancellationToken token)
        {
            _writer = writer;
            _observers = observers;
            _window = window;
            _token = token;
        }

        public Task Validate() =>
            _observers.Observe(_window, _token);

        public async Task Test(T item) =>
            await _writer.WriteAsync(item, _token);
    }
}
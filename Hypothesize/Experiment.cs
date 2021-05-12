using System;
using System.Threading;
using System.Threading.Channels;
using Hypothesize.Observers;

namespace Hypothesize
{
    internal class Experiment<T> : IExperiment<T>
    {
        private readonly IObservers _observers;
        private readonly ChannelWriter<T> _writer;

        public Experiment(IObservers observers, ChannelWriter<T> writer)
        {
            _observers = observers;
            _writer = writer;
        }

        public IHypothesis<T> Within(TimeSpan window, CancellationToken token) =>
            new Hypothesis<T>(_writer, _observers, window, token);
        
        public IHypothesis<T> Forever(CancellationToken token) =>
            new Hypothesis<T>(_writer, _observers, TimeSpan.FromMilliseconds(-1), token);
    }
}
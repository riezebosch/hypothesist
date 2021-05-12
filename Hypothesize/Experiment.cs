using System;
using System.Threading;
using System.Threading.Channels;
using Hypothesize.Observers;
    
namespace Hypothesize
{
    internal class Experiment<T> : IExperiment<T>
    {
        private readonly IObserve<T> _observer;
        private readonly Channel<T> _channel;

        public Experiment(IObserve<T> observer, Channel<T> channel)
        {
            _observer = observer;
            _channel = channel;
        }

        public IHypothesis<T> Within(TimeSpan window, CancellationToken token) =>
            new Hypothesis<T>(_channel, _observer, window, token);
        
        public IHypothesis<T> Forever(CancellationToken token) =>
            new Hypothesis<T>(_channel, _observer, TimeSpan.FromMilliseconds(-1), token);
    }
}
using System;
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

        public IHypothesis<T> Within(TimeSpan window) =>
            new Hypothesis<T>(_channel, _observer, window);
        
        public IHypothesis<T> Forever() =>
            new Hypothesis<T>(_channel, _observer, TimeSpan.FromMilliseconds(-1));
    }
}
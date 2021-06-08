using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Hypothesist.Observers;
using Hypothesist.Time;

namespace Hypothesist
{
    internal class Hypothesis<T> : IHypothesis<T>
    {
        private readonly Channel<T> _channel = Channel.CreateUnbounded<T>();
        private readonly List<IExperiment<T>> _experiments = new();

        async Task IHypothesis<T>.Validate(TimeSpan period, CancellationToken token)
        {
            try
            {
                await foreach (var sample in _channel.Reader.ReadAllAsync(token).Sliding(period, token))
                {
                    _experiments.ForEach(x => x.OnNext(sample));
                    if (_experiments.All(x => x.Done))
                    {
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _experiments.ForEach(x => x.OnCompleted());
            }
        }

        async Task IHypothesis<T>.Test(T sample, CancellationToken token) =>
            await _channel.Writer.WriteAsync(sample, token);

        public IHypothesis<T> Add(IExperiment<T> observer)
        {
            _experiments.Add(observer);
            return this;
        }
    }
}
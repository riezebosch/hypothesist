using System;
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
        private readonly IExperiment<T> _experiment;

        public Hypothesis(IExperiment<T> experiment) => _experiment = experiment;

        async Task IHypothesis<T>.Validate(TimeSpan period, CancellationToken token)
        {
            try
            {
                await foreach (var sample in _channel.Reader.ReadAllAsync(token).Sliding(period, token))
                {
                    _experiment.OnNext(sample);
                    if (_experiment.Done)
                    {
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _experiment.OnCompleted();
            }
        }

        async Task IHypothesis<T>.Test(T sample, CancellationToken token) =>
            await _channel.Writer.WriteAsync(sample, token);
    }
}
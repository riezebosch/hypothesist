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
        private readonly IObserve<T> _observer;
        private readonly IConstraint<T> _constraint;
        private readonly Predicate<T> _match;

        public Hypothesis(IObserve<T> observer, IConstraint<T> constraint, Predicate<T> match)
        {
            _observer = observer;
            _constraint = constraint;
            _match = match;
        }

        Task IHypothesis<T>.Validate(CancellationToken token) =>
            _observer.Observe(_match, _constraint.Read(_channel.Reader, token));

        async Task IHypothesis<T>.Test(T sample, CancellationToken token) =>
            await _channel.Writer.WriteAsync(sample, token);
    }
}
using System;
using Hypothesist.Observers;
using Hypothesist.Time;

namespace Hypothesist.Builders
{
    internal class Window<T> : IWindow<T>
    {
        private readonly IObserve<T> _observer;
        private readonly IConstraint<T> _constraint;

        public Window(IObserve<T> observer, IConstraint<T> constraint)
        {
            _observer = observer;
            _constraint = constraint;
        }

        IHypothesis<T> IWindow<T>.Matches(Predicate<T> match) =>
            new Hypothesis<T>(_observer, _constraint, match);
        
        IHypothesis<T> IWindow<T>.Matches(Action<T> match) =>
            new Hypothesis<T>(_observer, _constraint, x =>
            {
                try
                {
                    match(x);
                    return true;
                }
                catch
                {
                    return false;
                }
            });

    }
}
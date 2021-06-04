using System;
using System.Collections.Generic;

namespace Hypothesist.Observers
{
    internal sealed class Each<T> : IExperiment<T>
    {
        private readonly Predicate<T> _match;
        private readonly List<T> _matched = new();

        public Each(Predicate<T> match) => 
            _match = match;

        void IObserver<T>.OnCompleted()
        {
        }

        void IObserver<T>.OnError(Exception error)
        {
        }

        void IObserver<T>.OnNext(T value)
        {
            if (!_match(value))
            {
                throw new InvalidException<T>(_matched,  new[] { value });
            }

            _matched.Add(value);
        }

        bool IExperiment<T>.Done => false;
    }
}
using System;
using System.Linq;

namespace Hypothesist.Observers
{
    public class First<T> : IExperiment<T>
    {
        private readonly Predicate<T> _match;

        public First(Predicate<T> match) => 
            _match = match;

        void IObserver<T>.OnCompleted() => 
            throw new InvalidException<T>(Enumerable.Empty<T>(), Enumerable.Empty<T>());

        void IObserver<T>.OnError(Exception error)
        {
        }

        void IObserver<T>.OnNext(T value)
        {
            if (!_match(value))
            {
                throw new InvalidException<T>(Enumerable.Empty<T>(),  new[] { value });
            }

            Done = true;
        }

        public bool Done { get; private set; }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hypothesist.Observers
{
    public class Exactly<T> : IObserve<T>
    {
        private readonly int _occurrences;

        public Exactly(int occurrences) => _occurrences = occurrences;

        async Task IObserve<T>.Observe(Predicate<T> match, IAsyncEnumerable<T> samples)
        {
            var matched = new List<T>();
            var unmatched = new List<T>();

            try
            {
                await foreach (var sample in samples)
                {
                    (match(sample) ? matched : unmatched).Add(sample);
                }
            }
            catch (OperationCanceledException)
            {
            }

            if (matched.Count != _occurrences)
            {
                throw new InvalidException<T>(matched, unmatched);
            }
        }
    }
}
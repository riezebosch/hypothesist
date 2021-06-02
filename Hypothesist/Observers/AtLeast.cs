using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hypothesist.Observers
{
    internal class AtLeast<T> : IObserve<T>
    {
        private readonly int _occurrences;

        public AtLeast(int occurrences) => _occurrences = occurrences;

        async Task IObserve<T>.Observe(Predicate<T> match, IAsyncEnumerable<T> samples)
        {
            var matched = new List<T>();
            var unmatched = new List<T>();

            try
            {
                await foreach (var sample in samples)
                {
                    (match(sample) ? matched : unmatched).Add(sample);
                    if (matched.Count == _occurrences)
                    {
                        return;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw new InvalidException<T>(matched, unmatched);
            }
        }
    }
}
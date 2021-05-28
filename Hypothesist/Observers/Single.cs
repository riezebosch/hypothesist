using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hypothesist.Observers
{
    internal sealed class Single<T> : IObserve<T>
    {
        async Task IObserve<T>.Observe(Predicate<T> match, IAsyncEnumerable<T> samples)
        {
            var matched = new List<T>();
            var unmatched = new List<T>();
            
            try
            {
                await foreach (var sample in samples)
                {
                    (match(sample) ? matched : unmatched).Add(sample);
                    if (matched.Count > 1)
                    {
                        throw new InvalidException<T>(matched, unmatched);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                if (!matched.Any())
                {
                    throw new InvalidException<T>(matched, unmatched);
                }
            }
        }
    }
}
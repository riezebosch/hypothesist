using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hypothesist.Observers
{
    internal sealed class Any<T> : IObserve<T>
    {
        async Task IObserve<T>.Observe(Predicate<T> match, IAsyncEnumerable<T> samples)
        {
            var unmatched = new List<T>();
            try
            {
                await foreach (var sample in samples)
                {
                    if (match(sample))
                    {
                        return;
                    }

                    unmatched.Add(sample);
                }
            }
            catch (OperationCanceledException)
            {
                throw new InvalidException<T>(Enumerable.Empty<T>(),  unmatched);
            }
        }
    }
}
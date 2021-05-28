using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hypothesist.Observers
{
    internal sealed class All<T> : IObserve<T>
    {
        async Task IObserve<T>.Observe(Predicate<T> match, IAsyncEnumerable<T> samples)
        {
            try
            {
                var matched = new List<T>();
                await foreach (var sample in samples)
                {
                    if (!match(sample))
                    {
                        throw new InvalidException<T>(matched,  new[] { sample });
                    }
                    
                    matched.Add(sample);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}
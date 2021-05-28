using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hypothesist.Observers
{
    public class First<T> : IObserve<T>
    {
        async Task IObserve<T>.Observe(Predicate<T> match, IAsyncEnumerable<T> samples)
        {
            try
            {
                await foreach (var sample in samples)
                {
                    if (match(sample))
                    {
                        return;
                    }

                    throw new InvalidException<T>(Enumerable.Empty<T>(),  new[] { sample });
                }
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hypothesize.Observers
{
    public class First<T> : IObserve<T>
    {
        private readonly Action<T> _assert;

        public First(Action<T> assert) => 
            _assert = assert;

        async Task IObserve<T>.Observe(IAsyncEnumerable<T> items)
        {
            try
            {
                await foreach (var item in items)
                {
                    _assert(item);
                    return;
                }
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException();
            }
        }
    }
}
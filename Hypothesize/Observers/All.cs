using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hypothesize.Observers
{
    internal sealed class All<T> : IObserve<T>
    {
        private readonly Action<T> _assert;

        public All(Action<T> assert) => 
            _assert = assert;

        async Task IObserve<T>.Observe(IAsyncEnumerable<T> items)
        {
            try
            {
                await foreach (var item in items)
                {
                    _assert(item);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}
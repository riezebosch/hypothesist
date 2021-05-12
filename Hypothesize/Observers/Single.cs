using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hypothesize.Observers
{
    internal sealed class Single<T> : IObserve<T>
    {
        private readonly Action<T> _assert;

        public Single(Action<T> assert) =>
            _assert = assert;

        async Task IObserve<T>.Observe(IAsyncEnumerable<T> items)
        {
            var valid = 0;
            try
            {
                await foreach (var message in items)
                {
                    _assert(message);
                    valid++;

                    if (valid > 1)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                if (valid == 0)
                {
                    throw new TimeoutException();
                }
            }
        }
    }
}
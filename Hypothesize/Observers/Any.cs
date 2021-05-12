using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hypothesize.Observers
{
    internal sealed class Any<T> : IObserve<T>
    {
        private readonly Action<T> _assert;

        public Any(Action<T> assert) => 
            _assert = assert;

        async Task IObserve<T>.Observe(IAsyncEnumerable<T> items)
        {
            var exceptions = new List<Exception>();

            try
            {
                await foreach (var message in items)
                {
                    try
                    {
                        _assert(message);
                        return;
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw exceptions.Any() 
                    ? new AggregateException(exceptions) 
                    : new TimeoutException();
            }
        }
    }
}
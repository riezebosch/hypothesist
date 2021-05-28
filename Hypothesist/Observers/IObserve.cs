using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hypothesist.Observers
{
    internal interface IObserve<T>
    {
        Task Observe(Predicate<T> match, IAsyncEnumerable<T> samples);
    }
}
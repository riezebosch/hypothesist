using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hypothesize.Observers
{
    internal interface IObserve<in T>
    {
        Task Observe(IAsyncEnumerable<T> items);
    }
}
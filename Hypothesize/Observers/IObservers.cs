using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hypothesize.Observers
{
    internal interface IObservers
    {
        Task Observe(TimeSpan window, CancellationToken token = default);
    }
}
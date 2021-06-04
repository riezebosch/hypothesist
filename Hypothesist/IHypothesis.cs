using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hypothesist
{
    public interface IHypothesis<in T>
    {
        Task Validate(TimeSpan period, CancellationToken token = default);
        Task Test(T item, CancellationToken token = default);
    }
}
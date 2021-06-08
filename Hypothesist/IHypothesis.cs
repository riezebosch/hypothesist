using System;
using System.Threading;
using System.Threading.Tasks;
using Hypothesist.Experiments;

namespace Hypothesist
{
    public interface IHypothesis<T>
    {
        Task Validate(TimeSpan period, CancellationToken token = default);
        Task Test(T item, CancellationToken token = default);
        IHypothesis<T> Add(IExperiment<T> observer);
    }
}
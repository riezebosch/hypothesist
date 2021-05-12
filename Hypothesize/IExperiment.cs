using System;
using System.Threading;

namespace Hypothesize
{
    public interface IExperiment<in T>
    {
        IHypothesis<T> Within(TimeSpan window, CancellationToken token = default);
        IHypothesis<T> Forever(CancellationToken token = default);
    }
}
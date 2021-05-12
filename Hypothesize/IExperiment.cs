using System;

namespace Hypothesize
{
    public interface IExperiment<in T>
    {
        IHypothesis<T> Within(TimeSpan window);
        IHypothesis<T> Forever();
    }
}
using System;

namespace Hypothesist.Experiments;

public interface IExperiment<in T> : IObserver<T>
{
    bool Done { get; }
}
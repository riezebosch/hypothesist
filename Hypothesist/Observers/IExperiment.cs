using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hypothesist.Observers
{
    public interface IExperiment<in T> : IObserver<T>
    {
        bool Done { get; }
    }
}
using System;
using System.Diagnostics.CodeAnalysis;

namespace Hypothesist.Experiments;

public interface IExperiment<in T> : IObserver<T>
{
    [ExcludeFromCodeCoverage]
    void IObserver<T>.OnError(Exception error) => throw new NotImplementedException();
    
    bool Done { get; }
}
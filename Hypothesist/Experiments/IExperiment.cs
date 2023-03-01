using System;
using System.Diagnostics.CodeAnalysis;

namespace Hypothesist.Experiments;

public interface IExperiment<in T> : IObserver<T>
{
    #if NETSTANDARD2_1_OR_GREATER
    [ExcludeFromCodeCoverage]
    void IObserver<T>.OnError(Exception error) => throw new NotImplementedException();
    #endif

    bool Done { get; }
}
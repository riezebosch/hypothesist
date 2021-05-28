using System;
using Hypothesist.Observers;
using Hypothesist.Time;

namespace Hypothesist.Builders
{
    internal class Statement<T> : IStatement<T>
    {
        private readonly IObserve<T> _observer;

        public Statement(IObserve<T> observer) => 
            _observer = observer;

        IWindow<T> IStatement<T>.Within(TimeSpan window) =>
            new Window<T>(_observer, new Within<T>(window));

        IWindow<T> IStatement<T>.Forever() =>
            new Window<T>(_observer, new Forever<T>());
    }
}
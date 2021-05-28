using System;

namespace Hypothesist.Builders
{
    public interface IWindow<T>
    {
        IHypothesis<T> Should(Predicate<T> match);
        IHypothesis<T> Should(Action<T> match);
    }
}
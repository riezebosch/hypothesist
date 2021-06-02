using System;

namespace Hypothesist.Builders
{
    public interface IWindow<T>
    {
        IHypothesis<T> Matches(Predicate<T> match);
        IHypothesis<T> Matches(Action<T> match);
    }
}
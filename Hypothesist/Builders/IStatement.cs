using System;

namespace Hypothesist.Builders
{
    public interface IStatement<T>
    {
        IWindow<T> Within(TimeSpan window);
        IWindow<T> Forever();
    }
}
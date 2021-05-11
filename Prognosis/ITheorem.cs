using System;
using System.Threading;
using System.Threading.Tasks;

namespace Prognosis
{
    public interface ITheorem
    {
        Task Within(TimeSpan window, CancellationToken token = default);
    }

    public interface ITheorem<in T> : ITheorem
    {
        Task Prove(T item);
    }
}
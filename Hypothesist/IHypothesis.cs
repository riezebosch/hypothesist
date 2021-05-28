using System.Threading;
using System.Threading.Tasks;

namespace Hypothesist
{
    public interface IHypothesis<in T>
    {
        Task Validate(CancellationToken token = default);
        Task Test(T item, CancellationToken token = default);

    }
}
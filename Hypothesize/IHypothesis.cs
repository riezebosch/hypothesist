using System.Threading.Tasks;

namespace Hypothesize
{
    public interface IHypothesis<in T>
    {
        Task Validate();
        Task Test(T item);

    }
}
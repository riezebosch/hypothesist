using System.Threading.Tasks;
using MassTransit;

namespace Hypothesist;

public static class Factory
{
    public static IConsumer<T> AsConsumer<T>(this IHypothesis<T> hypothesis) where T : class =>
        new Consumer<T>(hypothesis);

    private class Consumer<T> : IConsumer<T> where T : class
    {
        private readonly IHypothesis<T> _hypothesis;

        public Consumer(IHypothesis<T> hypothesis) => 
            _hypothesis = hypothesis;

        public Task Consume(ConsumeContext<T> context) =>
            _hypothesis.Test(context.Message);
    }
}
using System.Threading.Tasks;
using MassTransit;

namespace Hypothesist;

public static class Factory
{
    public static IConsumer<T> AsConsumer<T>(this IHypothesis<T> hypothesis) where T : class =>
        new Consumer<T>(hypothesis);

    private class Consumer<T>(IHypothesis<T> hypothesis) : IConsumer<T>
        where T : class
    {
        public Task Consume(ConsumeContext<T> context) =>
            hypothesis.Test(context.Message);
    }
}
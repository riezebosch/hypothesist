using System.Threading.Tasks;
using MassTransit;

namespace Hypothesist;

public static class Factory
{
    public static IConsumer<T> AsConsumer<T>(this Observer<T> observer) where T : class =>
        new Consumer<T>(observer);

    private class Consumer<T>(Observer<T> observer) : IConsumer<T>
        where T : class
    {
        public Task Consume(ConsumeContext<T> context) =>
            observer.Add(context.Message);
    }
}
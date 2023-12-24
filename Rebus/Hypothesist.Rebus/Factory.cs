using Rebus.Handlers;

namespace Hypothesist.Rebus;

public static class Factory
{
    public static IHandleMessages<TMessage> AsHandler<TMessage>(this Observer<TMessage> observer) => 
        new Handler<TMessage>(observer);

    private class Handler<TMessage>(Observer<TMessage> observer) : IHandleMessages<TMessage>
    {
        Task IHandleMessages<TMessage>.Handle(TMessage message) => 
            observer.Add(message);
    }
}
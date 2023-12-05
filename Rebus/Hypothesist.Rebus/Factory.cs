using Rebus.Handlers;

namespace Hypothesist.Rebus;

public static class Factory
{
    public static IHandleMessages<TMessage> AsHandler<TMessage>(this IHypothesis<TMessage> hypothesis) => 
        new Handler<TMessage>(hypothesis);

    private class Handler<TMessage>(IHypothesis<TMessage> hypothesis) : IHandleMessages<TMessage>
    {
        Task IHandleMessages<TMessage>.Handle(TMessage message) => 
            hypothesis.Test(message);
    }
}
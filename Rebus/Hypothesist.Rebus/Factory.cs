using Rebus.Handlers;

namespace Hypothesist.Rebus;

public static class Factory
{
    public static IHandleMessages<TMessage> AsHandler<TMessage>(this IHypothesis<TMessage> hypothesis) => 
        new Handler<TMessage>(hypothesis);

    private class Handler<TMessage> : IHandleMessages<TMessage>
    {
        private readonly IHypothesis<TMessage> _hypothesis;

        public Handler(IHypothesis<TMessage> hypothesis) => 
            _hypothesis = hypothesis;

        Task IHandleMessages<TMessage>.Handle(TMessage message) => 
            _hypothesis.Test(message);
    }
}
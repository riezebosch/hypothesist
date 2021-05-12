using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Hypothesize.Observers
{
    internal interface IObserve<T>
    {
        Task Observe(ChannelReader<T> reader, TimeSpan window, CancellationToken token);
    }
}
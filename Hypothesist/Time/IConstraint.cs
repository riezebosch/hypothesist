using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;

namespace Hypothesist.Time
{
    internal interface IConstraint<T>
    {
        IAsyncEnumerable<T> Read(ChannelReader<T> reader, CancellationToken token);
    }
}
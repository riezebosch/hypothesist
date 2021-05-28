using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;

namespace Hypothesist.Time
{
    internal class Forever<T> : IConstraint<T>
    {
        public IAsyncEnumerable<T> Read(ChannelReader<T> reader, CancellationToken token) => 
            reader.ReadAllAsync(token);
    }
}
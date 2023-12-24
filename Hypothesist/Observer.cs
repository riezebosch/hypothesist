namespace Hypothesist;

public class Observer<T> : IAsyncEnumerable<T>
{
    private readonly Channel<T> _channel = Channel.CreateUnbounded<T>();

    public async Task Add(T data, CancellationToken token = default) =>
        await _channel.Writer.WriteAsync(data, token);

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new()) => 
        _channel.Reader.ReadAllAsync(cancellationToken).GetAsyncEnumerator(cancellationToken);
}

public static class Observer
{
    public static Observer<T> For<T>() => new();
}
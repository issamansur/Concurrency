using System.Threading.Channels;

namespace MyOwnTests.ProducerConsumer;

public class ChannelProducerConsumer<T> : BaseProducerConsumer<T>
{
    private readonly Channel<T> _channel;

    public ChannelProducerConsumer(int capacity = 100)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _channel = Channel.CreateBounded<T>(options);
    }

    public override async Task EnqueueMessage(T message, CancellationToken cancellationToken = default)
    {
        try
        {
            await _channel.Writer.WriteAsync(message, cancellationToken);
        }
        catch (ChannelClosedException)
        {
        }
    }

    public override async Task ProcessMessages(CancellationToken cancellationToken = default)
    {
        await foreach (var message in _channel.Reader.ReadAllAsync(cancellationToken))
        {
            await ProcessMessage(message, cancellationToken);
        }
    }

    public override Task OnStopAsync(CancellationToken cancellationToken = default)
    {
        _channel.Writer.Complete();
        
        return Task.CompletedTask;
    }
}
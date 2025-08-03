using System.Threading.Tasks.Dataflow;

namespace MyOwnTests.ProducerConsumer;

public class TplDataflowOnlyBufferProducerConsumer<T> : BaseProducerConsumer<T>
{
    private readonly BufferBlock<T> _bufferBlock;

    public TplDataflowOnlyBufferProducerConsumer(int capacity = 100)
    {
        var bufferOptions = new DataflowBlockOptions
        {
            BoundedCapacity = capacity
        };

        _bufferBlock = new BufferBlock<T>(bufferOptions);
    }

    public override Task EnqueueMessage(T message, CancellationToken cancellationToken = default)
    {
        return _bufferBlock.SendAsync(message, cancellationToken);
    }

    public sealed override async Task ProcessMessages(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            try
            {
                var item = await _bufferBlock.ReceiveAsync(cancellationToken);
                await ProcessMessage(item, cancellationToken);
            }
            catch (InvalidOperationException)
            {
                break;
            }
        }
    }

    public override async Task OnStopAsync(CancellationToken cancellationToken = default)
    {
        _bufferBlock.Complete();
        await _bufferBlock.Completion.WaitAsync(cancellationToken);
    }
}
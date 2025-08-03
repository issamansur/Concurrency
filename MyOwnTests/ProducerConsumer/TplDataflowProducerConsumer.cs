using System.Threading.Tasks.Dataflow;

namespace MyOwnTests.ProducerConsumer;

public class TplDataflowProducerConsumer<T> : BaseProducerConsumer<T>
{
    private readonly BufferBlock<T> _bufferBlock;
    private readonly ActionBlock<T> _consumerBlock;

    public TplDataflowProducerConsumer(int capacity = 100, int consumersCount = 1)
    {
        var bufferOptions = new DataflowBlockOptions
        {
            BoundedCapacity = capacity
        };
        _bufferBlock = new BufferBlock<T>(bufferOptions);

        var consumerOptions = new ExecutionDataflowBlockOptions
        {
            MaxDegreeOfParallelism = consumersCount,
            BoundedCapacity = capacity // на всякий
        };

        _consumerBlock = new ActionBlock<T>(async message =>
        {
            await ProcessMessage(message);
        }, consumerOptions);

        _bufferBlock.LinkTo(_consumerBlock, new DataflowLinkOptions { PropagateCompletion = true });
    }

    public override Task EnqueueMessage(T message, CancellationToken cancellationToken = default)
    {
        return _bufferBlock.SendAsync(message, cancellationToken);
    }

    public sealed override Task ProcessMessages(CancellationToken cancellationToken = default)
    {
#if DEBUG
        Console.WriteLine($"{nameof(TplDataflowProducerConsumer<T>)} calls {nameof(ProcessMessage)} automatically.");
        
        return Task.CompletedTask;
#else
        return Task.FromException(
            new InvalidOperationException(
                $"{nameof(TplDataflowProducerConsumer)} calls {nameof(ProcessMessage)} automatically."
            )
        );
#endif
    }

    public override async Task OnStopAsync(CancellationToken cancellationToken = default)
    {
        _bufferBlock.Complete();
        await _consumerBlock.Completion.WaitAsync(cancellationToken);
    }
}
using System.Collections.Concurrent;

namespace MyOwnTests.ProducerConsumer;

public class BlockingCollectionProducerConsumer<T> : BaseProducerConsumer<T>
{
    private readonly BlockingCollection<T> _blockingCollection;

    public BlockingCollectionProducerConsumer(int capacity = 100)
    {
        _blockingCollection = new BlockingCollection<T>(capacity);
    }
    
    public override Task EnqueueMessage(T message, CancellationToken cancellationToken = default)
    {
        try
        {
            _blockingCollection.Add(message, cancellationToken);
        }
        catch (InvalidOperationException)
        {
        }
        return Task.CompletedTask;
    }

    public override Task ProcessMessages(CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            foreach (var item in _blockingCollection.GetConsumingEnumerable(cancellationToken))
            {
                ProcessMessage(item, cancellationToken).Wait(cancellationToken);
            }
        }, cancellationToken);
    }

    public override Task OnStopAsync(CancellationToken cancellationToken = default)
    {
        _blockingCollection.CompleteAdding();
        
        return Task.CompletedTask;
    }
}
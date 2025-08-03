namespace MyOwnTests.ProducerConsumer;


/// <summary>
/// Bad example from interview/challenge
/// </summary>
public sealed class InitProducerConsumer<T> : BaseProducerConsumer<T>
{
    // Unsafe collection for concurrent work
    private readonly Queue<T> _messages = new();
    private readonly int _maxMessages;

    public InitProducerConsumer(int capacity = 100)
    {
        _maxMessages = capacity;
    }

    public override async Task EnqueueMessage(T message, CancellationToken cancellationToken = default)
    {
        if (_messages.Count >= _maxMessages)
        {
            // Hard waiting with static value
            // We'll have more than _maxMessage, even if we'll overload 
            await Task.Delay(100);
        }

        _messages.Enqueue(message);
    }

    public override async Task ProcessMessages(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            if (_messages.Count > 0)
            {
                var message = _messages.Dequeue();
                await ProcessMessage(message);
            }

            // Why do we need to waste processor's time?
            await Task.Delay(100);
        }
    }
}
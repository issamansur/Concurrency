namespace MyOwnTests.Attributes;

public class ThreadLocal
{
    // 1. Here we can see, that we can use ThreadLocal with generics.
    // Also, we can add init value, that works correctly.
    // AND!!! we can use it with NON-STATIC!!!
    public ThreadLocal<int> Value = new(() => 100);

    private readonly int _threadCount;
    private readonly Barrier _barrier;

    public ThreadLocal(int threadCount = 5)
    {
        _threadCount = threadCount;
        _barrier = new Barrier(threadCount);
    }

    private void DoSomething()
    {
        // 2. We can see here, that init value for field with type 'ThreadLocal'
        // works for every thread at start (it's second difference)
        Console.WriteLine(Value);
        
        var threadId = Environment.CurrentManagedThreadId;

        Value.Value = threadId;

        Console.WriteLine($"Thread-{threadId} changed 'Value' on it's own thread id.");
        
        // wait while all threads set value
        _barrier.SignalAndWait();

        // 3. We can see here, that static value in every thread different, not the same.
        // So we have N different values for N threads. 
        Console.WriteLine(Value);
    }

    public void Execute()
    {
        Parallel.Invoke(Enumerable.Repeat(DoSomething, _threadCount).ToArray());
    }
}
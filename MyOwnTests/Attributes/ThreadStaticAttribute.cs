namespace MyOwnTests.Attributes;

public class ThreadStaticAttribute
{
    [System.ThreadStatic]
    public static int Value = 100;

    private readonly int _threadCount;
    private readonly Barrier _barrier;

    public ThreadStaticAttribute(int threadCount = 5)
    {
        _threadCount = threadCount;
        _barrier = new Barrier(threadCount);
    }

    private void DoSomething()
    {
        // 1. We can see here, that init value for field with 'System.ThreadStatic' attribute
        // works only for 1 thread (which take it first :D ). For other - it has default (0 for int)
        Console.WriteLine(Value);
        
        var threadId = Environment.CurrentManagedThreadId;

        Value = threadId;

        Console.WriteLine($"Thread-{threadId} changed 'Value' on it's own thread id.");
        
        // wait while all threads set value
        _barrier.SignalAndWait();

        // 2. We can see here, that static value in every thread different, not the same.
        // So we have N different values for N threads. 
        Console.WriteLine(Value);
    }

    public void Execute()
    {
        Parallel.Invoke(Enumerable.Repeat(DoSomething, _threadCount).ToArray());
    }
}
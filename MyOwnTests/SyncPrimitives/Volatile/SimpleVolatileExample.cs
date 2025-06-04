namespace MyOwnTests.SyncPrimitives.Volatile;

// RUN IN RELEASE MODE TO ENABLE OPTIMIZATION
public static class SimpleVolatileExample
{
    // Example with no volatile var (unexpected behaviour)
    private static void Worker(Object o) {
        Int32 x = 0;
        while (!_stopWorker) x++;
        Console.WriteLine("Worker: stopped when x={0}", x);
    }
    
    private static Boolean _stopWorker = false;
    
    public static void UnexpectedBehaviour() {
        Console.WriteLine("Main: letting worker run for 5 seconds");
        Thread t = new Thread(Worker!);
        t.Start();
        Thread.Sleep(5000);
        _stopWorker = true;
        Console.WriteLine("Main: waiting for worker to stop");
        t.Join();
    }

    // Example with volatile var (expected behaviour)
    private static void WorkerVolatile(Object o) {
        Int32 x = 0;
        while (!_stopWorkerVolatile) x++;
        Console.WriteLine("Worker: stopped when x={0}", x);
    }

    private static volatile Boolean _stopWorkerVolatile = false;
    
    public static void ExpectedBehaviour() {
        Console.WriteLine("Main: letting worker run for 5 seconds");
        Thread t = new Thread(WorkerVolatile!);
        t.Start();
        Thread.Sleep(5000);
        _stopWorkerVolatile = true;
        Console.WriteLine("Main: waiting for worker to stop");
        t.Join();
    }
}
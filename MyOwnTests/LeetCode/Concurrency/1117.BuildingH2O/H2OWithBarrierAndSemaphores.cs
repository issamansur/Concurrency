namespace MyOwnTests.LeetCode.Concurrency._1117.BuildingH2O;

// Solution with Barrier and Semaphores (I think this is the best solution)
// Barrier is used to synchronize threads and Semaphores are used to control the number of threads
// We can also use SemaphoreSlim for async operations

// Remember to remove " : ISolution" from code snippet
using System.Threading;

public class H2OWithBarrierAndSemaphores : IH2O
{
    private readonly SemaphoreSlim _hydrogenSemaphore = new(2, 2);
    private readonly SemaphoreSlim _oxygenSemaphore = new(1, 1);

    private readonly Barrier _barrier = new(3);

    public void Hydrogen(Action releaseHydrogen)
    {
        _hydrogenSemaphore.Wait();
        // releaseHydrogen() outputs "H". Do not change or remove this line.
        releaseHydrogen();
        _barrier.SignalAndWait();
        _hydrogenSemaphore.Release();
    }

    public void Oxygen(Action releaseOxygen)
    {
        _oxygenSemaphore.Wait();
        // releaseOxygen() outputs "O". Do not change or remove this line.
        releaseOxygen();
        _barrier.SignalAndWait();
        _oxygenSemaphore.Release();
    }
}
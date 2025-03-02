namespace MyOwnTests.LeetCode.Concurrency._1115.PrintFooBarAlternately;

// Solution with SemaphoreSlim (good, but not the best)
// I think for this problem we can use AutoResetEvent
// 1. We don't need to async wait, 'cause we have only 2 threads
// 2. AutoResetEvent is more lightweight than SemaphoreSlim


// Remember to remove " : ISolution" from code snippet
using System.Threading;

public class FooBarWithSemaphoreSlim(int n) : IFooBar
{
    private readonly SemaphoreSlim _semaphoreFoo = new(1, 1);
    private readonly SemaphoreSlim _semaphoreBar = new(0, 1);

    public void Foo(Action printFoo)
    {
        for (int i = 0; i < n; i++)
        {
            _semaphoreFoo.Wait();
            // printFoo() outputs "foo". Do not change or remove this line.
            printFoo();
            _semaphoreBar.Release();
        }
    }

    public void Bar(Action printBar)
    {
        for (int i = 0; i < n; i++)
        {
            _semaphoreBar.Wait();
            // printBar() outputs "bar". Do not change or remove this line.
            printBar();
            _semaphoreFoo.Release();
        }
    }
}
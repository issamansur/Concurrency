namespace MyOwnTests.LeetCode.Concurrency._1115.PrintFooBarAlternately;

// Solution with AutoResetEvent (good)
// I think also this is the best solution
// FOR THIS PROBLEM
// Interesting, but SignalAndWait is faster than WaitOne and Set

// Remember to remove " : ISolution" from code snippet
using System.Threading;

public class FooBarWithARE2(int n) : IFooBar
{
    private readonly AutoResetEvent _are1 = new(false);
    private readonly AutoResetEvent _are2 = new(false);

    public void Foo(Action printFoo) {
        
        for (int i = 0; i < n; i++)
        {
            _are1.WaitOne();
            // printFoo() outputs "foo". Do not change or remove this line.
            printFoo();
            _are2.Set();
        }
    }

    public void Bar(Action printBar) {
        
        for (int i = 0; i < n; i++)
        {
            WaitHandle.SignalAndWait(_are1, _are2);
            // printBar() outputs "bar". Do not change or remove this line.
            printBar();
        }
    }
}
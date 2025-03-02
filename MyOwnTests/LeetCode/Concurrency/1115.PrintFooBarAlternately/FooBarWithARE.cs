namespace MyOwnTests.LeetCode.Concurrency._1115.PrintFooBarAlternately;

// Solution with AutoResetEvent (good)
// I think also this is the best solution
// FOR THIS PROBLEM

// Remember to remove " : ISolution" from code snippet
using System.Threading;

public class FooBarWithARE(int n) : IFooBar
{
    private readonly AutoResetEvent _are1 = new(true);
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
        
        for (int i = 0; i < n; i++) {
            _are2.WaitOne();
            // printBar() outputs "bar". Do not change or remove this line.
            printBar();
            _are1.Set();
        }
    }
}
namespace MyOwnTests.LeetCode.Concurrency._1115.PrintFooBarAlternately;

// Solution with SpinWait (I don't know good is it or not)
// Sometimes SpinWait.Count = 22+
// so it will cause performance issues
// and will call Thread.Sleep method
// P.S. You can use SpinWait.SpinUntil method
// instead of SpinWait.SpinOnce


// Remember to remove " : ISolution" from code snippet
using System.Threading;

public class FooBarWithSpinWait(int n) : IFooBar
{
    private bool _isFoo = true;

    public void Foo(Action printFoo)
    {
        var sw = new SpinWait();
        for (int i = 0; i < n; i++)
        {
            while (!_isFoo)
            {
                sw.SpinOnce();
            }

            // printFoo() outputs "foo". Do not change or remove this line.
            printFoo();
            _isFoo = false;
            sw.Reset();
        }
    }

    public void Bar(Action printBar)
    {
        var sw = new SpinWait();
        for (int i = 0; i < n; i++)
        {
            while (_isFoo)
            {
                sw.SpinOnce();
            }

            // printBar() outputs "bar". Do not change or remove this line.
            printBar();
            _isFoo = true;
            sw.Reset();
        }
    }
}
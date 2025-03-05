namespace MyOwnTests.LeetCode.Concurrency._1114.PrintInOrder;

using System.Threading;

public class PrintInOrderWithARE
{
    private readonly AutoResetEvent _are2 = new(false);
    private readonly AutoResetEvent _are3 = new(false);

    public void First(Action printFirst)
    {
        // printFirst() outputs "first". Do not change or remove this line.
        printFirst();
        _are2.Set();
    }

    public void Second(Action printSecond)
    {
        _are2.WaitOne();
        // printSecond() outputs "second". Do not change or remove this line.
        printSecond();
        _are3.Set();
    }

    public void Third(Action printThird)
    {
        _are3.WaitOne();
        // printThird() outputs "third". Do not change or remove this line.
        printThird();
    }
}
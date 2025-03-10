namespace MyOwnTests.LeetCode.Concurrency._1116.PrintZeroEvenOdd;

using System.Threading;

public class ZeroEvenOddWithARE(int n) {
    private readonly AutoResetEvent _areZero = new(false);
    private readonly AutoResetEvent _areEven = new(false);
    private readonly AutoResetEvent _areOdd = new(false);

    // printNumber(x) outputs "x", where x is an integer.
    public void Zero(Action<int> printNumber) {
        for (int i=0;i<n;i++)
        {
            printNumber(0);
            WaitHandle.SignalAndWait(i % 2 == 0 ? _areOdd : _areEven, _areZero);
        }
    }

    public void Odd(Action<int> printNumber) {
        for (int i=1;i<=n;i+=2){
            _areOdd.WaitOne();
            printNumber(i);
            _areZero.Set();
        }
    }

    public void Even(Action<int> printNumber) {
        for (int i=2;i<=n;i+=2){
            _areEven.WaitOne();
            printNumber(i);
            _areZero.Set();
        }
    }
}
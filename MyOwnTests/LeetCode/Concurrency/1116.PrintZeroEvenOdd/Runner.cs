namespace MyOwnTests.LeetCode.Concurrency._1116.PrintZeroEvenOdd;

public static class Runner
{
    private static void PrintNumber(int number)
    {
        Console.Write(number);
    }
    
    // Runner for Solutions
    public static void Run(int n)
    { 
        var zeroEvenOdd = new ZeroEvenOddWithARE(n);
        Parallel.Invoke(
            () => zeroEvenOdd.Zero(PrintNumber),
            () => zeroEvenOdd.Even(PrintNumber),
            () => zeroEvenOdd.Odd(PrintNumber)
        );
    }
}
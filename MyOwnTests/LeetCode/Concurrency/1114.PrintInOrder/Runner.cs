using MyOwnTests.LeetCode.Concurrency._1115.PrintFooBarAlternately;

namespace MyOwnTests.LeetCode.Concurrency._1114.PrintInOrder;

public static class Runner
{
    // I changed slightly output of PrintFoo and PrintBar methods
    private static void First()
    {
        Console.Write("First");
    }
    
    private static void Second()
    {
        Console.Write("Second");
    }
    
    private static void Third()
    {
        Console.WriteLine("Third");
    }
    
    
    // Runner for Solutions
    public static Task RunAsync()
    {
        var printInOrder = new PrintInOrderWithARE();
        
        var t3 = Task.Run(() => printInOrder.Third(Third));
        Thread.Sleep(1000);
        var t1 = Task.Run(() => printInOrder.First(First));
        Thread.Sleep(1000);
        var t2 = Task.Run(() => printInOrder.Second(Second));
        
        return Task.WhenAll(t1, t2);
    }
}
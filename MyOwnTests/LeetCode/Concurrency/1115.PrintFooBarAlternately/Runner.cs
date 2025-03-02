namespace MyOwnTests.LeetCode.Concurrency._1115.PrintFooBarAlternately;

public static class Runner
{
    // I changed slightly output of PrintFoo and PrintBar methods
    private static void PrintFoo()
    {
        Console.Write("Foo");
    }
    
    private static void PrintBar()
    {
        Console.WriteLine("Bar");
    }
    
    // Runner for Solutions
    public static Task RunAsync(IFooBar fooBar)
    {
        var t1 = Task.Run(() => fooBar.Foo(PrintFoo));
        var t2 = Task.Run(() => fooBar.Bar(PrintBar));
        
        return Task.WhenAll(t1, t2);
    }
}
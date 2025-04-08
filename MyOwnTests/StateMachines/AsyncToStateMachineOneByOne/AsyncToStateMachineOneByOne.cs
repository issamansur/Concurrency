namespace MyOwnTests.StateMachines.AsyncToStateMachineOneByOne;

public static class AsyncToStateMachineOneByOne
{
    public static async Task<int> DoAsync1()
    {
        Console.WriteLine("DoAsync1 started");
        int n = await DoAsync2();
        Console.WriteLine("DoAsync1 finished");
        return n;
    }
    
    public static async Task<int> DoAsync2()
    {
        Console.WriteLine(" DoAsync2 started");
        int n = await DoAsync3();
        Console.WriteLine(" DoAsync2 finished");
        return n;
    }
    
    public static async Task<int> DoAsync3()
    {
        Console.WriteLine("  DoAsync3 started");
        await Task.Delay(1000);
        Console.WriteLine("  DoAsync3 finished");
        return 42;
    }
}
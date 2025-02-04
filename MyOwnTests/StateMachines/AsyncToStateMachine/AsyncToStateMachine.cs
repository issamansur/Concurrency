namespace MyOwnTests.StateMachines.AsyncToStateMachine;

public static class AsyncToStateMachine
{
    // Асинхронная функция с асинхронным ожиданием
    public static async Task DoAsyncWithAwait()
    {
        Console.WriteLine("Before async 1");
        
        await Task.Delay(0);

        Console.WriteLine("Between async 1 & 2");
        
        await Task.Delay(0);

        Console.WriteLine("After async 2");
    }
}
namespace MyOwnTests.StateMachines.AsyncToStateMachineWithConfigureAwait;

public static class AsyncToStateMachineWithConfigureAwait
{
    // Асинхронная функция без фактической асинхронности
    public static async Task DoAsyncWithoutAwait()
    {
        Console.WriteLine("Before sync");
        
        Thread.Sleep(5000);

        Console.WriteLine("After sync");
    }
}
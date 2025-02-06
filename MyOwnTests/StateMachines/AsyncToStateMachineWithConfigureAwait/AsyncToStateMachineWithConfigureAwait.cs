namespace MyOwnTests.StateMachines.AsyncToStateMachineWithConfigureAwait;

public static class AsyncToStateMachineWithConfigureAwait
{
    // Асинхронная функция без фактической асинхронности
    public static async Task DoAsyncWithConfigureAwait()
    {
        Console.WriteLine("Before sync");

        await Task.Delay(1000).ConfigureAwait(false);

        Console.WriteLine("After sync");
    }
}
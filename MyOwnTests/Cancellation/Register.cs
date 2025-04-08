namespace MyOwnTests.Cancellation;

public static class Register
{
    private static void DoOnCancel()
    {
        Console.WriteLine("Task was cancelled");
        throw new OperationCanceledException();
    }
    
    // When we cancel the token, all registered callbacks will be called
    // simultaneously, and we will get an AggregateException
    public static async Task RegisterAndCancelFalse()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        
        token.Register(DoOnCancel);
        token.Register(DoOnCancel);
        token.Register(DoOnCancel);
        
        await Task.Delay(1000, CancellationToken.None);
        
        // Or u can use overload with same effect
        // await cts.Cancel();
        // await cts.Cancel(false);
        await cts.CancelAsync();
    }

    // When we cancel the token, all registered callbacks will be called
    // one by one, and we will get first exception
    public static async Task RegisterAndCancelTrue()
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        token.Register(DoOnCancel);
        token.Register(DoOnCancel);
        token.Register(DoOnCancel);
        
        await Task.Delay(1000, CancellationToken.None);
        
        cts.Cancel(true);
    }
}
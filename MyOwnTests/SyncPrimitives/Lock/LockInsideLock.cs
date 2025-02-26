namespace MyOwnTests.SyncPrimitives.Lock;

public static class LockInsideLock
{
    private static readonly object _lockObject = new();

    // Всё работает, потому что мы используем один и тот же поток
    public static void LockExternal()
    {
        lock (_lockObject)
        {
            Console.WriteLine("LockExternal");
            LockInternal();
        }
    }
    
    public static void LockInternal()
    {
        lock (_lockObject)
        {
            Console.WriteLine("LockInternal");
        }
    }

    // Не работает, потому что в LockInternalAsync мы ждем завершения асинхронной операции
    // поток освобождается и другой поток пытается войти в LockInternalAsync
    public async static Task LockExternalAsync()
    {
        lock (_lockObject)
        {
            Console.WriteLine("LockExternalAsync");
            LockInternalAsync().Wait();
        }
    }

    public async static Task LockInternalAsync()
    {
        await Task.Delay(1);

        lock (_lockObject)
        {
            Console.WriteLine("LockInternalAsync");
        }
    }
}
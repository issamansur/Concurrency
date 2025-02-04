namespace MyOwnTests.SyncPrimitives.Lock;

// Используем монитор вместо Lock, чтобы увидеть исключения после await
// Можем увидеть, почему использование блокируется на уровне анализатора и компилятора
// (После await никто не дает гарантии, что вернется тот же поток, а значит и в лок поток войти не сможет)
public class AwaitInsideLock
{
    private readonly object _lockObject = new object();

    public async Task DoWorkAsync()
    {
        bool lockTaken = false;
        try
        {
            Console.WriteLine("State before lock 1 " + Environment.CurrentManagedThreadId);
            Monitor.Enter(_lockObject, ref lockTaken);
            Console.WriteLine("State after lock 1 " + Environment.CurrentManagedThreadId);
            Console.WriteLine("Начало работы внутри Monitor");

            Console.WriteLine("State before await 1 " + Environment.CurrentManagedThreadId);
            // Вставляем асинхронную операцию ИЛИ просто блокируем поток на время, чтобы увидеть разницу
            // await Task.Delay(5000).ConfigureAwait(true);
            // Thread.Sleep(5000);

            Console.WriteLine("State after await 1 " + Environment.CurrentManagedThreadId);
            Console.WriteLine("Конец работы внутри Monitor");
        }
        finally
        {
            if (lockTaken)
            {
                Console.WriteLine("State before unlock 1 " + Environment.CurrentManagedThreadId);
                Monitor.Exit(_lockObject);
                Console.WriteLine("State after unlock 1 " + Environment.CurrentManagedThreadId);
            }
        }
    }

    public async Task AnotherMethodAsync()
    {
        bool lockTaken = false;
        try
        {
            Console.WriteLine("State before lock 2 " + Environment.CurrentManagedThreadId);
            Monitor.Enter(_lockObject, ref lockTaken);
            Console.WriteLine("State after lock 2 " + Environment.CurrentManagedThreadId);
            Console.WriteLine("AnotherMethodAsync вошёл в Monitor");
            // Здесь можно добавить дополнительные операции
        }
        finally
        {
            if (lockTaken)
            {
                Console.WriteLine("State before unlock 2 " + Environment.CurrentManagedThreadId);
                Monitor.Exit(_lockObject);
                Console.WriteLine("State after unlock 2 " + Environment.CurrentManagedThreadId);
            }
        }
    }
}

public static class WhyLockCannotContainsAwaitStarter
{
    public static async Task Start()
    {
        var example = new AwaitInsideLock();

        // Запускаем два метода одновременно
        var task1 = example.DoWorkAsync();
        var task2 = example.AnotherMethodAsync();

        await Task.WhenAll(task1, task2);
    }
}
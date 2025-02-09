namespace MyOwnTests.MyQuestions;

// xml-documentation? Нет, не слышал.

/*
Вопрос:
Сколько времени займет выполнение методов в каждом из
следующих случаев и почему?
P.S. Если тяжело, то расскомментируйте строки с выводом
и продебажьте.
*/
public static class HowMuchTimeQuestion
{
    #region Вспомогательные функции
    private static void SleepSync()
    {
        Thread.Sleep(5000);
        // Console.WriteLine("Sync sleep delay!");
    }
    private static void DelaySync()
    {
        Task.Delay(TimeSpan.FromSeconds(5));
        // Console.WriteLine("Delay sync done!");
    }

    private static async Task DelayAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(5));
        // Console.WriteLine("Async delay done!");
    }
    #endregion
    
    // Легко
    public static async Task Question1()
    {
        await DelayAsync();
        await DelayAsync();
        await DelayAsync();
    } // 15 секунд
    
    public static Task Question2()
    {
        DelayAsync();
        DelayAsync();
        DelayAsync();

        return Task.CompletedTask;
    } // 0 секунд

    public static Task Question3()
    {
        SleepSync();
        SleepSync();
        SleepSync();

        return Task.CompletedTask;
    } // 15 секунд
    
    // Средне
    public static Task Question4()
    {
        DelaySync();
        DelaySync();
        DelaySync();

        return Task.CompletedTask;
    } // 0 секунд
    
    public static Task Question5()
    {
        var t1 = DelayAsync();
        var t2 = DelayAsync();
        var t3 = DelayAsync();
        
        Task.WaitAll(t1, t2, t3);

        return Task.CompletedTask;
    } // 5 секунд
    
    public static async Task Question6()
    {
        var t1 = DelayAsync();
        var t2 = DelayAsync();
        var t3 = DelayAsync();
        
        await Task.WhenAll(t1, t2, t3);
    } // 5 секунд

    // Сложно
    public static async Task Question7()
    {
        var numbers = new[] { 1, 2, 3 };
        
        var tasks = numbers.Select(async n =>
        {
            await DelayAsync();
            Console.WriteLine(n);
        });

        foreach (var task in tasks)
        {
            await task;
        }
    } // 15 секунд

    public static async Task Question8()
    {
        var numbers = new[] { 1, 2, 3 };
        
        var tasks = numbers.Select(async n =>
        {
            await DelayAsync();
            Console.WriteLine(n);
        }).ToArray();

        foreach (var task in tasks)
        {
            await task;
        }
    } // 5 секунд

    public static Task Question9()
    {
        Parallel.Invoke(
            async () => await DelayAsync(),
            async () => await DelayAsync(),
            async () => await DelayAsync()
        );
        
        return Task.CompletedTask;
    } // 0 cекунд

    public static Task Question10()
    {
        Parallel.Invoke(
            DelaySync,
            DelaySync,
            DelaySync
        );
        
        return Task.CompletedTask;
    } // 0 cекунд

    public static Task Question11()
    {
        Parallel.Invoke(
            SleepSync,
            SleepSync,
            SleepSync
        );

        return Task.CompletedTask;
    } // 5 cекунд
}
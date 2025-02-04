namespace MyOwnTests.Parallel;

public static class HowMuchThreads
{
    private static void DoSync()
    {
        // Задержка, чтобы задачи не могли брать уже использованные потоки
        Thread.Sleep(2000);
    }
    
    // Запускает метод и считает кол-во потоков вначале и в конце
    public static void InvokeAndCount()
    {
        // If Local and runtime - 2+, else - 0
        Console.WriteLine(ThreadPool.ThreadCount);

        System.Threading.Tasks.Parallel.Invoke(
            DoSync,
            DoSync,
            DoSync,
            DoSync,
            DoSync,
            DoSync,
            DoSync,
            DoSync
        );

        // If Debug and runtime - 8+, else - 8
        Console.WriteLine(ThreadPool.ThreadCount);
    }
    
    // Запускает метод и считает кол-во потоков вначале и в конце
    public static void ForAndCount()
    {
        // If Local and runtime - 2+, else - 0
        Console.WriteLine(ThreadPool.ThreadCount);

        System.Threading.Tasks.Parallel.For(0, 8, _ => DoSync());

        // 12+
        Console.WriteLine(ThreadPool.ThreadCount);
    }
}
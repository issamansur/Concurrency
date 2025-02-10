namespace ConcurrencyInCSharp_StephenCleary.Chapter_02_BasicsOfAsync;

/*
Задача:
Требуется отреагировать на прогресс выполнения операции.
*/
public static class Part_03_TaskProgress
{
    /*
    Используйте типы IProgress<T> и Progress<T>.
    Ваш async-метод должен получать аргумент IProgress<T>; 
    здесь T — тип прогресса, о котором вы хотите сообщать
     */
    public static async Task MyMethodAsync(IProgress<int>? progress = null)
    {
        bool done = false;
        int percentComplete = 0;
        while (!done)
        {
            percentComplete += 1;
            if (percentComplete == 100)
            {
                done = true;
            }

            progress?.Report(percentComplete);

            // P.S. Добавляю задержку, чтобы 
            // пул потоков успел обработать событие
            Thread.Sleep(1);
        };
    }
    
    // Пример использования в вызывающем коде:
    public static async Task CallMyMethodAsync()
    {
        var progress = new Progress<int>();
        progress.ProgressChanged += (sender, args) =>
        {
            //...
            Console.WriteLine($"Current progress: {args}");
        };
        await MyMethodAsync(progress);
    }
}
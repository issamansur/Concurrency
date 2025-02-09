namespace ConcurrencyInCSharp_StephenCleary.Chapter_02_BasicsOfAsync;

/*
Задача:
 У вас есть несколько задач, и нужно подождать, пока они все закончатся.
*/
public static class Part_04_WaitingGroupOfTasks
{
    /*
    Фреймворк предоставляет для этой цели метод Task.WhenAll. Метод
    получает несколько задач и возвращает задачу, которая завершается
    при завершении всех указанных задач.
    */
    public static async Task DoTasks()
    {
        Task task1 = Task.Delay(TimeSpan.FromSeconds(5));
        Task task2 = Task.Delay(TimeSpan.FromSeconds(5));
        Task task3 = Task.Delay(TimeSpan.FromSeconds(5));

        await Task.WhenAll(task1, task2, task3);

        Console.WriteLine("All done!");
    }

    /*
    Если все задачи имеют одинаковый тип результата и все завершаются
    успешно, то задача Task.WhenAll возвращает массив, содержащий
    результаты всех задач.
    */
    public static async Task DoTasksWithSameResultTypes()
    {
        Task<int> task1 = Task.FromResult(3);
        Task<int> task2 = Task.FromResult(5);
        Task<int> task3 = Task.FromResult(7);

        // "results" содержит { 3, 5, 7 }
        int[] results = await Task.WhenAll(task1, task2, task3);

        Console.WriteLine(string.Join(",", results));
    }

    /*
    Есть перегруженная версия Task.WhenAll, которая получает
    IEnumerable, но я не рекомендую ее использовать. Код получается
    более понятным, когда я явно «материализую» последовательность.
    */
    public static async Task<string> DownloadAllAsync(
        HttpClient client,
        IEnumerable<string> urls
    )
    {
        // Определить действие, выполняемое для каждого URL.
        var downloads = urls.Select(url => client.GetStringAsync(url));
        // Обратите внимание: задачи еще не запущены,
        //  потому что последовательность не была обработана.

        // Запустить загрузку для всех URL одновременно.
        Task<string>[] downloadTasks = downloads.ToArray();
        // Все задачи запущены.

        // Асинхронно ожидать завершения всех загрузок.
        string[] htmlPages = await Task.WhenAll(downloadTasks);
        // Все задачи завершены.

        return string.Concat(htmlPages);
    }

    /*
    Если какие-либо задачи выдают исключения, то Task.WhenAll
    сообщает об отказе своей возвращенной задачи с этим
    исключением. Если сразу несколько задач выдают исключение,
    то все эти исключения помещаются в задачу Task,
    возвращаемую Task.WhenAll. Тем не менее при ожидании
    этой задачи будет выдано только одно из них.
    */
    private static async Task ThrowNotImplementedExceptionAsync()
    {
        throw new NotImplementedException();
    }

    private static async Task ThrowInvalidOperationExceptionAsync()
    {
        throw new InvalidOperationException();
    }

    public static async Task ObserveOneExceptionAsync()
    {
        var task1 = ThrowNotImplementedExceptionAsync();
        var task2 = ThrowInvalidOperationExceptionAsync();
        try
        {
            await Task.WhenAll(task1, task2);
        }
        catch (Exception ex)
        {
            // "ex"  либо NotImplementedException, либо InvalidOperationException.
            // ...
            Console.WriteLine(ex.GetType());
        }
    }

    /*
    Если нужно каждое конкретное исключение, проверьте
    свойство Exception задачи Task, возвращаемой
    Task.WhenAll.
    */
    public static async Task ObserveAllExceptionsAsync()
    {
        var task1 = ThrowNotImplementedExceptionAsync();
        var task2 = ThrowInvalidOperationExceptionAsync();

        Task allTasks = Task.WhenAll(task1, task2);
        try
        {
            await allTasks;
        }
        catch (Exception ex)
        {
            // Здесь будет InvalidOperationException / NotImplementedException.
            Console.WriteLine(ex.GetType());
            
            // Здесь получаем все исключения.
            if (allTasks.Exception != null)
            {
                foreach (var exception in allTasks.Exception.Flatten().InnerExceptions)
                {
                    Console.WriteLine(exception.GetType());
                }
            }
        }
    }
}
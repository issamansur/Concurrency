using System.Threading.Tasks.Dataflow;

namespace MyOwnTests.MyQuestions;

// xml-documentation? Нет, не слышал.

/*
Вопрос:
Что за проблемы в данных примерах? 
Как их можно исправить?
*/
public static class WhatIsTheProblem
{
    // Пример 1. Вызов асинхронной функции из синхронной
    // Не мой, но достаточно показательный
    private static async Task WaitAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
    }

    public static void Example1()
    {
        Task task = WaitAsync();
        task.Wait();
    }
    // Подсказка: WindowsForms, ASP.NET, WPF
    // Ответ: один из уровней в Geometry Dash


    // Пример 2. Функция, которая должна вернуть список чисел
    public static async Task<IEnumerable<int>> Example2()
    {
        var numbers = new List<int>();

        var tasks = Enumerable.Range(1, 100)
            .Select(async i =>
            {
                await Task.Delay(10);
                numbers.Add(i);
            });

        var startedTasks = tasks.ToList(); 
        
        await Task.WhenAll(startedTasks);

        return numbers;
    }
    // Подсказка: несогласованность
    // Ответ: Непотокобезопасное изменение коллекции
    // Или даже изменение непотокобезопасной коллекции
    
    
    // Пример 3. Параллельная обработка асинхронных задач
    public static void Example3()
    {
        var numbers = new[] {1, 2, 3, 4, 5};
        
        Parallel.ForEach(numbers, async number =>
        {
            await Task.Delay(1000);
            Console.WriteLine($"Number: {number}");
        });
        
        Console.WriteLine("Parallel.ForEach finished!");
    }
    // Подсказка: какие ещё версии Parallel.ForEach есть?
    // Ответ: Parallel.ForEach не поддерживает асинхронные
    // делегаты, используем Parallel.ForEachAsync

    // Пример 4. Обработка исключений в асинхронных задачах
    // На самом деле, это не плохой пример, но всё равно
    // есть проблема
    private static async Task ThrowAfterDelay(int seconds)
    {
        await Task.Delay(TimeSpan.FromSeconds(seconds));
        throw new InvalidOperationException($"Boom after {seconds} sec!");
    }

    public static async Task Example4()
    {
        try
        {
            var tasks = new[]
            {
                ThrowAfterDelay(1),
                ThrowAfterDelay(2),
                ThrowAfterDelay(3)
            };

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Caught exception: " + ex.Message);
            if (ex is AggregateException { InnerExceptions.Count: > 1 } agg)
            {
                Console.WriteLine($"There are {agg.InnerExceptions.Count} inner exceptions!");
            }
        }
    }
    // Подсказка: Когда именно будет выброшено исключение?
    // Ответ: Необходимо получить таск task.WhenAll
    // и получить исключение из него


    // Пример 5. Запуск асинхронных задач в цикле
    public static async Task Example5()
    {
        var tasks = new List<Task>();
        
        for (int i = 0; i < 3; i++)
        {
            tasks.Add(Task.Run(async () =>
            {
                // Эмуляция асинхронной работы
                await Task.Delay(1000);
                Console.WriteLine($"Value: {i}");
            }));
        }

        // Вопрос: что выведется?
        // Вопрос: сколько времени займет выполнение?
        await Task.WhenAll(tasks);
    }
    // Подсказка: замыкание, ну и асинхронность
    // Ответ: 3, 3, 3; 1 секунда
    // Подробнее:
    // в C# после 5-й версии переменная цикла
    // захватывается по отдельности и на момент, когда
    // задачи реально «дойдут» до Console.WriteLine,
    // i уже успевает стать равным 3 (выход из цикла).
    // P.S.
    // Try foreach
    
    // Пример 6. Параллельное выполнение задач через await
    private static async Task DoAsync()
    {
        Thread.Sleep(1000);
        Console.WriteLine("3");
        await Task.Delay(1000);
        Console.WriteLine("4");
        Thread.Sleep(1000);
        Console.WriteLine("5");
    }

    public static async Task Example6()
    {
        var t = DoAsync();
        Console.WriteLine("1");
        Thread.Sleep(1500);
        Console.WriteLine("2");
        await t;
    }
    // Подсказка: что выведется на консоль? 
    // Когда именно DoAsync уйдёт в другой поток?
    // Ответ: 3, 1, 4, 2, 5
    
    // Пример 7. Обработка сети через Dataflow
    // Пример из книги (но некорректный). Почему?
    public static async Task Example7()
    {
        var block = new TransformBlock<int, int>(item =>
        {
            if (item == 1)
                throw new InvalidOperationException("Blech.");

            Console.WriteLine($"Processing {item}");

            return item * 2;
        });
        
        try
        {
            block.Post(1);
            block.Post(2);
            
            await block.Completion;
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Caught InvalidOperationException");
        }
    }
    // Подсказка: Когда таска завершится?
    // Ответ:
    // 1. Необходимо сообщить о завершении блока
    // через block.Complete()
    // 2. Необходимо прочитать данные из блока
    // до ожидания завершения блока
    // P.S.
    // Данная обработка вполне легальна в продакшене
    // (после исправлений, разумеется). Однако советую
    // рассмотреть другие варианты, чтобы блоки обработки
    // не входили в состояние отказа.
    // Например: "Рельсовое" программирование
    
    
    // Пример 8. Синхронное ожидание в асинхронной функции.
    // Честно говоря, могу только пожелать удачи
    public static async Task Example8()
    {
        // Indicates the task has been started and is ready.
        var taskReady = new TaskCompletionSource<object>();

        // Start the task, running on a thread pool thread.
        var task = Task.Run(() =>
        {
            // Spend a bit of time getting ready.
            Thread.Sleep(100);
        
            // Let the Test method know we've been started and are ready.
            taskReady.SetResult(5);

            // Spend a bit more time doing nothing in particular.
            Thread.Sleep(100);
        });

        // Wait for the task to be started and ready.
        await taskReady.Task;

        // Block until the task is completed.
        task.Wait();
    }
    // Ответ: https://blog.stephencleary.com/2012/12/dont-block-in-asynchronous-code.html
    // Краткий ответ: Используйте TaskCreationOptions.RunContinuationsAsynchronously
    
    // Пример 9. Использование Intersect для счётчика
    private static int Example9_Counter = 0;
    static int GetValue()
    {
        // Атомарное увеличение счётчика
        Interlocked.Increment(ref Example9_Counter);
        Console.WriteLine($"Counter: {Example9_Counter}");
        return Example9_Counter;
    }
    public static async Task Example9()
    {
        // Имитация множественных запросов
        var tasks = Enumerable
            .Range(0, 100)
            .Select(_ => Task.Run(GetValue));
        
        var executedTasks = tasks.ToList();
        
        int[] results = await Task.WhenAll(executedTasks);

        var set = new HashSet<int>(results);
        var count = set.Count;
        
        Console.WriteLine(count);
    }
    // Подсказка: что выведется на консоль?
    // Поможет ли Volatile?
    // Что именно делает Intersect?
    // Ответ: про порядок думаю, не стоит говорить - CW не безопасен
    // Volatile не поможет, так как проблема не в переменной.
    // Intersect помогает в данном примере, однако для
    // вывода и возврата используем возвращаемое значение Intersect
}
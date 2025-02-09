using Nito.AsyncEx;

namespace ConcurrencyInCSharp_StephenCleary.Chapter_02_BasicsOfAsync;

/*
Задача:
Имеется коллекция задач, которые будут использоваться с await;
требуется организовать обработку каждой задачи после ее
завершения. При этом обработка каждой задачи должна
происходить сразу же после завершения, без ожидания других
задач.

P.S. По мнению автора, WaitAny - это плохой способ решения
Я ещё в процессе изучения, so... ¯\_(ツ)_/¯
*/
public static class Part_06_HandleTasksAfterCompletion
{
    // Метод, который возвращает значение после указанной
    // задержки.
    private static async Task<int> DelayAndReturnAsync(int value)
    {
        await Task.Delay(TimeSpan.FromSeconds(value));
        return value;
    }

    /*
    В текущей версии метод выводит "2", "3" и "1".
    При этом метод должен выводить "1", "2" и "3".
    */
    public static async Task ProcessTasksAsync()
    {
        // Создать последовательность задач.
        Task<int> taskA = DelayAndReturnAsync(2);
        Task<int> taskB = DelayAndReturnAsync(3);
        Task<int> taskC = DelayAndReturnAsync(1);

        Task<int>[] tasks = new[] { taskA, taskB, taskC };

        // Ожидать каждую задачу по порядку.
        foreach (Task<int> task in tasks)
        {
            var result = await task;
            Console.WriteLine(result);
        }
    }

    /*
    Простейшее решение заключается в рефакторинге кода и
    введении высокоуровневого async-метода, который
    обеспечивает ожидание задачи и обработку ее результата.
    */

    // Метод-обёртка, который обрабатывает результат задачи
    // Тот самый "высокоуровневый async-метод"
    private static async Task AwaitAndProcessAsync(Task<int> task)
    {
        int result = await task;
        Console.WriteLine(result);
    }

    // Этот метод теперь выводит "1", "2" и "3".
    public static async Task ProcessTasksAsync2()
    {
        // Создать последовательность задач.
        Task<int> taskA = DelayAndReturnAsync(2);
        Task<int> taskB = DelayAndReturnAsync(3);
        Task<int> taskC = DelayAndReturnAsync(1);
        Task<int>[] tasks = new[] { taskA, taskB, taskC };
        IEnumerable<Task> taskQuery =
            from t in tasks select AwaitAndProcessAsync(t);
        Task[] processingTasks = taskQuery.ToArray();
        // Ожидать завершения всей обработки
        await Task.WhenAll(processingTasks);
    }

    /*
    Ну или так
    */
    // Этот метод теперь выводит "1", "2" и "3".
    public static async Task ProcessTasksAsync3()
    {
        // Создать последовательность задач.
        Task<int> taskA = DelayAndReturnAsync(2);
        Task<int> taskB = DelayAndReturnAsync(3);
        Task<int> taskC = DelayAndReturnAsync(1);
        Task<int>[] tasks = new[] { taskA, taskB, taskC };
        Task[] processingTasks = tasks.Select(async t =>
        {
            var result = await t;
            Console.WriteLine(result);
        }).ToArray();
        // Ожидать завершения всей обработки.
        await Task.WhenAll(processingTasks);
    }

    /*
    С таким методом расширения, как OrderByCompletion,
    изменения в исходной версии кода сводятся до минимума
    (NuGet-пакет Nito.AsyncEx)
    */
    // Этот метод теперь выводит "1", "2" и "3".
    public static async Task UseOrderByCompletionAsync()
    {
        // Создать последовательность задач.
        Task<int> taskA = DelayAndReturnAsync(2);
        Task<int> taskB = DelayAndReturnAsync(3);
        Task<int> taskC = DelayAndReturnAsync(1);
        Task<int>[] tasks = new[] { taskA, taskB, taskC };
        // Ожидать каждой задачи по мере выполнения.
        foreach (Task<int> task in tasks.OrderByCompletion())
        {
            int result = await task;
            Console.WriteLine(result);
        }
    }
}
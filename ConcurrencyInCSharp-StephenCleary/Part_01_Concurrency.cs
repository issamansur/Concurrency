using System.Reactive.Linq;

namespace ConcurrencyInCSharp_StephenCleary;

public static class Part_01_Concurrency
{
    // 1. Сохранение контекста
    // !!!
    // Должен быть SynchronizationContext
    // UI-context (к примеру)
    // !!!

    // 1.1
    // все синхронные части пытаются 
    // возобновить продолжение в исходном контексте:
    public static async Task DoSomethingAsync()
    {
        int value = 13;

        // Асинхронно ожидать 1 секунду.
        await Task.Delay(TimeSpan.FromSeconds(1));

        value *= 2;

        // Асинхронно ожидать 1 секунду.
        await Task.Delay(TimeSpan.FromSeconds(1));
        Console.WriteLine(value);
    }

    // 1.2
    // после приостановки await он возобновляет выполнение
    // в потоке из пула потоков:
    public static async Task DoSomethingAsyncWithConfigureAwait()
    {
        int value = 13;
        // Асинхронно ожидать 1 секунду.
        await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
        value *= 2;
        // Асинхронно ожидать 1 секунду.
        await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
        Console.WriteLine(value);
    }

    // 2. Обработка ошибок
    static async Task PossibleExceptionAsync()
    {
        throw new NotSupportedException();
    }

    // 2.1 Вариант первый
    public static async Task TrySomethingAsync()
    {
        try
        {
            await PossibleExceptionAsync();
        }
        catch (NotSupportedException ex)
        {
            Console.WriteLine("Ошибка без обёртки");
            Console.WriteLine(ex);
            throw;
        }
    }

    // 2.2 Вариант второй
    public static async Task TrySomething2Async()
    {
        // Исключение попадает в Task, а не выдается напрямую.
        Task task = PossibleExceptionAsync();
        try
        {
            // Исключение из Task exception будет выдано здесь, в точке await.
            await task;
        }
        catch (NotSupportedException ex)
        {
            Console.WriteLine("Ошибка без обёртки");
            Console.WriteLine(ex);
            throw;
        }
    }

    // 3. Дедлок
    // Если вы вызываете async-метод, следует 
    // (в конечном итоге) выполнить await для возвращаемой им задачи. 
    // Боритесь с искушением вызвать Task.Wait, Task<TResult>.Result
    // или GetAwaiter().GetResult():
    // это приведет к взаимоблокировке (deadlock)

    // !!!
    // Должен быть SynchronizationContext
    // UI-context (к примеру)
    // !!!
    static async Task WaitAsync()
    {
        // await сохранит текущий контекст ...
        await Task.Delay(TimeSpan.FromSeconds(1));
        // ... и попытается возобновить метод в этой точке с этим контекстом.
    }

    public static void Deadlock()
    {
        // Начать задержку.
        Task task = WaitAsync();
        // Синхронное блокирование с ожиданием завершения async-метода.
        task.Wait();
    }

    // 4. Использование Parallel.Invoke() для параллельной обработки
    // Вместо 15 секунд, будет 5+- секунд из-за параллельности
    public static void ProcessArray(double[] array)
    {
        Parallel.Invoke(
            () => ProcessPartialArray(array, 0, array.Length / 2),
            () => ProcessPartialArray(array, array.Length / 3, array.Length / 3 * 2),
            () => ProcessPartialArray(array, array.Length / 3 * 2, array.Length)
        );
    }

    static void ProcessPartialArray(double[] array, int begin, int end)
    {
        Thread.Sleep(5000);
    }

    // 5. Получение AggregateException
    public static void ThrowAndHandleAggregateException()
    {
        Console.WriteLine("Start");

        try
        {
            Parallel.Invoke(
                () => { },
                () => throw new Exception(),
                () => throw new Exception()
            );
        }
        catch (AggregateException ex)
        {
            Console.WriteLine("Received AggregateException");

            ex.Handle(exception =>
            {
                Console.WriteLine(exception);
                return true; // "обработано"
            });
        }

        Console.WriteLine("End");
    }

    // 6. Пример использование Reactive
    // !!!
    // Необходимо установить библиотеку System.Reactive
    // !!!

    // 6.1 Основной пример
    public static void ReactiveExample()
    {
        // Запуск счетчика по периодическому таймеру
        Observable.Interval(TimeSpan.FromSeconds(1))
            // Добавление временной метки для каждого события
            .Timestamp()
            // События фильтруются (только четные значения счетчика)
            .Where(x => x.Value % 2 == 0)
            // каждое поступившее значение временной метки записывается
            .Select(x => x.Timestamp)
            .Subscribe(x => Console.WriteLine(x));

        // Задержка для видимости результата
        Thread.Sleep(10000);
    }

    // 6.2 Эквивалентный пример
    /*
    Для типа нормально определять наблюдаемые потоки и делать их доступными
    в виде ресурса IObservable<TResult>. Затем другие типы могут
    подписываться на эти потоки или объединять их с другими операторами
    для создания другого наблюдаемого потока.
    */
    public static void ReactiveExampleAnotherVariant()
    {
        // Подобный запрос
        IObservable<DateTimeOffset> timestamps =
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Timestamp()
                .Where(x => x.Value % 2 == 0)
                .Select(x => x.Timestamp);

        // ПОдписка на IObservable
        timestamps.Subscribe(x => Console.WriteLine(x));

        // Задержка для видмости результата
        Thread.Sleep(10000);
    }

    // 6.3 Добавление обработчика ошибок
    public static void ReactExampleWithExceptionHandler()
    {
        // Подобный запрос
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Timestamp()
            .Where(x => x.Value % 2 == 0)
            .Select(x => x.Timestamp)
            // Подписка на ошибки
            .Subscribe(x => Console.WriteLine(x),
                ex => Console.WriteLine(ex));

        // Задержка для видмости результата
        Thread.Sleep(10000);
    }
}
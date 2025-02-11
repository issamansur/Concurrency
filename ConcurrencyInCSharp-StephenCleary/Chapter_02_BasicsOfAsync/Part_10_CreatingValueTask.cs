namespace ConcurrencyInCSharp_StephenCleary.Chapter_02_BasicsOfAsync;

/*
Задача:
Требуется создать метод, возвращающий ValueTask<T>.
*/
public static class Part_10_CreatingValueTask
{
    /*
    ValueTask<T> используется как возвращаемый тип в ситуациях,
    в которых обычно может быть возвращен синхронный результат,
    а асинхронное поведение встречается реже.

    Рассматривать следует только!!! после профилирования,
    показывающее, что это приведет к повышению быстродействия.
    */
    public static async ValueTask<int> MethodAsync()
    {
        await Task.Delay(100); // Асинхронная работа.
        return 13;
    }

    /*
    Нередко метод, возвращающий ValueTask<T>, способен
    немедленно вернуть значение; в таких случаях можно
    применить оптимизацию для этого сценария с использованием
    конструктора ValueTask<T>, а затем передавать управление
    медленному асинхронному методу только при необходимости
    */
    public static ValueTask<int> MethodAsyncOptimized()
    {
        if (CanBehaveSynchronously)
            return new ValueTask<int>(13);
        return new ValueTask<int>(SlowMethodAsync());
    }

    public static bool CanBehaveSynchronously { get; set; }

    private static Task<int> SlowMethodAsync()
    {
        Thread.Sleep(10000);
        return Task.FromResult(13);
    }

    /*
    В следующем примере показана реализация IAsyncDisposable,
    которая выполняет свою логику асинхронного освобождения
    однократно; при будущих вызовах метод DisposeAsync
    завершается успешно и синхронно:
    */
    public class MyAsyncDisposable : IAsyncDisposable
    {
        private Func<Task> _disposeLogic;

        public ValueTask DisposeAsync()
        {
            if (_disposeLogic == null)
                return default;
            // Примечание: этот простой пример не является потокобезопасным;
            //  если сразу несколько потоков вызовут DisposeAsync,
            //  логика может быть выполнена более одного раза.
            Func<Task> logic = _disposeLogic;
            _disposeLogic = null;
            return new ValueTask(logic());
        }
    }
}
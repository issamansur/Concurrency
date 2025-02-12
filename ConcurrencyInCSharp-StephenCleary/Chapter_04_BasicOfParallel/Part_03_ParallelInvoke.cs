namespace ConcurrencyInCSharp_StephenCleary.Chapter_04_BasicOfParallel;

/*
Задача:
Имеется набор методов, которые должны вызываться параллельно.
Эти методы (в основном) независимы друг от друга
*/
public static class Part_03_ParallelInvoke
{
    /*
    В следующем примере массив разбивается надвое, и две 
    половины обрабатываются независимо
    */
    public static void ProcessArray(double[] array)
    {
        Parallel.Invoke(
            () => ProcessPartialArray(array, 0, array.Length / 2),
            () => ProcessPartialArray(array, array.Length / 2, array.Length)
        );
    }
    
    private static void ProcessPartialArray(double[] array, int begin, int end)
    {
        // Обработка, интенсивно использующая процессор...
    }
    
    /*
    Методу Parallel.Invoke также можно передать массив 
    делегатов, если количество вызовов неизвестно до момента
    выполнения
    */
    public static void DoAction20Times(Action action)
    {
        Action[] actions = Enumerable
            .Repeat(action, 20)
            .ToArray();

        Parallel.Invoke(actions);
    }
    
    /*
    Parallel.Invoke поддерживает отмену, как и другие методы
    класса Parallel:
    */
    public static void DoAction20Times(Action action, CancellationToken token)
    {
        Action[] actions = Enumerable.Repeat(action, 20).ToArray();
        Parallel.Invoke(new ParallelOptions { CancellationToken = token }, actions);
    }
}
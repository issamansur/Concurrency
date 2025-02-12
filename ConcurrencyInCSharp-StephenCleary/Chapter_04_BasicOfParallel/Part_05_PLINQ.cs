namespace ConcurrencyInCSharp_StephenCleary.Chapter_04_BasicOfParallel;

/*
Задача:
Требуется выполнить параллельную обработку последовательности
данных, чтобы сгенерировать другую их последовательность или
обобщение этих данных.
*/
public static class Part_05_PLINQ
{
    /*
    Следующий простой пример просто умножает каждый 
    элемент последовательности на 2
    */
    public static IEnumerable<int> MultiplyBy2(IEnumerable<int> values)
    {
        return values
            .AsParallel()
            .Select(value => value * 2);
    }
    
    /*
    В следующем примере обработка ведется параллельно, 
    но с сохранением исходного порядка
    */
    public static IEnumerable<int> MultiplyBy2Ordered(
        IEnumerable<int> values
    )
    {
        return values
            .AsParallel()
            .AsOrdered()
            .Select(value => value * 2);
    }
    
    /*
    В следующем примере выполняется параллельное суммирование
    */
    public static int ParallelSum(
        IEnumerable<int> values
     )
    {
        return values
            .AsParallel()
            .Sum();
    }
}
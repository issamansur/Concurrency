namespace ConcurrencyInCSharp_StephenCleary.Chapter_04_BasicOfParallel;

/*
Задача:
Требуется агрегировать результаты при завершении параллельной
операции (примеры агрегирования — суммирование значений или
вычисление среднего).
*/
public static class Part_02_ParallelAggregate
{
    /*
    Пример параллельного суммирования
    */
    // Примечание: это не самая эффективная реализация.
    // Это всего лишь пример использования блокировки
    // для защиты совместного состояния.
    public static int ParallelSum(IEnumerable<int> values)
    {
        object mutex = new object();
        int result = 0;
        Parallel.ForEach(source: values,
            localInit: () => 0,
            body: (item, state, localValue) => localValue + item,
            localFinally: localValue =>
            {
                lock (mutex)
                    result += localValue;
            });
        return result;
    }
    
    /*
    В Parallel LINQ реализована более понятная поддержка 
    агрегирования, чем в классе Parallel
    */
    public static int ParallelSum2(IEnumerable<int> values)
    {
        return values.AsParallel().Sum();
    }
    
    /*
    В PLINQ также предусмотрена обобщенная поддержка 
    агрегирования с оператором Aggregate:
    */
    public static int ParallelSum3(IEnumerable<int> values)
    {
        return values.AsParallel().Aggregate(
            seed: 0,
            func: (sum, item) => sum + item
        );
    }
    
    
}
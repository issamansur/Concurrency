namespace ConcurrencyInCSharp_StephenCleary.Chapter_04_BasicOfParallel;

/*
Задача:
Имеется коллекция данных. Требуется выполнить одну и ту же
операцию с каждым элементом данных. Эта операция является
ограниченной по вычислениям и может занять некоторое время.
*/
public static class Part_01_ParallelDataHandling
{
    /*
    Тип Parallel содержит метод ForEach, разработанный
    специально для этой задачи.
    */
    public static void RotateMatrices(
        IEnumerable<Matrix> matrices,
        float degrees
    )
    {
        Parallel.ForEach(
            matrices,
            matrix => matrix.Rotate(degrees)
        );
    }

    /*
    Следующий пример обращает каждую матрицу, но при
    обнаружении недействительной матрицы цикл будет прерван

    P.S.
    Внимание: некоторые элементы могут быть в обработке,
    когда цикл прерывается. Поэтому они продолжат выполнение.
    Для примера - создайте 5 матриц с IsInvertible = false
    и 5 матриц с IsInvertible = true, затем увеличьте кол-во
    матриц с IsInvertible = false до 10. Затем до 15
    Внимание: Они должны быть первыми в списке
    */
    public static void InvertMatrices(
        IEnumerable<Matrix> matrices
    )
    {
        Parallel.ForEach(matrices, (matrix, state) =>
        {
            if (!matrix.IsInvertible)
                state.Stop();
            else
                matrix.Invert();
        });
    }

    /*
    Более распространенная ситуация встречается тогда, когда
    требуется отменить параллельный цикл. Это не то же, что
    остановка цикла; цикл останавливается изнутри и отменяется
    за своими пределами. Например, кнопка отмены может
    отменить CancellationTokenSource, отменяя параллельный
    цикл, как в следующем примере
    
    P.S. Забавно, но даже если все задачи успеют выполниться,
    но сам Parallel под капотом не успеет отработать - получим
    OperationCanceledException
    */
    public static void RotateMatrices(
        IEnumerable<Matrix> matrices, float degrees,
        CancellationToken token)
    {
        Parallel.ForEach(matrices,
            new ParallelOptions { CancellationToken = token },
            matrix => matrix.Rotate(degrees)
        );
    }
    
    /*
    Следует иметь в виду, что каждая параллельная задача 
    может выполняться в другом потоке, поэтому любое 
    совместное состояние должно быть защищено. Следующий 
    пример обращает каждую матрицу и подсчитывает количество
    матриц, которые обратить не удалось
    */

    // Примечание: это не самая эффективная реализация.
    // Это всего лишь пример использования блокировки
    // для защиты совместного состояния.
    public static int InvertMatricesAndCount(IEnumerable<Matrix> matrices)
    {
        object mutex = new object();
        int nonInvertibleCount = 0;
        Parallel.ForEach(matrices, matrix =>
        {
            if (matrix.IsInvertible)
            {
                matrix.Invert();
            }
            else
            {
                lock (mutex)
                {
                    ++nonInvertibleCount;
                }
            }
        });
        return nonInvertibleCount;
    }

    #region Вспомогательные типы

    public class Matrix
    {
        public Matrix(bool isInvertible = true)
        {
            IsInvertible = isInvertible;
        }

        public bool IsInvertible { get; set; }

        public void Rotate(float degrees)
        {
            // Заменим логику просто на долгое ожидание
            Thread.Sleep(5000);
            Console.WriteLine("Rotated");
        }

        public void Invert()
        {
            // Заменим логику просто на долгое ожидание
            Thread.Sleep(5000);
            Console.WriteLine("Inverted");
        }
    }

    #endregion
}
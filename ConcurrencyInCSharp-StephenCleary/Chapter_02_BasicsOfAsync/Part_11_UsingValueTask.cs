namespace ConcurrencyInCSharp_StephenCleary.Chapter_02_BasicsOfAsync;

/*
Задача:
Требуется использовать ValueTask<T>
*/
public static class Part_11_UsingValueTask
{
    /*
    Самый простой и прямолинейный способ потребления 
    ValueTask<T> или ValueTask основан на await. 
    В большинстве случаев это все, что вам необходимо сделать
    */
    private static async ValueTask<int> MethodAsync()
    {
        await Task.Delay(1000);
        return 13;
    }

    public static async Task ConsumingMethodAsync()
    {
        int value = await MethodAsync();
    }
    
    /*
    Также можно выполнить await после выполнения конкурентной
    операции, как в случае с Task<T>:
    */ 
    public static async Task ConsumingMethodAsync2()
    {
        ValueTask<int> valueTask = MethodAsync();
        // Другая параллельная работа.
        Console.WriteLine(123);
        int value = await valueTask;
    }
    
    /*
    Чтобы сделать что-то более сложное, преобразуйте 
    ValueTask<T> в Task<T> вызовом AsTask,
    т.к. Многократное ожидание Task<T> абсолютно безопасно.
    */
    public static async Task ConsumingMethodAsync3()
    {
        Task<int> task = MethodAsync().AsTask();
        // Другая параллельная работа.

        int value = await task;
        int anotherValue = await task;
    }
    
    /*
    Также возможны другие операции — например, асинхронное 
    ожидание завершения нескольких операций
    */
    public static async Task ConsumingMethodAsync4()
    {
        Task<int> task1 = MethodAsync().AsTask();
        Task<int> task2 = MethodAsync().AsTask();
        // Другая параллельная работа.
        
        int[] task = await Task.WhenAll(task1, task2);
    }
}
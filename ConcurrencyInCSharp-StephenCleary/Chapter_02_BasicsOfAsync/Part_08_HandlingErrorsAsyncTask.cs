namespace ConcurrencyInCSharp_StephenCleary.Chapter_02_BasicsOfAsync;

/*
Задача:
Обработка исключений является важнейшей частью любой
программной архитектуры. Спроектировать код для успешного
результата несложно, но структура кода не может считаться
правильной, если в ней не обрабатываются потенциальные ошибки.
К счастью, обработка исключений из методов async Task
реализуется прямолинейно.
*/
public static class Part_08_HandlingErrorsAsyncTask
{
    /*
    Исключения можно перехватывать простой конструкцией 
    try/catch, как вы бы сделали для синхронного кода
    */
    private static async Task ThrowExceptionAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        throw new InvalidOperationException("Test");
    }
    
    public static async Task TestAsync()
    {
        try
        {
            await ThrowExceptionAsync();
        }
        catch (InvalidOperationException)
        {
        }
    }
    
    /*
    Исключения, выданные из методов async Task, помещаются
    в возвращаемый объект Task. Они выдаются только при
    использовании await с возвращаемым объектом Task
    */
    public static async Task TestAsync2()
    {
        // Исключение выдается методом и помещается в задачу.
        Task task = ThrowExceptionAsync();
        try
        {
            // Здесь исключение будет выдано повторно.
            await task;
        }
        catch (InvalidOperationException)
        {
            // Здесь исключение правильно перехватывается.
        }
    }
}
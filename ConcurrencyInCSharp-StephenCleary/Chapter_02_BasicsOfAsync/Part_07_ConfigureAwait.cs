namespace ConcurrencyInCSharp_StephenCleary.Chapter_02_BasicsOfAsync;

/*
Задача:
Когда async-метод возобновляет работу после await, 
по умолчанию он продолжает выполнение в том же контексте. 
Это может создать проблемы с быстродействием, если 
контекстом был UI-контекст, а в UI-контексте возобновляет 
работу большое количество async-методов.
*/
public static class Part_07_ConfigureAwait
{
    public static async Task ResumeOnContextAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        // Этот метод возобновляется в том же контексте.
    }
    
    /*
    Чтобы избежать возобновления в контексте, используйте 
    await для результата ConfigureAwait и передайте false 
    в параметре continueOnCapturedContext
    */
    public static async Task ResumeWithoutContextAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
        // Этот метод теряет свой контекст при возобновлении.
    }
    
    // P.S. От меня
    // Если в одном месте вам контекст нужен, но дальше по
    // коду он не нужен, то можно использовать
    // ConfigureAwait(false) внутри всех вложенных методов.
    // Например:
    
    // Один из вложенных методов
    private static async Task DoSomethingAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
        // Этот метод теряет свой контекст при возобновлении.
    }
    
    private static async Task DoAsyncWithoutContext()
    {
        await DoSomethingAsync().ConfigureAwait(false);
        // Этот метод теряет свой контекст при возобновлении.
    }
    
    public static async Task DoAsyncWithContext()
    {
        await DoAsyncWithoutContext();
        // Этот метод возобновляется в том же контексте.
    }
}
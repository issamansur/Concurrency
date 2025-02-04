namespace ConcurrencyInCSharp_StephenCleary;

public class Part_02_BasicsOfAsync
{
    // 1. Обычная асинхронная задержка
    /*
    Требуется (асинхронно) приостановить выполнение программы 
    на некоторый период времени. Такая ситуация часто 
    встречается при модульном тестировании или реализации 
    задержки для повторного использования. Она также возникает
    при программировании простых тайм-аутов.
     */
    public static async Task<T> DelayResult<T>(T result, TimeSpan delay)
    {
        Console.WriteLine("Start of delay");
        await Task.Delay(delay);
        Console.WriteLine("End of delay");
        return result;
    }
    
    // 2. Экспоненциальная задержка
    /*
    — стратегия увеличения задержек между повторными
    попытками. Используется ее при работе с веб-службами,
    чтобы не перегружать сервер повторными попытками.
     */
    /*
    !!!
    В реальном коде рекомендуется применить более качественное 
    решение (например, использующее библиотеку Polly NuGet); код, 
    приведенный здесь, является всего лишь примером использования 
    Task.Delay.
    !!!
     */
    public static async Task<string> DownloadStringWithRetries(HttpClient client, string uri)
    {
        // Повторить попытку через 1 секунду, потом через 2 и через 4 секунды.
        TimeSpan nextDelay = TimeSpan.FromSeconds(1);
        for (int i = 0; i != 3; ++i)
        {
            try
            {
                return await client.GetStringAsync(uri);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            await Task.Delay(nextDelay);
            nextDelay = nextDelay + nextDelay;
        }
        // Попробовать в последний раз и разрешить распространение ошибки.
        return await client.GetStringAsync(uri);
    }
    
    // 3.1 "Мягкий" таймаут
    public static async Task<string?> DownloadStringWithTimeout(
        HttpClient client,
        string uri
    )
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        
        Task<string> downloadTask = client.GetStringAsync(uri);
        Task timeoutTask = Task.Delay(Timeout.InfiniteTimeSpan, cts.Token);

        Task completedTask = await Task.WhenAny(downloadTask, timeoutTask);
        if (completedTask == timeoutTask)
        {
            return null;
        }

        return await downloadTask;
    }
    
    // 3.2
    /*
    Task.Delay неплохо подходит для модульного 
    тестирования асинхронного кода или реализации логики 
    повторных попыток. Но если нужно реализовать тайм-аут, 
    лучшим кандидатом будет CancellationToken.
     */
    public static async Task<string?> DownloadStringWithTimeoutRight(
        HttpClient client,
        string uri
    )
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        
        Task<string> downloadTask = client.GetStringAsync(uri, cts.Token);
            
        return await downloadTask;
    }
}
namespace ConcurrencyInCSharp_StephenCleary.Chapter_02_BasicsOfAsync;

/*
Задача:
Есть несколько задач и требуется отреагировать на завершение
любой задачи из группы. Задача чаще всего встречается при
выполнении нескольких независимых попыток выполнения операции
в структуре «первому достается все». Например, можно запросить
биржевые котировки у нескольких веб-служб одновременно, но 
интересует вас только первый ответ.
*/
public static class Part_05_WaitingAnyTask
{
    /*
    Используйте метод Task.WhenAny. Метод Task.WhenAny
    получает последовательность задач и возвращает задачу,
    которая завершается при завершении любой из задач
    последовательности.
    */

    // Возвращает длину данных первого ответившего URL-адреса.
    public static async Task<int> FirstRespondingUrlAsync(
        HttpClient client,
        string urlA,
        string urlB
    )
    {
        // Запустить обе загрузки параллельно.
        Task<byte[]> downloadTaskA = client.GetByteArrayAsync(urlA);
        Task<byte[]> downloadTaskB = client.GetByteArrayAsync(urlB);

        // Ожидать завершения любой из этих задач.
        Task<byte[]> completedTask =
            await Task.WhenAny(downloadTaskA, downloadTaskB);

        // Вернуть длину данных, загруженных по этому URL-адресу.
        byte[] data = await completedTask;

        return data.Length;
    }
}
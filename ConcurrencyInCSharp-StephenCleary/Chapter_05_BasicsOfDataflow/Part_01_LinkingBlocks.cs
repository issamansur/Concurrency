using System.Collections;
using System.Threading.Tasks.Dataflow;

namespace ConcurrencyInCSharp_StephenCleary.Chapter_05_BasicsOfDataflow;

/*
Задача:
Требуется связать блоки Dataflow для создания сети.
*/
public static class Part_01_LinkingBlocks
{
    /*
    Метод расширения LinkTo предоставляет простой механизм 
    связывания блоков потока данных
    */
    public static async Task<IEnumerable<int>> LinkingBlocks(IEnumerable<int> startValues)
    {
        var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
        var subtractBlock = new TransformBlock<int, int>(item => item - 2);

        multiplyBlock.LinkTo(subtractBlock);

        foreach (var value in startValues)
        {
            await multiplyBlock.SendAsync(value); // Гарантированная отправка данных
        }

        multiplyBlock.Complete(); // Завершаем поток данных

        await multiplyBlock.Completion; // Дожидаемся, пока multiplyBlock полностью обработает данные

        subtractBlock.Complete(); // Завершаем поток данных

        var results = new List<int>();
        
        // Считываем данные из subtractBlock
        while (await subtractBlock.OutputAvailableAsync())
        {
            results.Add(await subtractBlock.ReceiveAsync());
        }
        
        await subtractBlock.Completion; // Дожидаемся, пока subtractBlock полностью обработает данные

        return results;
    }

    /*
    Чтобы распространять завершение (и ошибки), установите 
    параметр PropagateCompletion для связи
    */
    public static async Task<IEnumerable<int>> LinkingBlocksWithCompetion(IEnumerable<int> startValues)
    {
        var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
        var subtractBlock = new TransformBlock<int, int>(item => item - 2);

        // Теперь завершение propagateCompletion = true
        var options = new DataflowLinkOptions { PropagateCompletion = true };
        multiplyBlock.LinkTo(subtractBlock, options);

        foreach (var value in startValues)
        {
            await multiplyBlock.SendAsync(value); // Гарантированная отправка данных
        }

        multiplyBlock.Complete(); // Завершаем поток данных

        var results = new List<int>();
        
        // Считываем данные из subtractBlock
        while (await subtractBlock.OutputAvailableAsync())
        {
            results.Add(await subtractBlock.ReceiveAsync());
        }
        
        await subtractBlock.Completion; // Дожидаемся, пока subtractBlock полностью обработает данные

        return results;
    }
}
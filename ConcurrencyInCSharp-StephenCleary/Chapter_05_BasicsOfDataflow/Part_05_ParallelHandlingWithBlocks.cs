using System.Threading.Tasks.Dataflow;

namespace ConcurrencyInCSharp_StephenCleary.Chapter_05_BasicsOfDataflow;

public static class Part_05_ParallelHandlingWithBlocks
{
    /*
    Если один конкретный блок выполняет интенсивные 
    вычисления на процессоре, — вы можете дать команду 
    этому блоку работать параллельно с входными данными, 
    устанавливая параметр MaxDegreeOfParallelism.
     */
    // P.S. Этот метод не из книги, ибо там опять 2+2=4
    // Пожалуйста
    // Кстати, теперь очень хочется использовать
    // в проекте, где есть много вычислений
    public static async Task ParallelHandlingWithBlocks()
    {
        var buffer = new BufferBlock<int>();
        var transform = new TransformBlock<int, int>(async item =>
            {
                Console.WriteLine($"Processing {item}");
                await Task.Delay(1000);
                return item * 2;
            },
            new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
            }
        );

        var action = new ActionBlock<int>(item => { Console.WriteLine($"Output: {item}"); });

        buffer.LinkTo(transform, new DataflowLinkOptions { PropagateCompletion = true });
        transform.LinkTo(action, new DataflowLinkOptions { PropagateCompletion = true });

        for (int i = 0; i < 10; i++)
        {
            await buffer.SendAsync(i);
        }

        buffer.Complete();
        await action.Completion;
    }
}
using System.Threading.Tasks.Dataflow;

namespace ConcurrencyInCSharp_StephenCleary.Chapter_05_BasicsOfDataflow;

/*
Задача:
Найти способ реагировать на ошибки, которые могут происходить 
в сети потока данных.
*/
public static class Part_02_HandlingExceptions
{
    // !!!
    // P.S. Не знаю, почему в книге не было этого
    // фрагмента кода, но в случае успеха
    // блок не будет обработан, так как он не завершен
    // для всех примеров
    private static async Task MissingPart(TransformBlock<int, int> block)
    {
        block.Complete();
            
        // Читаем данные перед ожиданием Completion
        while (await block.OutputAvailableAsync())
        {
            Console.WriteLine($"Received: {await block.ReceiveAsync()}");
        }
    }
    
    /*
    В следующем коде блок не производит никаких выходных 
    данных; первое значение выдает исключение, а второе 
    просто теряется
    */
    public static async Task NotHandlingExceptions()
    {
        var block = new TransformBlock<int, int>(item =>
        {
            if (item == 1)
                throw new InvalidOperationException("Blech.");

            return item * 2;
        });

        // Выдаст исключение
        block.Post(1);
        // Просто потеряет значение
        block.Post(2);

        // !!!
        await MissingPart(block);
        // !!!
        
        await block.Completion;
    }

    /*
    Чтобы перехватывать исключения от блока потока данных,
    необходимо ожидать его свойства Completion.
    */
    public static async Task HandlingExceptions()
    {
        var block = new TransformBlock<int, int>(item =>
        {
            if (item == 1)
                throw new InvalidOperationException("Blech.");

            Console.WriteLine($"Processing {item}");

            return item * 2;
        });
        
        try
        {
            block.Post(1);
            block.Post(2);

            // !!!
            await MissingPart(block);
            // !!!
            
            await block.Completion;
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Caught InvalidOperationException");
        }
    }
    
    /*
    Следующий пример перехватывает исключение в конце 
    конвейера, поэтому он перехватит AggregateException, 
    если исключение было распространено из предыдущих 
    блоков:
    */
    public static async Task HandlingAggregateExceptions()
    {
        try
        {
            var multiplyBlock = new TransformBlock<int, int>(item =>
            {
                if (item == 1)
                    throw new InvalidOperationException("Blech.");
                return item * 2;
            });
            var subtractBlock = new TransformBlock<int, int>(item => item - 2);
            multiplyBlock.LinkTo(subtractBlock,
                new DataflowLinkOptions { PropagateCompletion = true });
            multiplyBlock.Post(2);
            
            // !!!
            multiplyBlock.Complete();
            while (await subtractBlock.OutputAvailableAsync())
            {
                Console.WriteLine($"Received: {await subtractBlock.ReceiveAsync()}");
            }
            // !!!
            
            await subtractBlock.Completion;
        }
        catch (AggregateException)
        {
            // P.S. Flatten() заюзать и обработать
            // А то я устал модифицировать код
            Console.WriteLine("Caught AggregateException");
        }
    }
}
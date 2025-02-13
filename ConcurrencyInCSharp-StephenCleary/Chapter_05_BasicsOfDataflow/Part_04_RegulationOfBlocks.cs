using System.Threading.Tasks.Dataflow;

namespace ConcurrencyInCSharp_StephenCleary.Chapter_05_BasicsOfDataflow;

/*
Задача:
Имеется сеть потока данных с ветвлением. Требуется 
организовать передачу данных с распределением нагрузки.
*/
public static class Part_04_RegulationOfBlocks
{
    /*
    При связи, когда один блок источник связан с двумя и 
    более блоками-приемниками, можно воспользоваться 
    регулировкой (throttling) блоков-приемников с 
    использованием параметра блока BoundedCapacity
    */
    public static async Task RegulationOfBlocks()
    {
        var sourceBlock = new BufferBlock<int>();
        var options = new DataflowBlockOptions
        {
            // P.S. Ставлю 5, ибо так нагляднее, а то 1...
            BoundedCapacity = 5, 
        };
        var targetBlockA = new BufferBlock<int>(options);
        var targetBlockB = new BufferBlock<int>(options);
        
        // !!!
        // P.S. Добавлю как обычно свой код, чтобы было
        // что-то работающее
        var linkOptions = new DataflowLinkOptions
        {
            PropagateCompletion = true
        };
        // !!!
        
        sourceBlock.LinkTo(targetBlockA, linkOptions);
        sourceBlock.LinkTo(targetBlockB, linkOptions);
        
        // !!!
        for (var i = 0; i < 10; i++)
        {
            sourceBlock.Post(i);
        }
        
        sourceBlock.Complete();
           
        await foreach (var item in targetBlockA.ReceiveAllAsync())
        {
            Console.WriteLine($"A: {item}");
        }

        await foreach (var item in targetBlockB.ReceiveAllAsync())
        {
            Console.WriteLine($"B: {item}");
        }
        
        await Task.WhenAll(targetBlockA.Completion, targetBlockB.Completion);
        // !!!
    }
}
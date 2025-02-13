using System.Threading.Tasks.Dataflow;

namespace ConcurrencyInCSharp_StephenCleary.Chapter_05_BasicsOfDataflow;

/*
Задача:
Имеется некоторая логика, которую требуется разместить в 
нестандартном блоке потока данных. Это позволит создавать
большие блоки, содержащие сложную логику.
*/
public static class Part_06_CreatingOwnBlocks
{
    /*
    Следующий код создает из двух блоков нестандартный 
    блок потока данных с распространением данных и завершения:
    */
    public static IPropagatorBlock<int, int> CreateMyCustomBlock()
    {
        var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
        var addBlock = new TransformBlock<int, int>(item => item + 2);
        var divideBlock = new TransformBlock<int, int>(item => item / 2);
        
        var flowCompletion = new DataflowLinkOptions
        {
            PropagateCompletion = true
        };
        
        multiplyBlock.LinkTo(addBlock, flowCompletion);
        addBlock.LinkTo(divideBlock, flowCompletion);
        
        return DataflowBlock.Encapsulate(multiplyBlock, divideBlock);
    }
    // P.S. Пример использования:
    // ага, да, разумеется, его нет
}
using System.Threading.Tasks.Dataflow;

namespace ConcurrencyInCSharp_StephenCleary.Chapter_05_BasicsOfDataflow;

/*
Задача
В процессе обработки необходимо динамически изменить 
структуру потока данных. Это нетипичный сценарий, с которым
вы вряд ли когда-либо столкнетесь в жизни, 
НО МЫ ЖЕ ТЕОРЕТИКИ!
*/
public static class Part_03_RemovingLinks
{
    /*
    При создании связи между блоками потока данных сохраните
    объект IDisposable, возвращенный методом LinkTo, и 
    уничтожьте его, когда потребуется разорвать связь между
    блоками:
    */
    public static async Task RemovingLinks()
    {
        var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
        var subtractBlock = new TransformBlock<int, int>(item => item - 2);
        IDisposable link = multiplyBlock.LinkTo(subtractBlock);
        multiplyBlock.Post(1);
        multiplyBlock.Post(2);

        // Удаление связей между блоками.
        // Данные, отправленные выше, могут быть уже переданы
        // или не переданы по связи. В реальном коде стоит
        // рассмотреть возможность блока using вместо
        // простого вызова Dispose.
        link.Dispose();
        
        // P.S. Ну да, ну да, а мне дописывать код до
        // рабочего состояния
        multiplyBlock.Complete();

        await foreach (var item in multiplyBlock.ReceiveAllAsync())
        {
            Console.WriteLine(item);
        }
        
        await multiplyBlock.Completion;
        
        // т.к. связь между блоками удалена,
        // блок subtractBlock тоже необходимо завершить
        subtractBlock.Complete();
        
        await foreach (var item in subtractBlock.ReceiveAllAsync())
        {
            Console.WriteLine(item);
        }
        
        await subtractBlock.Completion;
    }
}
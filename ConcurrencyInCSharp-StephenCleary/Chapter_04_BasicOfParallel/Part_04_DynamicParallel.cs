namespace ConcurrencyInCSharp_StephenCleary.Chapter_04_BasicOfParallel;

/*
Задача:
Требуется реализовать более сложную параллельную ситуацию:
структура и количество параллельных задач зависит от
информации, которая становится известной только во время
выполнения.
*/
public static class Part_04_DynamicParallel
{
    /*
    Метод Traverse обрабатывает текущий узел, а затем создает
    две дочерние задачи, по одной для каждой ветви под узлом
    (в данном примере предполагается, что родительские узлы
    должны быть обработаны до перехода к дочерним узлам).
    Метод ProcessTree начинает обработку, создавая
    родительскую задачу верхнего уровня и ожидая ее завершения:
    */
    public static void Traverse(Node current)
    {
        DoExpensiveActionOnNode(current);
        if (current.Left != null)
        {
            Task.Factory.StartNew(
                () => Traverse(current.Left),
                CancellationToken.None,
                TaskCreationOptions.AttachedToParent,
                TaskScheduler.Default);
        }

        if (current.Right != null)
        {
            Task.Factory.StartNew(
                () => Traverse(current.Right),
                CancellationToken.None,
                TaskCreationOptions.AttachedToParent,
                TaskScheduler.Default);
        }
    }

    public static void ProcessTree(Node root)
    {
        Task task = Task.Factory.StartNew(
            () => Traverse(root),
            CancellationToken.None,
            TaskCreationOptions.None,
            TaskScheduler.Default);
        task.Wait();
    }
    
    /*
    Вы можете запланировать запуск любой задачи после 
    другой задачи, используя продолжение
    */
    public static void ProcessTreeWithContinuation(Node root)
    {
        Task task = Task.Factory.StartNew(
            () => Traverse(root),
            CancellationToken.None,
            TaskCreationOptions.None,
            TaskScheduler.Default);

        Task continuation = task.ContinueWith(
            t => Console.WriteLine("All nodes have been processed"),
            CancellationToken.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default);
        continuation.Wait();

        // Внимание!
        // task не будет ждать завершения continuation,
        // поэтому необходимо дождаться завершения
        // continuation
        continuation.Wait();
    }

    #region Вспомогательные типы

    public class Node
    {
        public Node? Left { get; set; }
        public Node? Right { get; set; }
    }

    private static void DoExpensiveActionOnNode(Node node)
    {
        // Do something expensive
        Thread.Sleep(5000);
    }

    #endregion
}
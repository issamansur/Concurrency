namespace MyOwnTests.Exceptions;

public static class ExceptionsExamples
{ 
    // Correct example of exception throwing 
    // We use modificator async, so exceptions will be
    // caught and packed into Task.
    private static async Task ThrowNotImplementedExceptionAsyncCorrect()
    {
        throw new NotImplementedException();
    }

    private static async Task ThrowInvalidOperationExceptionAsyncCorrect()
    {
        throw new InvalidOperationException();
    }

    public static async Task ObserveAllExceptionsAsyncCorrect()
    {
        var task1 = ThrowNotImplementedExceptionAsyncCorrect();
        var task2 = ThrowInvalidOperationExceptionAsyncCorrect();

        Task allTasks = Task.WhenAll(task1, task2);
        try
        {
            await allTasks;
        }
        catch (Exception ex)
        {
            // Здесь будет InvalidOperationException / NotImplementedException.
            Console.WriteLine(ex.GetType());
            
            // Здесь получаем все исключения.
            if (allTasks.Exception != null)
            {
                foreach (var exception in allTasks.Exception.Flatten().InnerExceptions)
                {
                    Console.WriteLine(exception.GetType());
                }
            }
        }
    }

    // Correct example of exception throwing 
    // We use sync function and Task.FromException, 
    // so exceptions will be caught and packed into Task.
    private static Task ThrowNotImplementedExceptionSyncCorrect()
    {
        return Task.FromException(new NotImplementedException());
    }

    private static Task ThrowInvalidOperationExceptionSyncCorrect()
    {
        return Task.FromException(new InvalidOperationException());
    }

    public static async Task ObserveAllExceptionsSyncCorrect()
    {
        var task1 = ThrowNotImplementedExceptionSyncCorrect();
        var task2 = ThrowInvalidOperationExceptionSyncCorrect();

        Task allTasks = Task.WhenAll(task1, task2);
        try
        {
            await allTasks;
        }
        catch (Exception ex)
        {
            // Здесь будет InvalidOperationException / NotImplementedException.
            Console.WriteLine(ex.GetType());
            
            // Здесь получаем все исключения.
            if (allTasks.Exception != null)
            {
                foreach (var exception in allTasks.Exception.Flatten().InnerExceptions)
                {
                    Console.WriteLine(exception.GetType());
                }
            }
        }
    }

    // Incorrect example of exception throwing 
    // We use sync function and throw an exception directly, 
    // so exceptions will be thrown directly.
    private static Task ThrowNotImplementedExceptionSyncIncorrect()
    {
        throw new NotImplementedException();
    }

    private static Task ThrowInvalidOperationExceptionSyncIncorrect()
    {
        throw new InvalidOperationException();
    }

    public static async Task ObserveAllExceptionsSyncIncorrect()
    {
        var task1 = ThrowNotImplementedExceptionSyncIncorrect();
        var task2 = ThrowInvalidOperationExceptionSyncIncorrect();

        Task allTasks = Task.WhenAll(task1, task2);
        try
        {
            await allTasks;
        }
        catch (Exception ex)
        {
            // Здесь будет InvalidOperationException / NotImplementedException.
            Console.WriteLine(ex.GetType());
            
            // Здесь получаем все исключения.
            if (allTasks.Exception != null)
            {
                foreach (var exception in allTasks.Exception.Flatten().InnerExceptions)
                {
                    Console.WriteLine(exception.GetType());
                }
            }
        }
    }
}
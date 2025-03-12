namespace MyOwnTests.Exceptions;

public static class UnhandledExceptionsExamples
{
    // This method will throw an exception
    // 1. In the main thread
    // 2. Not caught
    // 3. Will crash the application
    public static void ThrowInMainThread(object? state = null)
    {
        throw new Exception("Exception thrown in Thread 1");
    }

    // This method will throw an exception
    // 1. In the main thread
    // 2. Caught in the main thread (finally block will be executed)
    // 3. Will not crash the application
    public static void ThrowInMainThreadAndCatch()
    {
        try
        {
            throw new Exception("Exception thrown in Thread 1");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Caught exception in Thread 1: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Finally block executed in Thread 1");
        }
    }

    // This method will throw an exception
    // 1. In the another (background/foreground) thread
    // 2. Not caught (finally block will not be executed)
    // 3. Will crash the application
    public static void ThrowInAnotherThread(bool isBackground = true)
    {
        try
        {
            var thread = new Thread(ThrowInMainThread)
            {
                IsBackground = isBackground
            };

            thread.Start();
            
            // Wait for the thread to finish
            thread.Join();
            
            Console.WriteLine("Thread 1 successfully finished");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Caught exception in Thread 2: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Finally block executed in Thread 2");
        }
    }

    // This method will throw an exception
    // 1. In the another (background/foreground) thread
    // 2. Caught in the another (background/foreground) thread (finally block will be executed)
    // 3. Will not crash the application
    public static void ThrowInAnotherThreadAndCatch(bool isBackground = true)
    {
        try
        {
            var thread = new Thread(ThrowInMainThreadAndCatch)
            {
                IsBackground = isBackground
            };
            thread.Start();
            
            // Wait for the thread to finish
            thread.Join();
            
            Console.WriteLine("Thread 1 successfully finished");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Caught exception in Thread 2: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Finally block executed in Thread 2");
        }
    }
    
    // This method will throw an exception
    // 1. In the another background thread, created by ThreadPool.QueueUserWorkItem
    // 2. Not caught (finally block will not be executed)
    // 3. Will crash the application
    public static void ThrowInAnotherThreadByQueueUserWorkItem()
    {
        try
        {
            ThreadPool.QueueUserWorkItem(ThrowInMainThread);
            
            // Wait for the thread to finish
            Thread.Sleep(1000);
            
            Console.WriteLine("Thread 1 successfully finished");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Caught exception in Thread 2: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Finally block executed in Thread 2");
        }
        Parallel.Invoke();
    }
    
    // P.S. Parallel.Invoke and Task.Run will not crash the application
    // because they catch exceptions and store them in the Task object
    
    // This method will throw an exception
    // 1. In the another (background/foreground) thread
    // 2. Not caught (finally block will not be executed)
    // 3. Will crash the application
    // P.S. UnhandledExceptionEventHandler will HANDLE this exception
    // but not prevent the application from crashing
    public static void ThrowInAnotherThreadWithHandler(bool isBackground = true)
    {
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var exception = (Exception) args.ExceptionObject;
            Console.WriteLine($"Caught exception in Thread 2 by handler: {exception.Message}");
            Console.WriteLine("Application will crash anyway");
        };
        
        try
        {
            var thread = new Thread(ThrowInMainThread)
            {
                IsBackground = isBackground
            };

            thread.Start();
            
            // Wait for the thread to finish
            thread.Join();
            
            Console.WriteLine("Thread 1 successfully finished");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Caught exception in Thread 2: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Finally block executed in Thread 2");
        }
    }
}
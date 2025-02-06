namespace ConcurrencyInCSharp_StephenCleary.Chapter_02_BasicsOfAsync;

/*
Задача:
Требуется реализовать синхронный метод с асинхронной сигнатурой.
Например, такая ситуация может возникнуть, если вы наследуете
от асинхронного интерфейса или базового класса, но хотите
реализовать его синхронно. Этот прием особенно полезен при
модульном тестировании асинхронного кода, когда нужна простая
заглушка или имитированная реализация для асинхронного
интерфейса.
 */
public class Part_02_ReturningFinishedTasks
{
    private interface IMyAsyncInterface
    {
        Task<int> GetValueAsync();
        Task DoSomethingAsync();
        Task<int> GetValueAsync(CancellationToken cancellationToken);
    }


    public class MySynchronousImplementation : IMyAsyncInterface
    {
        /*
        Можно использовать Task.FromResult для создания и
        возвращения нового объекта Task<T>, уже завершенного
        с заданным значением.
        */
        public Task<int> GetValueAsync()
        {
            return Task.FromResult(13);
        }

        /*
        Для методов, не имеющих возвращаемого значения,
        можно использовать Task.CompletedTask — кэшированный
        объект успешно завершенной задачи Task.
        */
        public Task DoSomethingAsync()
        {
            return Task.CompletedTask;
        }

        /*
        Если потребуется задача с другим типом результата
        (например, задача, завершенная с
        NotImplementedException), вы можете использовать
        Task.FromException.
        */
        public Task<T> NotImplementedAsync<T>()
        {
            return Task.FromException<T>(new NotImplementedException());
        }
        
        /*
        Аналогично существует метод Task.FromCanceled 
        для создания задач, уже отмененных из заданного 
        маркера CancellationToken
        */
        public Task<int> GetValueAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled<int>(cancellationToken);

            return Task.FromResult(13);
        }

        /*
        Если в синхронной реализации может произойти отказ,
        перехватывайте исключения и используйте
        Task.FromException для их возвращения.
        */
        public Task DoSomethingAsyncWithTryCatch()
        {
            try
            {
                DoSomethingSynchronously();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        private void DoSomethingSynchronously()
        {
            // Синхронный код
            // ...
        }
        
        /*
        Если вы регулярно используете Task.FromResult с одним
        значением, подумайте о кэшировании задачи.
        */
        private static readonly Task<int> zeroTask = Task.FromResult(0);
        Task<int> GetValueZeroAsync()
        {
            return zeroTask;
        }
    }
}
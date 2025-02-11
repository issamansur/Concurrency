using System.Windows.Input;
using Nito.AsyncEx;

namespace ConcurrencyInCSharp_StephenCleary.Chapter_02_BasicsOfAsync;

/*
Задача:
Имеется метод async void. Требуется обработать исключения,
распространенные из этого метода.
*/
public static class Part_09_HandlingErrorsAsyncVoid
{
    /*
    Хорошего решения не существует. Если возможно, измените
    метод так, чтобы он возвращал Task вместо void. В 
    некоторых ситуациях это невозможно; например, 
    представьте, что нужно провести модульное тестирование 
    реализации ICommand (которая должна возвращать void). 
    В этом случае необходимо предоставить перегруженную 
    версию вашего метода Execute, которая возвращает Task
    */
    sealed class MyAsyncCommand : ICommand
    {
        async void ICommand.Execute(object parameter)
        {
            try
            {
                await Execute(parameter);
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }

        public async Task Execute(object parameter)
        {
            // Здесь размещается асинхронная реализация команды.
            await Task.Delay(1000);
        }

        #region Другие составляющие (CanExecute и т. д.)

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public event EventHandler? CanExecuteChanged;

        #endregion
    }
    
    /*
    Также возможно обрабатывать исключения из методов 
    async void посредством управления SynchronizationContext.
    Можно воспользоваться типом AsyncContext из 
    NuGet-библиотеки Nito.AsyncEx.
    */
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                AsyncContext.Run(() => MainAsync(args));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }
        // ПЛОХОЙ КОД!!!
        // В реальных приложениях не используйте метод async void
        // без крайней необходимости.
        static async void MainAsync(string[] args)
        {
            //...
        }
    }
}
namespace CLRViaCSharp_JeffreyRichter.ExecutionContext;

using ExecutionContext = System.Threading.ExecutionContext;

public static class ExecutionContextExample
{
    private static AsyncLocal<string> _name = new AsyncLocal<string>();
    
    public static void ExecuteWithAndWithoutContext()
    {
        // Помещаем данные в контекст логического вызова потока метода Main
        //CallContext.LogicalSetData("Name", "Jeffrey");
        _name.Value = "Jeffrey";
        
        // Заставляем поток из пула работать
        // Поток из пула имеет доступ к данным контекста логического вызова
        ThreadPool.QueueUserWorkItem(
            state => Console.WriteLine(
                "Name={0}",
                //CallContext.LogicalGetData("Name")
                _name.Value
            )
        );
   
        // Запрещаем копирование контекста исполнения потока метода Main
        ExecutionContext.SuppressFlow();
   
        // Заставляем поток из пула выполнить работу.
        // Поток из пула НЕ имеет доступа к данным контекста логического вызова
        ThreadPool.QueueUserWorkItem(
            state => Console.WriteLine(
                "Name={0}", 
                //CallContext.LogicalGetData("Name")
                _name.Value
            )
        );
        // Восстанавливаем копирование контекста исполнения потока метода Main
        // на случай будущей работы с другими потоками из пула
        ExecutionContext.RestoreFlow();
    }
}
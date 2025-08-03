## Async как синтаксический сахар

Асинхронный код вида:

```csharp
async Task DoAsync() 
{
    await DoAsync2();
}
```

превращается на этапе компиляции в синхронный метод
и машину состояний вида:

```csharp
[AsyncStateMachine(typeof(DoAsyncStateMachine))]
public static Task DoAsync()
{
    DoAsyncStateMachine stateMachine = new DoAsyncStateMachine();
    stateMachine._builder = AsyncTaskMethodBuilder.Create();
    stateMachine._state = -1;
    stateMachine._builder.Start<DoAsyncStateMachine>(ref stateMachine);
    return stateMachine._builder.Task;
}

private sealed class DoAsyncStateMachine : IAsyncStateMachine
{
    public int _state;
    public AsyncTaskMethodBuilder _builder;
    private TaskAwaiter _taskAwaiter;

    void IAsyncStateMachine.MoveNext()
    {
        int num1 = this._state;
        try
        {
            TaskAwaiter awaiter1;
            int num2;
            if (num1 != 0)
            {
                awaiter = DoAsync2.GetAwaiter();
                if (!awaiter.IsCompleted)
                {
                    _state = num2 = 0;
                    _taskAwaiter = awaiter;
                    DoAsyncStateMachine stateMachine = this;
                    _builder.AwaitUnsafeOnCompleted<TaskAwaiter, DoAsyncStateMachine>(
                        ref awaiter1,
                        ref stateMachine
                    );
                    return;
                }
            }
            else
            {
                awaiter = this._taskAwaiter;
                this._taskAwaiter = new TaskAwaiter();
                this._state = num2 = -1;
            }

            awaiter.GetResult();
        }
        catch (Exception ex)
        {
            this._state = -2;
            this._builder.SetException(ex);
            return;
        }

        this._state = -2;
        this._builder.SetResult();
    }

    [DebuggerHidden]
    void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
    {
    }
}
```

```
В зависимости от типа сборки Debug/Release
машина состояний будет классом/структурой соответственно
```

## Превращение метода

Как видим метод

```csharp
async Task DoAsync() 
{
    await DoAsync2();
}
```

превратился в:

```csharp
public static Task DoAsync()
{
    DoAsyncStateMachine stateMachine = new DoAsyncStateMachine();
    stateMachine._builder = AsyncTaskMethodBuilder.Create();
    stateMachine._state = -1;
    stateMachine._builder.Start<DoAsyncStateMachine>(ref stateMachine);
    return stateMachine._builder.Task;
}
```

Заметим, что компилятор создал некую машину состояний (судя по названию)
с именем `DoAsyncStateMachine`, присваиваются значения её полям.
После чего у некоего билдера вызывается метод `Start(...)` и возвращается
таска, которая тоже хранится в загадочном билдере.

```
На самом деле компилятор создает машину состояний +- с таким же названием,
как и сам метод, но для компиляции и понимания я добавил приписку
StateMachine
```

```
Нет, builder это не TaskCreationSource, однако да функции у них
схожие. Наверное, это облегченная версия TCS для машины состояний,
но это не точно.
```

## Машина состояний

Как видим, не считая простейших операций присваивания и возврата,
единственная операция, которая могла каким-либо образом запустить
нашу таску и вернуть ожидаемую задачу, это:

```csharp
stateMachine._builder.Start<DoAsyncStateMachine>(ref stateMachine);
```

Учитывая, что `builder` создавался без каких-либо оригинальных настроек,
сделаем вывод, что все зависит от созданной машины состояний, которая
передается в качестве аргумента по ссылке.

Рассмотрим основные компоненты.

### Поля / состояние

### TODO

### Связь с TaskScheduler, ThreadPool, SynchronizationContext

```csharp
await DoAsync2();
```
Compilator convert to sync method with calling `builder`

```csharp
stateMachine._builder.Start<DoAsyncStateMachine>(ref stateMachine);
return stateMachine._builder.Task;
```

`builder.Start()` call `AsyncMachineState` and its `MoveNext()`.
`MoveNext()` start `Task` sync. If it has real async,
(P.S. `if (!awaiter.IsCompleted)`)
then we use `AwaitUnsafeOnCompleted()` or `AwaitOnCompleted()`.
It give us a callback to return to our state machine to continue
code when async operation will end.

```csharp
void IAsyncStateMachine.MoveNext()
{
    ...
    _builder.AwaitUnsafeOnCompleted<TaskAwaiter, DoAsyncStateMachine>(
        ref awaiter1,
        ref stateMachine
    );
    ...
}
```

Call by call `builder` use `TaskAwaiter.UnsafeOnCompletedInternal()`

```csharp
TaskAwaiter.UnsafeOnCompletedInternal(ta.m_task, box, continueOnCapturedContext: true);
```

It calls `Task.UnsafeSetContinuationForAwait()`

```csharp
task.UnsafeSetContinuationForAwait(stateMachineBox, continueOnCapturedContext);
```

HERE we meet our friends `SynchronizationContext` and `TaskSheduler`.

If we have custom `SynchronizationContext` (for example `UI`), then
we get it to continue on them.

If we don't have this one, but have custom `TaskScheduler`, then we
use it.

In every variant we also resolve `ExecutionContext`.

Else we start task on `ThreadPool`.

```csharp
if (continueOnCapturedContext)
{
    if (SynchronizationContext.Current is SynchronizationContext syncCtx && syncCtx.GetType() != typeof(SynchronizationContext))
    {
        tc = new SynchronizationContextAwaitTaskContinuation(syncCtx, stateMachineBox.MoveNextAction, flowExecutionContext: false);
        goto HaveTaskContinuation;
    }

    if (TaskScheduler.InternalCurrent is TaskScheduler scheduler && scheduler != TaskScheduler.Default)
    {
        tc = new TaskSchedulerAwaitTaskContinuation(scheduler, stateMachineBox.MoveNextAction, flowExecutionContext: false);
        goto HaveTaskContinuation;
    }
}

// Otherwise, add the state machine box directly as the continuation.
// If we're unable to because the task has already completed, queue it.
if (!AddTaskContinuation(stateMachineBox, addBeforeOthers: false))
{
    ThreadPool.UnsafeQueueUserWorkItemInternal(stateMachineBox, preferLocal: true);
}
return;

HaveTaskContinuation:
if (!AddTaskContinuation(tc, addBeforeOthers: false))
{
    tc.Run(this, canInlineContinuationTask: false);
}
```



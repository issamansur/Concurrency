## 1. Параллельная обработка данных

Если требуется выполнить одну и ту же операцию с каждым 
элементом данных - `Parallel` содержит метод `ForEach`, 
разработанный специально для этой задачи.

```csharp
Parallel.ForEach(collection, x => x.DoSomething(args));
```

Возможны ситуации, в которых преждевременно требуется 
прервать цикл:

```csharp
Parallel.ForEach(collection, (x, state) =>
{
    // ...
    state.Stop();
});
```

Более распространенная ситуация встречается тогда, когда 
требуется отменить параллельный цикл.

```csharp
Parallel.ForEach(
    collection, 
    new ParallelOptions { CancellationToken = token },
    x => x.DoSomething(args)
);
```

Любое совместное состояние должно быть защищено.

```csharp
object mutex = new object();

Parallel.ForEach(collection, x =>
{
    if (x.IsSomething)
    {
        lock (mutex)
        {
            // ...
        }
    }
});
```

Одно из различий между `Parallel` и `PLINQ` заключается в том, 
что `PLINQ` предполагает, что может использовать все ядра на
компьютере, тогда как `Parallel` может динамически реагировать 
на изменения условий процессора.

`Parallel.ForEach` реализует параллельный цикл `foreach`. 
Если вам потребуется выполнить параллельный цикл `for`, 
то класс Parallel также поддерживает метод `Parallel.For`.

## 2. Параллельное агрегирование

Для поддержки агрегирования класс `Parallel` использует 
концепцию локальных значений — переменных, существующих 
локально внутри параллельного цикла.

Следует отметить, что делегату `localFinally` не нужно 
синхронизировать доступ к переменной для хранения результата.

```csharp
object mutex = new object();
int result = 0;
Parallel.ForEach(source: values,
      localInit: () => 0,
      body: (item, state, localValue) => localValue + item,
      localFinally: localValue =>
      {
        lock (mutex)
          result += localValue;
      });
```

в `PLINQ` реализована встроенная поддержка многих 
распространенных операторов (например, `Sum`)

```csharp
values.AsParallel().Sum(); // и другие операторы
```

В `PLINQ` также предусмотрена обобщенная поддержка 
агрегирования с оператором `Aggregate`

```csharp
values.AsParallel().Aggregate(
    seed: 0,
    func: (sum, item) => sum + item,
    resultSelector: sum => sum
);
```

## 3. Параллельный вызов

Класс `Parallel` содержит простой метод `Invoke`, 
спроектированный для таких сценариев.

```csharp
Parallel.Invoke(
    () => DoSomething(),
    () => DoSomethingElse()
);
```

Методу `Parallel.Invoke` также можно передать 
массив делегатов

```csharp
Parallel.Invoke(
    new Action[] { DoSomething, DoSomethingElse }
);
```

`Parallel.Invoke` поддерживает отмену операции.

```csharp
CancellationTokenSource cts = new CancellationTokenSource();
Parallel.Invoke(
    new ParallelOptions { CancellationToken = cts.Token },
    () => DoSomething(),
    () => DoSomethingElse()
);
```

Метод `Parallel.Invoke` — отличное решение для простого 
параллельного вызова. Отмечу, что он уже не так хорошо 
подходит для ситуаций, в которых требуется активизировать 
действие для каждого элемента входных данных 
(для этого лучше использовать `Parallel.ForEach`), или если
каждое действие производит некоторый вывод (вместо этого 
следует использовать `Parallel LINQ`).

## 4. Динамический параллелизм

Если потребуется реализовать динамический параллелизм, 
проще использовать тип `Task` напрямую.

Флаг `AttachedToParent` гарантирует, что задача `Task` для
каждой ветви связывается с задачей `Task` своего 
родительского узла. Таким образом создаются отношения 
«родитель/потомок» между экземплярами `Task`

```csharp
Task parent = Task.Factory.StartNew(() =>
{
    Task.Factory.StartNew(() => DoSomething(), TaskCreationOptions.AttachedToParent);
    Task.Factory.StartNew(() => DoSomethingElse(), TaskCreationOptions.AttachedToParent);
});
```

Если ваша ситуация не относится к категории 
«родитель/потомок», вы можете запланировать запуск любой 
задачи после другой задачи, используя `ContinueWith`.

```csharp
Task task = Task.Factory.StartNew(() => DoSomething());
task.ContinueWith(t => DoSomethingElse());
```

Всегда лучше явно задать планировщик `TaskScheduler`, 
используемый `StartNew` и `ContinueWith`.

С таким же успехом можно сохранить каждую новую задачу в 
потокобезопасной коллекции, а затем ожидать завершения их 
всех с использованием `Task.WaitAll`.

Использование `Task` для параллельной обработки принципиально
отличается от использования `Task` для асинхронной обработки.

Тип `Task` в параллельном программировании служит двум целям:
он может представлять параллельную или асинхронную задачу.
Параллельные задачи могут использовать блокирующие методы,
такие как `Task.Wait`, `Task.Result`, `Task.WaitAll` и 
`Task.WaitAny`. Параллельные задачи также обычно используют
`AttachedToParent` для создания отношений «родитель/потомок»
между задачами. Параллельные задачи следует создавать 
методами `Task.Run` или `Task.Factory.StartNew`.

## 5. Parallel LINQ

`PLINQ` хорошо работает в потоковых сценариях, которые 
получают последовательность входных значений и производят
последовательность выходных значений.

Следует помнить, что класс `Parallel` ведет себя более 
корректно с другими процессами в системе, чем `PLINQ`; этот
фактор становится особенно существенным при выполнении 
параллельной обработки на серверной машине.

`PLINQ` предоставляет параллельные версии многих операторов, 
включая фильтрацию (`Where`), проекцию (`Select`) и разные
виды агрегирования, такие как `Sum`, `Average` и более общую
форму `Aggregate`. В общем случае все, что можно сделать с
обычным `LINQ`, также можно сделать в параллельном
режиме с `PLINQ`.
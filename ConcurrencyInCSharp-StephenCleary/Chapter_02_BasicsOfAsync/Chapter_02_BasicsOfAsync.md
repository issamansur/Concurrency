## 1. Приостановка на заданный период времени

**Экспоненциальная задержка** — стратегия 
увеличения задержек между повторными попытками.
Используйте ее при работе с веб-службами, чтобы
не перегружать сервер повторными попытками.

```
В реальном коде я бы рекомендовал применить более 
качественное решение (например, использующее 
библиотеку Polly NuGet).
```

`Task.Delay` неплохо подходит для модульного тестирования 
асинхронного кода или реализации логики повторных попыток.
Но если нужно реализовать тайм-аут, лучшим кандидатом
будет `CancellationToken`.

## 2. Возврат результатов синхронных методов с асинхронной сигнатурой

Можно использовать `Task.FromResult` для создания и возвращения
нового объекта `Task<T>`, уже завершенного с заданным значением.

Для методов, не имеющих возвращаемого значения, можно 
использовать `Task.CompletedTask` — кэшированный объект
успешно завершенной задачи `Task`.

Если потребуется задача с результатом ошибки вы можете
использовать `Task.FromException`.

Аналогично существует метод `Task.FromCanceled` для создания
задач, уже отмененных из заданного маркера
`CancellationToken`.

Если вы регулярно используете `Task.FromResult` с одним 
значением, подумайте о кэшировании задачи.

```
На логическом уровне Task.FromResult, Task.FromException и Task.
 FromCanceled  являются вспомогательными методами и сокращенными
формами обобщенного типа TaskCompletionSource<T>. 
TaskCompletionSource<T> представляет собой низкоуровневый тип,
полезный для взаимодействия с другими формами асинхронного кода.
```

```
В общем случае следует применять сокращенную форму 
Task.FromResult и родственные формы, если хотите вернуть уже 
завершенную задачу. Используйте TaskCompletionSource<T> 
для возвращения задачи, которая завершается в некоторый момент
будущего.
```

## 3. Передача информации о ходе выполнения операции (IProgress<T>)

Используйте типы `IProgress<T>` и `Progress<T>`. 
Ваш `async`-метод должен получать аргумент `IProgress<T>`;
здесь `T` — тип прогресса, о котором вы хотите сообщать.

Помните, что метод `IProgress<T>.Report` обычно является 
асинхронным. Это означает, что `MyMethodAsync` может продолжить
выполнение перед сообщением о прогрессе.

По этой причине лучше определить `T` как неизменяемый тип 
(или по крайней мере тип-значение). Если T является изменяемым
ссылочным типом, то вам придется самостоятельно создавать
отдельную копию при каждом вызове `IProgress<T>.Report`.

`Progress<T>` сохраняет текущий контекст при создании и 
активизирует свой обратный вызов в этом контексте. Это 
означает, что если `Progress<T>` конструируется в UI-потоке,
то вы сможете обновить пользовательский интерфейс из его
обратного вызова, даже если асинхронный метод вызывает 
`Report` из фонового потока.

## 4. Ожидание завершения группы задач

Фреймворк предоставляет для этой цели метод `Task.WhenAll`.
Метод получает несколько задач и возвращает задачу, которая
завершается при завершении всех указанных задач.

Если все задачи имеют одинаковый тип результата и все 
завершаются успешно, то задача `Task.WhenAll` возвращает массив,
содержащий результаты всех задач

```csharp
T[] results = await Task.WhenAll(task1, task2, task3);
```

Есть перегруженная версия `Task.WhenAll`, которая получает 
`IEnumerable` с задачами; тем не менее я не рекомендую ее
использовать. Код получается более понятным, когда я явно 
«материализую» последовательность:

```csharp
// Определить действие, выполняемое для каждого x .
var tasks = collection.Select(x => DoSmthAsync(x));

// Запустить все задачи одновременно.
Task<T>[] startedTasks = tasks.ToArray();
// Все задачи запущены.
```

Если какие-либо задачи выдают исключения, то `Task.WhenAll`
сообщает об отказе своей возвращенной задачи с этим
исключением. Если сразу несколько задач выдают исключение,
то все эти исключения помещаются в задачу `Task`, 
возвращаемую `Task.WhenAll`. Тем не менее при ожидании этой
задачи будет выдано только одно из них. Если нужно каждое
конкретное исключение, проверьте свойство `Exception` задачи
`Task`, возвращаемой `Task.WhenAll`.

## 5. Ожидание завершения любой задачи

Используйте метод `Task.WhenAny`. Метод `Task.WhenAny`
получает последовательность задач и возвращает задачу,
которая завершается при завершении любой из задач 
последовательности.

Задача, возвращенная `Task.WhenAny`, никогда не завершается 
в состоянии отказа или отмены. Эта «внешняя» задача всегда
завершается успешно, а ее результирующее значение 
представляет собой первую завершенную задачу `Task` 
(«внутреннюю»).

Когда первая задача завершается, подумайте, не отменить ли
остальные задачи. Если другие задачи не отменяются, но к ним
не применяется await, они просто теряются. Потерянные задачи
отрабатывают до завершения, но их результаты игнорируются.
Все исключения от потерянных задач также будут
проигнорированы. Если эти задачи не будут отменены, они
продолжат работать и неэффективно расходовать ресурсы
(подключения HTTP, подключения к базе данных, таймеры и т. д.).

Вы можете использовать Task.WhenAny для реализации
тайм-аута (например, при использовании Task.Delay как одной
из задач), но так поступать не рекомендуется.

Другой антипаттерн `Task.WhenAny` — обработка задач по мере
их завершения. Сначала может показаться разумным вести список
задач и удалять каждую задачу из списка при завершении.
Проблема в том, что такое решение выполняется за время 
`O(N2)`, хотя существует алгоритм со временем `O(N)`.

## 6. Обработка задач при завершении

Простейшее решение заключается в рефакторинге кода и введении
высокоуровневого `async`-метода, который обеспечивает
ожидание задачи и обработку ее результата.

```csharp
async Task AwaitAndProcessAsync<T>(Task<T> task)
{
    T result = await task;
    // обработка результата
}

{
    // Создание задач
    // ...

    IEnumerable<Task> taskQuery =
        from t in tasks select AwaitAndProcessAsync(t);
    Task[] processingTasks = taskQuery.ToArray();
}
```

или же проще (для кого как).

```csharp
Task[] processingTasks = tasks.Select(async t =>
{
    var result = await t;
    // обработка результата
}).ToArray();
```

Метод расширения OrderByCompletion также доступен в 
библиотеке с открытым кодом `AsyncEx`
(NuGet-пакет `Nito.AsyncEx`). С таким методом расширения 
изменения в исходной версии кода сводятся до минимума:

```csharp
// Ожидать каждой задачи по мере выполнения.
foreach (Task<int> task in tasks.OrderByCompletion())
{
    T result = await task;
    // обработка результата
}
```

## 7. Обход контекста при продолжении

Чтобы избежать возобновления в контексте, используйте `await`
для результата `ConfigureAwait` и передайте `false` в 
параметре `continueOnCapturedContext`: 

```csharp
await Task.Delay(1000).ConfigureAwait(false);
// Этот метод теряет свой контекст при возобновлении.
```

Если у вас имеется `async`-метод с частями, требующими 
контекста, и частями, свободными от контекста, рассмотрите
возможность его разбиения на два (или более) `async`-метода.

## 8. Обработка исключений из методов async Task

Исключения можно перехватывать простой конструкцией 
`try/catch`, как вы бы сделали для синхронного кода.

Исключения, выданные из методов async Task, помещаются в 
возвращаемый объект `Task`. Они выдаются только при
использовании await с возвращаемым объектом `Task`.

Возможны ситуации (например, с `Task.WhenAll`), в которых 
`Task` может содержать несколько исключений, а `await` 
повторно выдает только первое из них.

## 9. Обработка исключений из методов async void

Лучше избегать распространения исключений из методов 
`async void`. Если же вы должны использовать метод 
`async void`, рассмотрите возможность упаковки всего кода 
в блок `try` и прямой обработки исключений.

Когда метод `async void` распространяет исключение, это
исключение выдается в контексте `SynchronizationContext`, 
активном на момент начала выполнения метода `async void`. 
Если среда выполнения предоставляет `SynchronizationContext`, 
то обычно она предоставляет механизм обработки этих 
высокоуровневых исключений на глобальном уровне.

Например, в приложениях 
WPF - это `Application.DispatcherUnhandledException`,
Universal Windows - `Application.UnhandledException`,
ASP.NET - `UseExceptionHandler`.

Также возможно обрабатывать исключения из методов `async void`
посредством управления `SynchronizationContext`. Можно 
воспользоваться типом `AsyncContext` из `NuGet`-библиотеки
`Nito.AsyncEx`.

Будьте внимательны и не устанавливайте этот контекст 
`SynchronizationContext` в потоках, которые вам не
принадлежат. 

## 10. Создание ValueTask

`ValueTask<T>` используется как возвращаемый тип в ситуациях, 
в которых обычно может быть возвращен синхронный результат, 
а асинхронное поведение встречается реже.

Нередко метод, возвращающий `ValueTask<T>`, способен 
немедленно вернуть значение; в таких случаях можно применить
оптимизацию для этого сценария с использованием конструктора
`ValueTask<T>`. 

```csharp
public ValueTask<int> GetIntAsync()
{
    if (someCondition)
        return new ValueTask<int>(42);
    return new ValueTask<int>(GetIntAsyncCore());
}
```

Большинство методов должно возвращать `Task<T>`, поскольку 
при потреблении `Task<T>` возникает меньше скрытых ловушек,
чем при потреблении `ValueTask<T>`.

## 11. Использование ValueTask

Самый простой и прямолинейный способ потребления 
`ValueTask<T>` или `ValueTask` основан на `await`.

Также можно выполнить `await` после выполнения конкурентной 
операции как в случае с `Task<T>`

`ValueTask` или `ValueTask<T>` может ожидаться только один 
раз, поэтому вы не можете одновременно использовать `await` 
и вызвать `AsTask` для одного `ValueTask<T>`.

Синхронное получение результатов от `ValueTask` или
`ValueTask<T>` может быть выполнено только один раз, после
завершения `ValueTask`, и это значение` ValueTask` уже не
может использоваться для ожидания или преобразования в
задачу.
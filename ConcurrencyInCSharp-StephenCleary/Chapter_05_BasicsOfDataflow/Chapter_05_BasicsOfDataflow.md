## 1. Связывание блоков

`Complete()` - метод, который завершает блок данных:

1. не принимаем новые данные через `Post()` или `SendAsync()`

2. завершает обработку данных, которые уже находятся в блоке

`await block.Completion;` - ожидание завершения обработки

1. Вызывается после вызова `Complete()`.

2. Вызывается без `Complete()`, если распространяется 
завершение через связанные блоки.

3. Возвращает `Task`, который завершается, 
когда блок завершает обработку. То есть, все данные 
переработаны и переданы дальше или получены вручную.

4. Если в блоке происходит отказ, то и в задаче `Completion`
тоже происходит отказ.

Метод расширения `LinkTo` предоставляет простой механизм 
связывания блоков потока данных:

```csharp
oneBlock.LinkTo(twoBlock);
```

Чтобы распространять завершение (и ошибки), установите
параметр `PropagateCompletion` для связи:

```csharp
var options = new DataflowLinkOptions 
{
    PropagateCompletion = true
};
oneBlock.LinkTo(twoBlock, options);
```

Таким образом, если имеется длинный конвейер, 
распространяющий завершения, исходная ошибка может быть
вложена в несколько экземпляров `AggregateException`.

## 2. Распространение ошибок

Если делегат, переданный блоку потока данных, выдает 
исключение, то этот блок входит в состояние отказа. 
Блок в состоянии отказа теряет все свои данные 
(и перестает принимать новые).

Свойство `Completion` возвращает объект `Task`, который 
завершается при завершении блока, а если в блоке 
происходит отказ, то и в задаче `Completion` тоже 
происходит отказ.

Когда вы распространяете завершение с помощью параметра связи
`PropagateCompletion`, ошибки тоже распространяются. 
Однако исключение передается следующему блоку, упакованному
в `AggregateException`.

Метод `AggregateException.Flatten` упрощает обработку ошибок
в сценарии с длинным конвейером и упакованными исключениями.

Также возможен другой вариант: если вы хотите, чтобы блоки
сохраняли работоспособность перед лицом исключений, можно 
рассматривать исключения как другую разновидность данных и 
дать им проходить через сеть с правильно обрабатываемыми 
элементами данных. 

P.S. **"Рельсовое программирование"**.

## 3.  Удаление связей между блоками

Связи между блоками потока данных можно создавать или 
удалять в любой момент; данные могут свободно проходить 
по сети, и это не помешает безопасно создавать или удалять 
связи. Как создание, так и удаление связей являются 
полностью потокобезопасными.

При создании связи между блоками потока данных сохраните 
объект `IDisposable`, возвращенный методом `LinkTo`, и 
уничтожьте его, когда потребуется разорвать связь между 
блоками:

```csharp
IDisposable link = oneBlock.LinkTo(twoBlock);
// ...
link.Dispose();
```

В реальном коде стоит рассмотреть возможность блока `using` 
вместо простого вызова `Dispose`.

Здесь нет условий гонки, которые могли бы привести к 
дублированию или потере данных.

Сценарий с разрывом связи нетипичен, но и он может 
пригодиться в некоторых ситуациях:

1. При смене фильтра между блоками
2. Для приостановки сети потока данных

## 4. Регулирование блоков

При связи, когда один блок источник связан с двумя и более
блоками-приемниками, можно воспользоваться регулировкой 
(`throttling`) блоков-приемников с использованием 
параметра блока `BoundedCapacity`

По умолчанию BoundedCapacity присваивается значение 
`DataflowBlockOptions.Unbounded`, при котором первый 
блок-приемник буферизует все данные, даже если еще не 
готов к их обработке.

Регулировка полезна для распределения нагрузки в 
конфигурациях с ветвлением.

Если сеть потока данных заполняется данными от операции 
ввода/вывода, можно применить `BoundedCapacity` к блокам 
своей сети. В этом случае вы не прочитаете слишком много
данных ввода/вывода до того, как сеть будет к этому готова.
А все входные данные не будут буферизованы сетью до того,
как она сможет его обработать.

## 5. Параллельная обработка с блоками потока данных

Если один конкретный блок выполняет интенсивные вычисления
на процессоре, — вы можете дать команду этому блоку 
работать параллельно с входными данными,
устанавливая параметр `MaxDegreeOfParallelism`.

`BoundedCapacity` можно присвоить 
`DataflowBlockOptions.Unbounded` или любое значение, 
большее `0`. Следующий пример позволяет любому количеству 
задач умножать данные одновременно:

```csharp
var block = new TransformBlock<int, int>(
    x => Convert(x),
    new ExecutionDataflowBlockOptions
    {
        MaxDegreeOfParallelism = 4
    }
);
```

Неожиданно высокое количество элементов данных может 
указывать на то, что реорганизация или параллелизация 
могли бы принести пользу.

`MaxDegreeOfParallelism` также работает и в том случае, 
если блок потока данных выполняет асинхронную обработку.
В этом случае параметр `MaxDegreeOfParallelism` задает 
уровень параллелизма — определенное количество слотов. 
Каждый элемент данных занимает слот, когда блок приступает
к его обработке, и покидает этот слот только при полном 
завершении асинхронной обработки.

## 6. Создание собственных блоков

Можно выделить любую часть сети потока данных, содержащую 
один входной и один выходной блок, с помощью метода 
`Encapsulate`.

```csharp
IPropagatorBlock<T1, T2> propagator = 
    DataflowBlock.Encapsulate(
        transformBlock1,
        transformBlock2
    );
```

Подумайте, как каждый параметр блока должен (или не должен)
передаваться вашей внутренней сети; во многих случаях 
некоторые параметры блоков неприменимы или не имеют смысла.
По этой причине нестандартные блоки обычно определяют 
собственные нестандартные параметры вместо того, чтобы
получать параметр `DataflowBlockOptions`.

`DataflowBlock.Encapsulate` инкапсулирует только сеть с 
одним входным и одним выходным блоками. 

P.S. Я слаб для этого, поэтому просто цитирую:

Если у вас имеется сеть с несколькими входными и/или 
выходными блоками, предназначенная для повторного
использования, вам следует инкапсулировать ее в специальном
объекте и предоставить доступ к входам и выходам как к 
свойствам типа `ITargetBlock<T>` (для входов) и 
`IReceivableSourceBlock<T>` (для выходов).



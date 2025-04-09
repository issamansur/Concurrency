// Decompiled with JetBrains decompiler
// Type: MyOwnTests.StateMachines.AsyncToStateMachineOneByOne.AsyncToStateMachineOneByOne
// Assembly: MyOwnTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FE176EF7-82A0-47D2-816F-244B511C7A5B
// Assembly location: C:\Users\EDEXADE\Desktop\Concurrency\MyOwnTests\bin\Release\net8.0\MyOwnTests.dll
// Local variable names from c:\users\edexade\desktop\concurrency\myowntests\bin\release\net8.0\myowntests.pdb
// Compiler-generated code is shown

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MyOwnTests.StateMachines.AsyncToStateMachineOneByOne
{
  public static class AsyncToStateMachineOneByOneRelease
  {
    [AsyncStateMachine(typeof(DoAsync1State))]
    public static Task<int> DoAsync1()
    {
      // Создаём машину состояний для первой функции с параметрами:
      // builder (req) - Билдер, используемый как легковесный CTS.
      // state (req) - Текущее состояние стэйт машины, в частности и таски
      // --- Начальное значение: -1
      // --- После первого вызова MoveNext, если значение не было вычислено сразу: 0
      // --- Окончание таски (результат или ошибка): -2
      // parameter (opt) - параметр, если функция принимает параметр
      var stateMachine = new DoAsync1State
      {
        _builder = AsyncTaskMethodBuilder<int>.Create(),
        _state = -1
      };
      // Метод для запуска билдера и, соответственно, ATMB:
      // 1. Сохраняем контекст синхронизации и исполнения
      // 2. Вызываем MoveNext у стейт машины
      // 3. Возвращаем контекст синхронизации и исполнения,
      // если они были изменены у вызывающего потока
      // (вход по стеку)
      stateMachine._builder.Start<DoAsync1State>(ref stateMachine);
      // (выход по стеку)
      return stateMachine._builder.Task;
    }

    [AsyncStateMachine(typeof(DoAsync2State))]
    public static Task<int> DoAsync2()
    {
      var stateMachine = new DoAsync2State
      {
        _builder = AsyncTaskMethodBuilder<int>.Create(),
        _state = -1
      };
      stateMachine._builder.Start<DoAsync2State>(ref stateMachine);
      return stateMachine._builder.Task;
    }

    [AsyncStateMachine(typeof(DoAsync3State))]
    public static Task<int> DoAsync3()
    {
      DoAsync3State stateMachine = new DoAsync3State
      {
        _builder = AsyncTaskMethodBuilder<int>.Create(),
        _state = -1
      };
      stateMachine._builder.Start<DoAsync3State>(ref stateMachine);
      return stateMachine._builder.Task;
    }

    //[CompilerGenerated]
    [StructLayout(LayoutKind.Auto)]
    private struct DoAsync1State : IAsyncStateMachine
    {
      public int _state;

      public AsyncTaskMethodBuilder<int> _builder;

      private TaskAwaiter<int> _taskAwaiter;

      void IAsyncStateMachine.MoveNext()
      {
        int num1 = _state;
        int result1;
        try
        {
          TaskAwaiter<int> awaiter;
          // MoveNext вызван в первый раз
          if (num1 != 0)
          {
            Console.WriteLine("DoAsync1 started");
            // Синхронно "идём" в следующий асинхронный метод,
            // вызывая функцию синхронно и получая TaskAwaiter
            // для полученной таски (вход по стеку)
            awaiter = DoAsync2().GetAwaiter();

            // Если эта стейт машина принадлежит последней асинхронной
            // функции в стеке вызовов, то мы доходим сюда.
            if (!awaiter.IsCompleted)
            {
              _state = 0;
              _taskAwaiter = awaiter;
              // выполнение таски
              _builder.AwaitUnsafeOnCompleted<TaskAwaiter<int>, DoAsync1State>(ref awaiter, ref this);
              // После возврата:
              // 1. Таска возвращается по стеку
              // 2. MoveNext вызовется при завершении операции снова 
              // через ContinueWith(t => MoveNext())
              // и пойдет обратно по стеку, завершая задачу
              // (выход по стеку в ожидании окончания задачи)
              return;
            }
          }
          // Метод вызывается после того, как колбэк вызовет
          // после завершения асинхронной операции
          else
          {
            awaiter = _taskAwaiter;
            _taskAwaiter = new TaskAwaiter<int>();
            _state = -1;
          }

          int result2 = awaiter.GetResult();
          Console.WriteLine("DoAsync1 finished");
          result1 = result2;
        }
        catch (Exception ex)
        {
          _state = -2;
          _builder.SetException(ex);
          return;
        }

        _state = -2;
        _builder.SetResult(result1);
      }

      [DebuggerHidden]
      void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
      {
        _builder.SetStateMachine(stateMachine);
      }
    }

    [CompilerGenerated]
    [StructLayout(LayoutKind.Auto)]
    private struct DoAsync2State : IAsyncStateMachine
    {
      public int _state;

      public AsyncTaskMethodBuilder<int> _builder;

      private TaskAwaiter<int> _taskAwaiter;

      void IAsyncStateMachine.MoveNext()
      {
        int num1 = _state;
        int result1;
        try
        {
          TaskAwaiter<int> awaiter;
          if (num1 != 0)
          {
            Console.WriteLine(" DoAsync2 started");
            awaiter = DoAsync3().GetAwaiter();
            if (!awaiter.IsCompleted)
            {
              _state = 0;
              _taskAwaiter = awaiter;
              _builder.AwaitUnsafeOnCompleted<TaskAwaiter<int>, DoAsync2State>(ref awaiter, ref this);
              return;
            }
          }
          else
          {
            awaiter = _taskAwaiter;
            _taskAwaiter = new TaskAwaiter<int>();
            _state = -1;
          }

          int result2 = awaiter.GetResult();
          Console.WriteLine(" DoAsync2 finished");
          result1 = result2;
        }
        catch (Exception ex)
        {
          _state = -2;
          _builder.SetException(ex);
          return;
        }

        _state = -2;
        _builder.SetResult(result1);
      }

      [DebuggerHidden]
      void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
      {
        _builder.SetStateMachine(stateMachine);
      }
    }

    [CompilerGenerated]
    [StructLayout(LayoutKind.Auto)]
    private struct DoAsync3State : IAsyncStateMachine
    {
      public int _state;

      public AsyncTaskMethodBuilder<int> _builder;
      private TaskAwaiter _taskAwaiter;

      void IAsyncStateMachine.MoveNext()
      {
        int num1 = _state;
        int result;
        try
        {
          TaskAwaiter awaiter;
          if (num1 != 0)
          {
            Console.WriteLine("  DoAsync3 started");
            awaiter = Task.Delay(1000).GetAwaiter();
            if (!awaiter.IsCompleted)
            {
              _state = 0;
              _taskAwaiter = awaiter;
              _builder.AwaitUnsafeOnCompleted<TaskAwaiter, DoAsync3State>(ref awaiter, ref this);
              return;
            }
          }
          else
          {
            awaiter = _taskAwaiter;
            _taskAwaiter = new TaskAwaiter();
            _state = -1;
          }

          awaiter.GetResult();
          Console.WriteLine("  DoAsync3 finished");
          result = 42;
        }
        catch (Exception ex)
        {
          _state = -2;
          _builder.SetException(ex);
          return;
        }

        _state = -2;
        _builder.SetResult(result);
      }

      [DebuggerHidden]
      void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
      {
        _builder.SetStateMachine(stateMachine);
      }
    }
  }
}

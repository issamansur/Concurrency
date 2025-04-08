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
  //[NullableContext(1)]
  //[Nullable(0)]
  public static class AsyncToStateMachineOneByOneRelease
  {
    [AsyncStateMachine(typeof (DoAsync1State))]
    public static Task<int> DoAsync1()
    {
      DoAsync1State stateMachine = new DoAsync1State();
      stateMachine._builder = AsyncTaskMethodBuilder<int>.Create();
      stateMachine._state = -1;
      stateMachine._builder.Start<DoAsync1State>(ref stateMachine);
      return stateMachine._builder.Task;
    }

    [AsyncStateMachine(typeof (DoAsync2State))]
    public static Task<int> DoAsync2()
    {
      DoAsync2State stateMachine = new DoAsync2State();
      stateMachine._builder = AsyncTaskMethodBuilder<int>.Create();
      stateMachine._state = -1;
      stateMachine._builder.Start<DoAsync2State>(ref stateMachine);
      return stateMachine._builder.Task;
    }

    [AsyncStateMachine(typeof (DoAsync3State))]
    public static Task<int> DoAsync3()
    {
      DoAsync3State stateMachine = new DoAsync3State();
      stateMachine._builder = AsyncTaskMethodBuilder<int>.Create();
      stateMachine._state = -1;
      stateMachine._builder.Start<DoAsync3State>(ref stateMachine);
      return stateMachine._builder.Task;
    }

    [CompilerGenerated]
    [StructLayout(LayoutKind.Auto)]
    private struct DoAsync1State : 
    /*[Nullable(0)]*/
    IAsyncStateMachine
    {
      public int _state;
      //[Nullable(0)]
      public AsyncTaskMethodBuilder<int> _builder;
      //[Nullable(0)]
      private TaskAwaiter<int> _taskAwaiter;

      void IAsyncStateMachine.MoveNext()
      {
        int num1 = this._state;
        int result1;
        try
        {
          TaskAwaiter<int> awaiter;
          int num2;
          if (num1 != 0)
          {
            Console.WriteLine("DoAsync1 started");
            awaiter = DoAsync2().GetAwaiter();
            if (!awaiter.IsCompleted)
            {
              this._state = num2 = 0;
              this._taskAwaiter = awaiter;
              this._builder.AwaitUnsafeOnCompleted<TaskAwaiter<int>, DoAsync1State>(ref awaiter, ref this);
              return;
            }
          }
          else
          {
            awaiter = this._taskAwaiter;
            this._taskAwaiter = new TaskAwaiter<int>();
            this._state = num2 = -1;
          }
          int result2 = awaiter.GetResult();
          Console.WriteLine("DoAsync1 finished");
          result1 = result2;
        }
        catch (Exception ex)
        {
          this._state = -2;
          this._builder.SetException(ex);
          return;
        }
        this._state = -2;
        this._builder.SetResult(result1);
      }

      [DebuggerHidden]
      void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
      {
        this._builder.SetStateMachine(stateMachine);
      }
    }

    [CompilerGenerated]
    [StructLayout(LayoutKind.Auto)]
    private struct DoAsync2State : 
    /*[Nullable(0)]*/
    IAsyncStateMachine
    {
      public int _state;
      //[Nullable(0)]
      public AsyncTaskMethodBuilder<int> _builder;
      //[Nullable(0)]
      private TaskAwaiter<int> _taskAwaiter;

      void IAsyncStateMachine.MoveNext()
      {
        int num1 = this._state;
        int result1;
        try
        {
          TaskAwaiter<int> awaiter;
          int num2;
          if (num1 != 0)
          {
            Console.WriteLine(" DoAsync2 started");
            awaiter = DoAsync3().GetAwaiter();
            if (!awaiter.IsCompleted)
            {
              this._state = num2 = 0;
              this._taskAwaiter = awaiter;
              this._builder.AwaitUnsafeOnCompleted<TaskAwaiter<int>, DoAsync2State>(ref awaiter, ref this);
              return;
            }
          }
          else
          {
            awaiter = this._taskAwaiter;
            this._taskAwaiter = new TaskAwaiter<int>();
            this._state = num2 = -1;
          }
          int result2 = awaiter.GetResult();
          Console.WriteLine(" DoAsync2 finished");
          result1 = result2;
        }
        catch (Exception ex)
        {
          this._state = -2;
          this._builder.SetException(ex);
          return;
        }
        this._state = -2;
        this._builder.SetResult(result1);
      }

      [DebuggerHidden]
      void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
      {
        this._builder.SetStateMachine(stateMachine);
      }
    }

    [CompilerGenerated]
    [StructLayout(LayoutKind.Auto)]
    private struct DoAsync3State : 
    /*[Nullable(0)]*/
    IAsyncStateMachine
    {
      public int _state;
      //[Nullable(0)]
      public AsyncTaskMethodBuilder<int> _builder;
      private TaskAwaiter _taskAwaiter;

      void IAsyncStateMachine.MoveNext()
      {
        int num1 = this._state;
        int result;
        try
        {
          TaskAwaiter awaiter;
          int num2;
          if (num1 != 0)
          {
            Console.WriteLine("  DoAsync3 started");
            awaiter = Task.Delay(1000).GetAwaiter();
            if (!awaiter.IsCompleted)
            {
              this._state = num2 = 0;
              this._taskAwaiter = awaiter;
              this._builder.AwaitUnsafeOnCompleted<TaskAwaiter, DoAsync3State>(ref awaiter, ref this);
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
          Console.WriteLine("  DoAsync3 finished");
          result = 42;
        }
        catch (Exception ex)
        {
          this._state = -2;
          this._builder.SetException(ex);
          return;
        }
        this._state = -2;
        this._builder.SetResult(result);
      }

      [DebuggerHidden]
      void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
      {
        this._builder.SetStateMachine(stateMachine);
      }
    }
  }
}

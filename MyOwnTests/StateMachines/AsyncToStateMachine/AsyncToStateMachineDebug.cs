// Decompiled with JetBrains decompiler
// Type: MyOwnTests.StateMachines.AsyncToStateMachine.AsyncToStateMachine_debug
// Assembly: MyOwnTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4FCA03D9-4C9B-4971-B7C5-76ACD0F58B4E
// Assembly location: D:\projects\Concurrency\MyOwnTests\bin\Debug\net8.0\MyOwnTests.dll
// Local variable names from d:\projects\concurrency\myowntests\bin\debug\net8.0\myowntests.pdb
// Compiler-generated code is shown

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MyOwnTests.StateMachines.AsyncToStateMachine
{
    public static class AsyncToStateMachineDebug
    {
        //[NullableContext(1)]
        [AsyncStateMachine(typeof(DoAsyncWithAwaitState))]
        [DebuggerStepThrough]
        public static Task DoAsyncWithAwait()
        {
            DoAsyncWithAwaitState stateMachine = new DoAsyncWithAwaitState();
            stateMachine._builder = AsyncTaskMethodBuilder.Create();
            stateMachine._state = -1;
            stateMachine._builder.Start<DoAsyncWithAwaitState>(ref stateMachine);
            return stateMachine._builder.Task;
        }

        [CompilerGenerated]
        private sealed class DoAsyncWithAwaitState : IAsyncStateMachine
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
                    TaskAwaiter awaiter2;
                    if (num1 != 0)
                    {
                        if (num1 != 1)
                        {
                            Console.WriteLine("Before async 1");

                            // ASYNC 1 BLOCK START

                            awaiter1 = Task.Delay(0).GetAwaiter();
                            if (!awaiter1.IsCompleted)
                            {
                                _state = num2 = 0;
                                _taskAwaiter = awaiter1;
                                DoAsyncWithAwaitState stateMachine = this;
                                _builder.AwaitUnsafeOnCompleted<TaskAwaiter, DoAsyncWithAwaitState>(ref awaiter1,
                                    ref stateMachine);
                                return;
                            }
                        }
                        else
                        {
                            awaiter2 = this._taskAwaiter;
                            this._taskAwaiter = new TaskAwaiter();
                            this._state = num2 = -1;
                            goto label_9;
                        }
                    }
                    else
                    {
                        awaiter1 = this._taskAwaiter;
                        this._taskAwaiter = new TaskAwaiter();
                        this._state = num2 = -1;
                    }

                    awaiter1.GetResult();

                    // ASYNC 1 BLOCK END

                    Console.WriteLine("Between async 1 & 2");

                    // ASYNC 2 BLOCK START

                    awaiter2 = Task.Delay(0).GetAwaiter();
                    if (!awaiter2.IsCompleted)
                    {
                        _state = num2 = 1;
                        _taskAwaiter = awaiter2;
                        DoAsyncWithAwaitState stateMachine = this;
                        _builder.AwaitUnsafeOnCompleted<TaskAwaiter, DoAsyncWithAwaitState>(ref awaiter2,
                            ref stateMachine);
                        return;
                    }

                    label_9:
                    awaiter2.GetResult();

                    // ASYNC 2 BLOCK END

                    Console.WriteLine("After async 2");
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
            void IAsyncStateMachine.SetStateMachine( /*[Nullable(1)]*/ IAsyncStateMachine stateMachine)
            {
            }
        }
    }
}
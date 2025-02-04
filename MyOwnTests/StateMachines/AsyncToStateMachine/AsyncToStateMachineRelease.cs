// Decompiled with JetBrains decompiler
// Type: MyOwnTests.StateMachines.AsyncToStateMachine.AsyncToStateMachineRelease
// Assembly: MyOwnTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7F167E7-1444-481F-970A-F6621218FDD6
// Assembly location: D:\projects\Concurrency\MyOwnTests\bin\Release\net8.0\MyOwnTests.dll
// Local variable names from d:\projects\concurrency\myowntests\bin\release\net8.0\myowntests.pdb
// Compiler-generated code is shown

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MyOwnTests.StateMachines.AsyncToStateMachine
{
    public static class AsyncToStateMachineRelease
    {
        //[NullableContext(1)]
        [AsyncStateMachine(typeof(AsyncToStateMachineRelease.DoAsyncWithAwaitState))]
        public static Task DoAsyncWithAwait()
        {
            DoAsyncWithAwaitState stateMachine = new DoAsyncWithAwaitState();
            stateMachine._builder = AsyncTaskMethodBuilder.Create();
            stateMachine._state = -1;
            stateMachine._builder.Start<DoAsyncWithAwaitState>(ref stateMachine);
            return stateMachine._builder.Task;
        }

        [CompilerGenerated]
        [StructLayout(LayoutKind.Auto)]
        private struct DoAsyncWithAwaitState : IAsyncStateMachine
        {
            public int _state;
            public AsyncTaskMethodBuilder _builder;
            private TaskAwaiter taskAwaiter;

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
                                this._state = num2 = 0;
                                this.taskAwaiter = awaiter1;
                                this._builder.AwaitUnsafeOnCompleted<TaskAwaiter, DoAsyncWithAwaitState>(ref awaiter1,
                                    ref this);
                                return;
                            }
                        }
                        else
                        {
                            awaiter2 = this.taskAwaiter;
                            this.taskAwaiter = new TaskAwaiter();
                            this._state = num2 = -1;
                            goto label_9;
                        }
                    }
                    else
                    {
                        awaiter1 = this.taskAwaiter;
                        this.taskAwaiter = new TaskAwaiter();
                        this._state = num2 = -1;
                    }

                    awaiter1.GetResult();

                    // ASYNC 1 BLOCK END

                    Console.WriteLine("Between async 1 & 2");

                    // ASYNC 2 BLOCK START

                    awaiter2 = Task.Delay(0).GetAwaiter();
                    if (!awaiter2.IsCompleted)
                    {
                        this._state = num2 = 1;
                        this.taskAwaiter = awaiter2;
                        this._builder
                            .AwaitUnsafeOnCompleted<TaskAwaiter, DoAsyncWithAwaitState>(ref awaiter2, ref this);
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
                this._builder.SetStateMachine(stateMachine);
            }
        }
    }
}
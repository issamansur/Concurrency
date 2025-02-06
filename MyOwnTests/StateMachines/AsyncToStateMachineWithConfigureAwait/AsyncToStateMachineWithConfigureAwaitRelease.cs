// Decompiled with JetBrains decompiler
// Type: MyOwnTests.StateMachines.AsyncToStateMachineWithConfigureAwait.AsyncToStateMachineWithConfigureAwaitRelease
// Assembly: MyOwnTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2E75E510-A707-404F-B72D-236BA8729B68
// Assembly location: D:\projects\Concurrency\MyOwnTests\bin\Release\net8.0\MyOwnTests.dll
// Local variable names from D:\projects\Concurrency\MyOwnTests\bin\Release\net8.0\MyOwnTests.pdb
// Compiler-generated code is shown

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MyOwnTests.StateMachines.AsyncToStateMachineWithConfigureAwait
{
    public static class AsyncToStateMachineWithConfigureAwaitRelease
    {
        //[NullableContext(1)]
        [AsyncStateMachine(typeof(DoAsyncWithConfigureAwaitState))]
        public static Task DoAsyncWithConfigureAwait()
        {
            DoAsyncWithConfigureAwaitState stateMachine = new();
            stateMachine._builder = AsyncTaskMethodBuilder.Create();
            stateMachine._state = -1;
            stateMachine._builder.Start<DoAsyncWithConfigureAwaitState>(ref stateMachine);
            return stateMachine._builder.Task;
        }

        [CompilerGenerated]
        [StructLayout(LayoutKind.Auto)]
        private struct DoAsyncWithConfigureAwaitState : IAsyncStateMachine
        {
            public int _state;
            public AsyncTaskMethodBuilder _builder;
            private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter _taskAwaiter;

            void IAsyncStateMachine.MoveNext()
            {
                int num1 = this._state;
                try
                {
                    ConfiguredTaskAwaitable.ConfiguredTaskAwaiter awaiter;
                    int num2;
                    if (num1 != 0)
                    {
                        Console.WriteLine("Before sync");
                        awaiter = Task.Delay(1000).ConfigureAwait(true).GetAwaiter();
                        if (!awaiter.IsCompleted)
                        {
                            this._state = num2 = 0;
                            this._taskAwaiter = awaiter;
                            this._builder
                                .AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable.ConfiguredTaskAwaiter,
                                    DoAsyncWithConfigureAwaitState>(ref awaiter, ref this);
                            return;
                        }
                    }
                    else
                    {
                        awaiter = this._taskAwaiter;
                        this._taskAwaiter = new ConfiguredTaskAwaitable.ConfiguredTaskAwaiter();
                        this._state = num2 = -1;
                    }

                    awaiter.GetResult();
                    Console.WriteLine("After sync");
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
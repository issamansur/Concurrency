// Decompiled with JetBrains decompiler
// Type: MyOwnTests.StateMachines.AsyncToStateMachineWithoutAwait.AsyncToStateMachineWithoutAwait
// Assembly: MyOwnTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E1DDF063-5EB8-4D30-A2A2-1DF34B4D70EB
// Assembly location: D:\projects\Concurrency\MyOwnTests\bin\Release\net8.0\MyOwnTests.dll
// Local variable names from d:\projects\concurrency\myowntests\bin\release\net8.0\myowntests.pdb
// Compiler-generated code is shown

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MyOwnTests.StateMachines.AsyncToStateMachineWithoutAwait
{
    public static class AsyncToStateMachineWithoutAwaitRelease
    {
        //[NullableContext(1)]
        [AsyncStateMachine(typeof(DoAsyncWithoutAwaitState))]
        public static Task DoAsyncWithoutAwait()
        {
            DoAsyncWithoutAwaitState stateMachine;
            stateMachine._builder = AsyncTaskMethodBuilder.Create();
            stateMachine._state = -1;
            stateMachine._builder.Start<DoAsyncWithoutAwaitState>(ref stateMachine);
            return stateMachine._builder.Task;
        }

        [CompilerGenerated]
        [StructLayout(LayoutKind.Auto)]
        private struct DoAsyncWithoutAwaitState : IAsyncStateMachine
        {
            public int _state;
            public AsyncTaskMethodBuilder _builder;

            void IAsyncStateMachine.MoveNext()
            {
                try
                {
                    // Code section START
                    Console.WriteLine("Before sync");
                    Thread.Sleep(5000);
                    Console.WriteLine("After sync");
                    // Code section END          
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
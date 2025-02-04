using MyOwnTests;
using MyOwnTests.Parallel;
using MyOwnTests.StateMachines.AsyncToStateMachine;
using MyOwnTests.StateMachines.AsyncToStateMachineWithoutAwait;

HowMuchThreads.ForAndCount();

await AsyncToStateMachineDebug.DoAsyncWithAwait();
await AsyncToStateMachineRelease.DoAsyncWithAwait();

await AsyncToStateMachineWithoutAwaitRelease.DoAsyncWithoutAwait();

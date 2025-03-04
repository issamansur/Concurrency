using BenchmarkDotNet.Attributes;

namespace MyOwnTests.LeetCode.Concurrency._1115.PrintFooBarAlternately;

public class Benchmark
{
    [Benchmark]
    public async Task RunWithARE()
    {
        await Runner.RunAsync(new FooBarWithARE(1000));
    }

    [Benchmark]
    public async Task RunWithARE2()
    {
        await Runner.RunAsync(new FooBarWithARE2(1000));
    }

    [Benchmark]
    public async Task RunWithSemaphoreSlim()
    {
        await Runner.RunAsync(new FooBarWithSemaphoreSlim(1000));
    }

    [Benchmark]
    public async Task RunWithSpinWait()
    {
        await Runner.RunAsync(new FooBarWithSpinWait(1000));
    }
}
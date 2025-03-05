using BenchmarkDotNet.Attributes;

namespace MyOwnTests.LeetCode.Concurrency._1117.BuildingH2O;

public class Benchmark
{
    [Benchmark]
    public async Task TaskRunWithBoolAndARE()
    {
        await Runner.RunAsync(new H2OWithBoolAndARE(), 10);
    }

    [Benchmark]
    public async Task TaskRunWithBarrierAndSemaphores()
    {
        await Runner.RunAsync(new H2OWithBarrierAndSemaphores(), 10);
    }
}
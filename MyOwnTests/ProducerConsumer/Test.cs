namespace MyOwnTests.ProducerConsumer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyOwnTests.ProducerConsumer;

public static class ProducerConsumerTestRunner
{
    public static async Task RunAllAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Testing all implementations with 1/1 and 1/5 configuration\n");

        var testCases = new List<(Func<IProducerConsumer<string>> Factory, string Name)>
        {
            (() => new BlockingCollectionProducerConsumer<string>(), "BlockingCollectionProducerConsumer"),
            (() => new ChannelProducerConsumer<string>(), "ChannelProducerConsumer"),
            (() => new TplDataflowOnlyBufferProducerConsumer<string>(100), "TplDataflowOnlyBufferProducerConsumer"),
            (() => new TplDataflowProducerConsumer<string>(consumersCount: 1), "TplDataflowProducerConsumer 1 consumer"),
            (() => new TplDataflowProducerConsumer<string>(consumersCount: 5), "TplDataflowProducerConsumer 5 consumers"),
        };

        foreach (var (factory, name) in testCases)
        {
            bool isTpl = name.StartsWith("TplDataflowProducerConsumer");
            await RunTestCaseAsync(factory(), name+ " (1 consumer)", messages: 5, consumerCount: isTpl ? 0 : 1, cancellationToken);

            if (!isTpl)
                await RunTestCaseAsync(factory(), name + " (5 consumers)", messages: 5, consumerCount: 5, cancellationToken);
        }
    }

    private static async Task RunTestCaseAsync(
        IProducerConsumer<string> queue,
        string name,
        int messages,
        int consumerCount = 0,
        CancellationToken cancellationToken = default
    )
    {
        Console.WriteLine($"--- Running {name} ---");

        var stopwatch = Stopwatch.StartNew();

        var consumers = Enumerable.Range(0, consumerCount).Select(_ =>
            Task.Run(() => queue.ProcessMessages(cancellationToken), cancellationToken)).ToArray();

        var producer = Task.Run(async () =>
        {
            for (int i = 0; i < messages; i++)
            {
                await queue.EnqueueMessage(i.ToString(), cancellationToken);
            }
            if (queue is BaseProducerConsumer<string> baseConsumer)
                await baseConsumer.OnStopAsync(cancellationToken);
        }, cancellationToken);

        await producer;
        await Task.WhenAll(consumers);

        stopwatch.Stop();
        Console.WriteLine($"Finished {name} in {stopwatch.Elapsed.TotalSeconds:F2} seconds\n");
    }
}
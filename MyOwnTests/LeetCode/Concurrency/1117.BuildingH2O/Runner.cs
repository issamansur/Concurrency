namespace MyOwnTests.LeetCode.Concurrency._1117.BuildingH2O;

public static class Runner
{
    // I changed slightly output of PrintFoo and PrintBar methods
    private static void PrintOxygen()
    {
        Console.Write("O");
    }
    
    private static void PrintHydrogen()
    {
        Console.Write("H");
    }
    
    // Runner for Solutions
    public static Task RunAsync(IH2O h2o, int countWaterMolecules = 10)
    {
        var tasksForHydrogen = Enumerable.Range(0, countWaterMolecules * 2)
            .Select(_ => Task.Run(() => h2o.Hydrogen(PrintHydrogen)));
            
        var tasksForOxygen = Enumerable.Range(0, countWaterMolecules)
            .Select(_ => Task.Run(() => h2o.Oxygen(PrintOxygen)))
            .ToList();
        
        var tasks = tasksForHydrogen
            .Concat(tasksForOxygen)
            .OrderBy(_ => Guid.NewGuid());

        var executedTasks = tasks.ToList();
        
        return Task.WhenAll(executedTasks);
    }
}
namespace MyOwnTests.LeetCode.Concurrency._1117.BuildingH2O;

// Solution with AutoResetEvent (good, but not the best)
// use bool instead of int counter for synchronization

// Remember to remove " : ISolution" from code snippet
using System.Threading;

public class H2OWithBoolAndARE : IH2O
{
    private readonly AutoResetEvent _hydrogenARE = new(false);
    private readonly AutoResetEvent _oxygenARE = new(true);
    
    private bool isReady = true;
    
    public void Hydrogen(Action releaseHydrogen) {
		_hydrogenARE.WaitOne();
        // releaseHydrogen() outputs "H". Do not change or remove this line.
        releaseHydrogen();
        isReady = !isReady;
        if (isReady)
        {
            _oxygenARE.Set();
        }
        else
        {
            _hydrogenARE.Set();
        }
    }

    public void Oxygen(Action releaseOxygen) {
        _oxygenARE.WaitOne();
        // releaseOxygen() outputs "O". Do not change or remove this line.
        releaseOxygen();
        _hydrogenARE.Set();
    }
}
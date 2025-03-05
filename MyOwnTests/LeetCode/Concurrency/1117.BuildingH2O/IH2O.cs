namespace MyOwnTests.LeetCode.Concurrency._1117.BuildingH2O;

public interface IH2O
{
    void Hydrogen(Action releaseHydrogen);
    void Oxygen(Action releaseOxygen);
}
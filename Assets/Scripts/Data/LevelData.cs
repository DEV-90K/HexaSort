using Newtonsoft.Json;

public class LevelData
{
    [JsonProperty]
    public GridData Grid { get; private set; }

    [JsonProperty]
    public StackQueueData StackQueueData { get; private set; }

    public LevelData(GridData grid, StackQueueData stackQueueData)
    {
        Grid = grid;
        StackQueueData = stackQueueData;
    }

    public LevelData(GridData grid)
    {
        Grid = grid;
        StackQueueData = null;
    }

    public LevelData()
    {
    }

    public void UpdateGridData(GridData grid)
    {
        Grid = grid;
    }
}

public class LevelPresenterData
{
    [JsonProperty]
    public int Level { get; private set; }
    [JsonProperty]
    public int Goal { get; private set; }
    [JsonProperty]
    public int Amount { get; private set; } = 3; // Amount of color appearing in the level with idHex from 1 to Amount 
    [JsonProperty]
    public int[] Probabilities { get; private set; } = new int[] { 40, 30, 30 }; //Probability of random type of stack 1 color, 2 color ... total element = 100;

    public LevelPresenterData()
    {
    }

    public LevelPresenterData(int level, int goal)
    {
        Level = level;
        Goal = goal;
    }

    public LevelPresenterData(int level, int goal, int amount, int[] probabilities) : this(level, goal)
    {
        Amount = amount;
        Probabilities = probabilities;
    }

    public void UpdateLevel(int level)
    {
        Level = level;
    }

    public void UpdateGoal(int goal)
    {
        Goal = goal;
    }
}


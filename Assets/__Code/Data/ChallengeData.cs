using Newtonsoft.Json;

public class ChallengeData
{
    public GridData Grid { get; private set; }
    public StackQueueData StackQueueData { get; private set; }

    public ChallengeData()
    {
    }

    public ChallengeData(StackQueueData stackQueueData)
    {
        StackQueueData = stackQueueData;
    }

    public ChallengeData(GridData grid, StackQueueData stackQueueData)
    {
        Grid = grid;
        StackQueueData = stackQueueData;
    }
}

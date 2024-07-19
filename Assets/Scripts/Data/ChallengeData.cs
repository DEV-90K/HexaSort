using Newtonsoft.Json;


public class ChallengeData
{
    [JsonProperty]
    public GridData Grid { get; private set; }
    [JsonProperty]
    public StackQueueData StackQueueData {  get; private set; }

    public ChallengeData()
    {
    }

    public ChallengeData(GridData gridData)
    {
        this.Grid = gridData;
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


public class ChallengePresenterData
{
    [JsonProperty]
    public int Challenge { get; private set; }

    public ChallengePresenterData(int challenge)
    {
        Challenge = challenge;
    }

    public void UpdateChallenge(int IDChallenge)
    {
        Challenge = IDChallenge;
    }
}
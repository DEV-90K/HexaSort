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
    [JsonProperty]
    public int Coin { get; private set; }
    [JsonProperty]
    public int Material { get; private set; }

    public ChallengePresenterData()
    {
    }

    public ChallengePresenterData(int challenge, int coin, int material)
    {
        Challenge = challenge;
        Coin = coin;
        Material = material;
    }

    public void UpdateChallenge(int IDChallenge)
    {
        Challenge = IDChallenge;
    }
}
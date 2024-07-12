using UnityEngine;

public class ChallengeManager : MonoSingleton<ChallengeManager>
{
    private enum ChalleneState
    {
        NONE,
        LOADING,
        PLAYING,
        PAUSED,
        FINISHED
    }

    [SerializeField]
    private GridManager _gridManager;
    [SerializeField]
    private StackManager _stackManager;

    private ChallengeData _challengeData;

    private int amountHexagon = 0;
    private ChalleneState challeneState = ChalleneState.NONE;

    //private void Start()
    //{
    //    challeneState = ChalleneState.NONE;

    //    Hexagon.OnVanish += Hexagon_OnVanish;
    //    StackMerger.OnStackMergeCompleted += StackMerge_OnStackMergeCompleted;

    //    InitChallengeTest();
    //    GameManager.Instance.ChangeState(GameState.PLAYING);
    //}

    private void OnDestroy()
    {
        Hexagon.OnVanish -= Hexagon_OnVanish;
        StackMerger.OnStackMergeCompleted -= StackMerge_OnStackMergeCompleted;
    }

    private void InitChallengeTest()
    {
        ChallengeData challengeData = ResourceManager.Instance.GetChallengeByID(1);        
        OnInit(challengeData);
    }

    public void OnInit(ChallengeData challengeData)
    {
        Debug.Log("OnInit ChallengeData: ");
        challengeData.DebugLogObject();

        challeneState = ChalleneState.LOADING;

        _challengeData = challengeData;
        amountHexagon = 0;

        _gridManager.OnInit(challengeData.Grid);
        _stackManager.OnInit(challengeData.StackQueueData);

        challeneState = ChalleneState.PLAYING;
    }

    public void OnReplay()
    {
        _stackManager.CollectRandomImmediate();
        _gridManager.CollectGridImmediate();

        OnInit(_challengeData);
    }

    public void OnFinish()
    {
        Debug.Log("OnFinish");
        if (challeneState == ChalleneState.FINISHED)
            return;

        challeneState = ChalleneState.FINISHED;

        _gridManager.CollectOccupied();
        _stackManager.CollectRandomed();

        Invoke(nameof(InitChallengeTest), 1f);
    }

    //Test Case Wrong: In Process Merge and Remove
    private void Hexagon_OnVanish()
    {
        if (challeneState != ChalleneState.PLAYING)
        {
            return;
        }
    }

    private void StackMerge_OnStackMergeCompleted()
    {
        //Debug.Log("Level State: " + EnumUtils.ParseString(levelState));
        //if (levelState != LevelState.PLAYING)
        //{
        //    return;
        //}

        //if (amountHexagon >= _presenterData.Goal)
        //{
        //    OnFinish();
        //}
    }
}

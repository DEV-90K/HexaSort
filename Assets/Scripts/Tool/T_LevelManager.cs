using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_LevelManager : MonoBehaviour
{
    public static T_LevelManager Instance;
    private LevelData _currentLevel;
    public ChallengeData challengeData;
    public ChallengePresenterData challengePresenterData;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void LoadLevelByData(LevelData levelData)
    {
        this._currentLevel = levelData;
        this.StartPlayLevel();
    }

    public void StartPlayLevel()
    {
        LevelData newLevel = new LevelData(this._currentLevel.Grid, this._currentLevel.StackQueueData);
        //T_GridController.Instance.InitDemo(10, 6);
    }

    public void SetChallengeData(ChallengeData challenge, ChallengePresenterData challengePresenter)
    {
        this.challengeData = challenge;
        this.challengePresenterData = challengePresenter;
    }
}

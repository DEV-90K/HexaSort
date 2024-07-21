using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_LevelManager : MonoBehaviour
{
    public static T_LevelManager Instance;
    public ChallengeData challengeData;
    public ChallengePresenterData challengePresenterData;
    public Dictionary<string, T_HexaInBoardData> hexasSelected;
    private LevelData _currentLevel;

    public int colorNumber;
    public int hexInEachHexaNumber;

    private void Awake()
    {
        Instance = this;
        this.hexasSelected = new Dictionary<string, T_HexaInBoardData>();
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

    public void SetHexaSelected(List<T_HexaInBoardObject> hexasSelected)
    {
        foreach(var item in hexasSelected)
        {
            this.hexasSelected[item.name] = item.GetDataHexa();
        }
    }

    public void SetLevel(int hexInEachHexaNumber, int colorNumber)
    {
        this.hexInEachHexaNumber = hexInEachHexaNumber;
        this.colorNumber = colorNumber;
    }
}

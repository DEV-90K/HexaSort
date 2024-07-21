using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_LevelManager : MonoBehaviour
{
    public static T_LevelManager Instance;
    public ChallengeData challengeData;
    public ChallengePresenterData challengePresenterData;
    public Dictionary<string, T_HexaInBoardData> hexasSelected = new Dictionary<string, T_HexaInBoardData>();
    private LevelData _currentLevel;

    public int colorNumber;
    public int hexInEachHexaNumber;

    private void Awake()
    {
        Instance = this;
        
        if(T_Data.Instance != null)
        {
            hexasSelected = T_Data.Instance._hexasSelected;
            colorNumber = T_Data.Instance.colorNumber;
            hexInEachHexaNumber = T_Data.Instance.hexInEachHexaNumber;
        }

        //DontDestroyOnLoad(this);
    }

    public void CacheData()
    {
        T_Data.Instance.colorNumber = colorNumber;
        T_Data.Instance.hexInEachHexaNumber = hexInEachHexaNumber;
        //T_Data.Instance.hexasSelected = hexasSelected;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_LevelManager : MonoBehaviour
{
    public static T_LevelManager Instance;
    private LevelData _currentLevel; 

    private void Awake()
    {
        Instance = this;
    }

    public void LoadLevelByData(LevelData levelData)
    {
        this._currentLevel = levelData;
        this.StartPlayLevel();
    }

    public void StartPlayLevel()
    {
        LevelData newLevel = new LevelData(this._currentLevel);
        //T_GridController.Instance.InitDemo(10, 6);
    }
}

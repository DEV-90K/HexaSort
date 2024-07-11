using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class T_PanelExport : T_PanelBase
{
    public TMP_InputField LevelTxt;

    public void OnExportBtnClick()
    {
        T_LevelData levelData = T_ScreenTool.Instance.GetLevelData();
        int level = 0;
        int.TryParse(this.LevelTxt.text.Trim(), out level);
        levelData.Level = level;
        string levelFile = string.Format("Level_{0}", level);
        T_GridController.Instance.CanContact = true;
        Debug.LogError(JsonConvert.SerializeObject(levelData));
        WebGLFileSaver.SaveFile(JsonConvert.SerializeObject(levelData), levelFile);
    }

    public void OnCloseBtnClick()
    {
        T_GridController.Instance.CanContact = true;
        this.Hide();
    }
}

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
        //T_LevelData levelData = T_ScreenTool.Instance.GetTLevelData();
        int level = 0;
        int.TryParse(this.LevelTxt.text.Trim(), out level);
        //levelData.Level = level;

        //LevelData levelData = T_ScreenTool.Instance.GetLevelData();
        LevelData levelData = T_GridController.Instance.GetLevelData();
        string fileName = string.Format("Level_{0}", level);
        string configInfo = JsonConvert.SerializeObject(levelData);
        Debug.LogError(configInfo);
        WebGLFileSaver.SaveFile(CompressText.Compress(configInfo), fileName);
        T_GridController.Instance.CanContact = true;
    }

    public void OnCloseBtnClick()
    {
        this.Hide();
        T_GridController.Instance.CanContact = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_ScreenDemo : MonoBehaviour
{
    public void OnBackBtnClick()
    {
        //T_GridController.Instance.InitDemo(10, 6);
        T_GUIManager.Instance.ShowTool();
    }
}

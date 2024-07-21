using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_GUIManager : MonoBehaviour
{
    public static T_GUIManager Instance;

    public GameObject ScreenTool;
    public GameObject ScreenDemo;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        this.ShowTool();
        if (T_GameController.Instance.Grid != null)
        {
            if (ScreenManager.instance != null && ScreenManager.instance.hexasSelected.Count > 0)
            {
                T_GridController.Instance.Init(10, ScreenManager.instance.hexasSelected);
                T_ScreenTool.Instance.InitLevel(ScreenManager.instance.hexInEachHexaNumber, ScreenManager.instance.colorNumber);
            }
            T_GameController.Instance.ShowGrid();
        }
    }

    public void ShowTool()
    {
        this.ScreenTool.SetActive(true);
        this.ScreenDemo.SetActive(false);
    }

    public void ShowDemo()
    {
        this.ScreenTool.SetActive(false);
        this.ScreenDemo.SetActive(true);
    }
}

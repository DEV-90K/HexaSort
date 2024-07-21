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
            if (T_Data.Instance != null && T_Data.Instance._hexasSelected.Count > 0)
            {
                T_GameController.Instance.ShowGrid();
                T_GridController.Instance.Init(10, T_Data.Instance._hexasSelected);
                T_ScreenTool.Instance.InitLevel(T_Data.Instance.hexInEachHexaNumber, T_Data.Instance.colorNumber);
            }                       
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

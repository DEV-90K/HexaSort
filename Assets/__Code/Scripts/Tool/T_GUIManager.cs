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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_GameManager : MonoBehaviour
{
    public static T_GameManager Instance;

    public GameObject ScreenTool;
    public GameObject ScreenDemo;

    private void Awake()
    {
        Instance = this;
        this.ScreenTool.SetActive(true);
        this.ScreenDemo.SetActive(true);
    }

    void Start()
    {
        
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

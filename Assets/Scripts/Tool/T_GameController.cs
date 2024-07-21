using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_GameController : MonoBehaviour
{
    public static T_GameController Instance;
    public GameObject Grid;
    private void Awake()
    {
        Instance = this;
        this.HideGrid();
        DontDestroyOnLoad(this);
    }

    public void ShowGrid()
    {
        this.Grid.SetActive(true);
    }

    public void HideGrid()
    {
        this.Grid.SetActive(false);
    }
}

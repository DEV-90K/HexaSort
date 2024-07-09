using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class T_PanelColorGroup : T_PanelBase
{
    public GameObject ColorGroup;
    private GameObject _colorBtn;
    private List<GameObject> _colorBtnList;

    private void Awake()
    {
        this._colorBtn = this.ColorGroup.transform.GetChild(0).gameObject;
        this._colorBtnList = new List<GameObject>();
    }

    public void InitColorBtn(int colorNumber)
    {
        int childCount = this.ColorGroup.transform.childCount;
        if(childCount > colorNumber)
        {
            for (int i = 0; i < childCount; i++)
            {
                GameObject gObj = this.ColorGroup.transform.GetChild(i).gameObject;
                if(i < colorNumber)
                {
                    T_ColorButton colorButton = gObj.GetComponent<T_ColorButton>();
                    colorButton.InitColor(i);
                }
                else
                    gObj.SetActive(false);
            }
        }
    }

    public void OnColorBtnClick(GameObject obj)
    {
        T_ColorButton colorButton = obj.GetComponent<T_ColorButton>();
        T_ScreenTool.Instance.SetColorHexa(colorButton.GetId());
    }
}

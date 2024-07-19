using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class T_PanelColorGroup : T_PanelBase
{
    public static T_PanelColorGroup Instance;

    public GameObject ColorGroup;
    public GameObject HexaButton;

    private GameObject _colorBtn;
    private List<GameObject> _colorBtnList;

    private void Awake()
    {
        Instance = this;
        this._colorBtnList = new List<GameObject>();
    }

    public void InitColorBtn(int colorNumber)
    {
        this._colorBtn = this.ColorGroup.transform.GetChild(0).gameObject;
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

    public void InitColor(int colorNumber)
    {
        if (this._colorBtnList.Count > 0) this.DestroyColor();
        if (colorNumber > T_ConfigValue.ColorList.Length) colorNumber = T_ConfigValue.ColorList.Length;
        for (int i = 0; i < colorNumber; i++)
        {
            GameObject gObj = Instantiate(this.HexaButton, this.ColorGroup.transform);
            gObj.name = string.Format("{0}_{1}", "Color", i);
            T_HexaButton hexaButton = gObj.GetComponent< T_HexaButton>();
            if(hexaButton != null) hexaButton.InitColor(T_ConfigValue.ColorList[i]);
            this._colorBtnList.Add(gObj);
        }
    }

    public void DestroyColor()
    {
        foreach(var gObj in this._colorBtnList)
        {
            Destroy(gObj);
        }
    }

    public void OnColorBtnClick(GameObject obj)
    {
        T_ColorButton colorButton = obj.GetComponent<T_ColorButton>();
        T_ScreenTool.Instance.SetColorHexa(colorButton.GetId());
    }
}

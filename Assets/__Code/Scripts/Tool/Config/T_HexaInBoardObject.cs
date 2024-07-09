using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_HexaInBoardObject : MonoBehaviour
{
    public MeshRenderer ModelHexa;
    public Color _hexaColor 
    { 
        get => this.ModelHexa.material.color; 
        set => this.ModelHexa.material.color = value; 
    }

    private T_HexaInBoardData _data;

    private string _deactive = "#ffffff";
    private string _active = "#06D001";

    public void Init(int numberHexaInHexa)
    {
        this._data = new T_HexaInBoardData();
        this._data.Id = 0;
        this._data.IsSelected = false;
        this._data.ColorHexa = this._deactive;
        this._data.HexagonDatas = new T_HexaInBoardData[numberHexaInHexa];

        for (int i = 0; i < numberHexaInHexa; i++)
        {
            this._data.HexagonDatas[i] = new T_HexaInBoardData();
            this._data.HexagonDatas[i].Id = i + 1;
            this._data.HexagonDatas[i].ColorHexa = T_ConfigValue.ColorList[i];
        }

        this._hexaColor = T_Utils.ConvertToColor(this._data.ColorHexa);
    }

    public void CheckHexaInBoard()
    {
        if (!this._data.IsSelected)
        {
            this.SetSelectedHexa(true);
            T_ScreenTool.Instance.ShowOnClickHexa(null, false);
        }
        else
        {
            T_ScreenTool.Instance.ShowOnClickHexa(this, true);
        }
    }

    public void SetSelectedHexa(bool isSelected)
    {
        if(isSelected)
        {
            this._data.IsSelected = true;
            this._data.ColorHexa = this._active;
            this._hexaColor = T_Utils.ConvertToColor(this._data.ColorHexa);
        }
        else
        {
            this._data.IsSelected = false;
            this._data.ColorHexa = this._deactive;
            this._hexaColor = T_Utils.ConvertToColor(this._data.ColorHexa);
        }
    }

    public T_HexaInBoardData GetDataHexa()
    {
        return this._data;
    }
}

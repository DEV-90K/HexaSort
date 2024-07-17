using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class T_HexaInBoardObject : MonoBehaviour
{
    public MeshRenderer ModelHexa;
    public Color _hexaColor 
    { 
        get => this.ModelHexa.material.color; 
        set => this.ModelHexa.material.color = value; 
    }

    private T_HexaInBoardData _data; // Data hexa của tool
    private GridHexagonData _gridHexagonData; // Data hexa của gameplay

    private string _deactive = "#ffffff";
    private string _active = "#06D001";
    private string _hide = "#686D76";

    public void Init(int numberHexaInHexa)
    {
        //int colorChallenge = T_GridController.Instance.ColorChallenge;
        this._data = new T_HexaInBoardData();
        this._data.Id = 0;
        this._data.IsSelected = false;
        this._data.State = VisualState.SHOW;
        this._data.ColorHexa = this._deactive;
        //this._data.ColorHexa = this._active;
        this._data.HexagonDatas = new T_HexaInBoardData[numberHexaInHexa];

        if(numberHexaInHexa > 0)
        {
            for (int i = 0; i < numberHexaInHexa; i++)
            {
                this._data.HexagonDatas[i] = new T_HexaInBoardData();
                this._data.HexagonDatas[i].Id = i + 1;
                this._data.HexagonDatas[i].ColorHexa = T_ConfigValue.ColorList[i];
                //this._data.HexagonDatas[i].ColorHexa = T_ConfigValue.ColorList[colorChallenge];
            }
        }

        this._hexaColor = T_Utils.ConvertToColor(this._data.ColorHexa);
        /*if(colorChallenge < numberHexaInHexa )
        {
            colorChallenge += 1;
            T_GridController.Instance.ColorChallenge = colorChallenge;
        }*/
        //T_GridController.Instance.ShowNumberHexaInHexa(this);
    }

    public void InitData(int numberHexaInHexa, int row, int column)
    {
        this._gridHexagonData = new GridHexagonData(GridHexagonState.NONE, row, column, 1, new StackHexagonData(new int[numberHexaInHexa]));
        for (int i = 0; i < numberHexaInHexa; i++)
        {
            this._gridHexagonData.StackHexagon.IDHexes[i] = i;
        }
    }

    public void InitChallenge(int numberHexa, int countColor, List<T_HexaInBoardObject> nearHexas)
    {
        this._data = new T_HexaInBoardData();
        this._data.Id = 0;
        this._data.IsSelected = false;
        this._data.State = VisualState.SHOW;
        this._data.ColorHexa = this._deactive;
        this._data.HexagonDatas = new T_HexaInBoardData[numberHexa];

        if (nearHexas.Count == 0)
        {
            this.RandomColorHexa(numberHexa, countColor);
            T_GridController.Instance.ShowNumberHexaInHexa(this);
        }
        else
        {
            List<string> colors = new List<string>();
            for(int i = 0; i < nearHexas.Count; i++)
            {
                int count = nearHexas[i].GetDataHexa().HexagonDatas.Length;
                string color = nearHexas[i].GetDataHexa().HexagonDatas[count - 1].ColorHexa;
                colors.Add(color);
                //Debug.Log(string.Format("{0}_{1}", this.name, color));
            }
            this.RandomColorHexa(numberHexa, countColor, colors);
            T_GridController.Instance.ShowNumberHexaInHexa(this);
        }
    }

    public void RandomColorHexa(int numberHexa, int countColor, List<string> colorFist = null)
    {
        int item = numberHexa - 1;
        Debug.LogError("qqqq");
        if (item < 0) return;
        Debug.LogError("wwww");
        int id = UnityEngine.Random.Range(0, countColor);
        Debug.LogError(T_GridController.Instance.CheckListColors());
        if (!T_GridController.Instance.CheckListColors()) return;
        Debug.LogError("ccccc");
        string color = T_GridController.Instance.AddIdColor(id);
        Debug.Log(string.Format("item: {0} + color: {1} + countColor: {2} + id: {3}", item, color, countColor, id));
        if(string.IsNullOrEmpty(color))
        {
            //T_GridController.Instance.RemoveColorItem();
            countColor = T_GridController.Instance.GetCountColor();
            Debug.LogError(string.Format("item: {0} + color: {1} + countColor: {2}", item, color, countColor));
            RandomColorHexa(item + 1, countColor); 
        }
        else
        {
            if (colorFist == null)
            {
                this._data.HexagonDatas[item] = new T_HexaInBoardData();
                this._data.HexagonDatas[item].Id = item + 1;
                this._data.HexagonDatas[item].ColorHexa = color;
                RandomColorHexa(item, countColor);
            }
            else
            {
                this._data.HexagonDatas[item] = new T_HexaInBoardData();
                this._data.HexagonDatas[item].Id = item + 1;
                this._data.HexagonDatas[item].ColorHexa = color;
                if (item == this._data.HexagonDatas.Length - 1)
                {
                    string nearSameColor = colorFist.Where(x => x.Contains(this._data.HexagonDatas[item].ColorHexa)).FirstOrDefault();
                    //Debug.LogError(string.Format("ObjName: {0} + nearSameColor: {1} + item: {2} + LengthData: {3}", this.name, nearSameColor, item, this._data.HexagonDatas.Length - 1));
                    if (string.IsNullOrEmpty(nearSameColor)) // Nếu màu ô phía trên chưa giống với 1 màu xug quanh
                    {
                        for (int i = 0; i < colorFist.Count; i++)
                        {
                            string nearColor = T_GridController.Instance.AddIdColor(-1, colorFist[i]);
                            //Debug.Log(string.Format("ObjName: {0} + nearColor: {1} + colorFist: {2}", this.name, nearColor, colorFist[i]));
                            if (!string.IsNullOrEmpty(nearColor))
                            {
                                this._data.HexagonDatas[item].ColorHexa = nearColor;
                                break;
                            }
                        }
                    }
                }
                RandomColorHexa(item, countColor, colorFist);
            }
        }
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
            this._gridHexagonData.State = GridHexagonState.UNLOCK;

            this._data.IsSelected = true;
            this._data.ColorHexa = this._active;
            this._hexaColor = T_Utils.ConvertToColor(this._data.ColorHexa);
            T_GridController.Instance.SetSelectedHexaObj(this, true);
        }
        else
        {
            this._gridHexagonData.State = GridHexagonState.NONE;

            this._data.IsSelected = false;
            this._data.ColorHexa = this._deactive;
            this._hexaColor = T_Utils.ConvertToColor(this._data.ColorHexa);
            T_GridController.Instance.SetSelectedHexaObj(this, false);
        }
    }

    public void SetVisualState(VisualState state)
    {
        this._data.State = state;
        if(this._data.State == VisualState.HIDE)
        {
            this._gridHexagonData.State = GridHexagonState.LOCK_BY_GOAL;

            this._data.IsSelected = true;
            this._data.ColorHexa = this._hide;
            this._hexaColor = T_Utils.ConvertToColor(this._data.ColorHexa);
        }
        else
        {
            this._gridHexagonData.State = GridHexagonState.UNLOCK;

            this._data.IsSelected = true;
            this._data.ColorHexa = this._active;
            this._hexaColor = T_Utils.ConvertToColor(this._data.ColorHexa);
        }
    }

    public T_HexaInBoardData GetDataHexa()
    {
        return this._data;
    }

    public GridHexagonData GetGridHexagonData()
    {
        return this._gridHexagonData;
    }
}

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

    public void RandomItemHexaChallenge(int number)
    {
        Debug.Log(string.Format("{0}_{1}", this.gameObject, number));
        if(number == 0)
        {
            RandomItemHexa0();
        }
    }

    public void RandomItemHexa0()
    {
        int countItem = Random.Range(0, 10);
        int countColor = 0, color = 0, sum = 0, a = 10;
        int k = 0;
        while (countColor <= 10)
        {
            Debug.Log(countColor);
            color = Random.Range(0, 10);
            countColor = Random.Range(0, a);
            a = 10 - countColor;
            sum += countColor;
            if (sum == 10)
            {
                Debug.LogError(sum);
                break;
            }
            k++;
            if (k == 100) break;
        }
    }
}

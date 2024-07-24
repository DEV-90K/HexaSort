using System.Collections.Generic;
using System.Linq;
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
    private int _numberHexa;

    public void Init(int numberHexaInHexa)
    {
        //int colorChallenge = T_GridController.Instance.ColorChallenge;
        this._data = new T_HexaInBoardData();
        this._data.Id = 0;
        this._data.IsSelected = false;
        this._data.State = VisualState.EMPTY;
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

    public void Init(T_HexaInBoardData hexaData)
    {
        this._data.Id = hexaData.Id;
        this._data.IsSelected = hexaData.IsSelected;
        this._data.State = hexaData.State;
        this._data.ColorHexa = hexaData.ColorHexa;
        this._data.HexagonDatas = hexaData.HexagonDatas;

        this._hexaColor = T_Utils.ConvertToColor(this._data.ColorHexa);
        if (this._data.State == VisualState.SHOW)
        {
            T_GridController.Instance.ShowNumberHexaInHexa(this);
        }
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
        this._data.IsSelected = true;
        this._data.State = VisualState.SHOW;
        //this._data.ColorHexa = this._deactive;
        this._data.ColorHexa = this._active;
        //this._hexaColor = T_Utils.ConvertToColor(this._data.ColorHexa); // 
        this._data.HexagonDatas = new T_HexaInBoardData[numberHexa];

        this._numberHexa = numberHexa;

        if (nearHexas.Count == 0)
        {
            this.RandomColorHexa(numberHexa, countColor);
            T_GridController.Instance.ShowNumberHexaInHexa(this);
        }
        else
        {
            /*List<string> colors = new List<string>();
            for (int i = 0; i < nearHexas.Count; i++)
            {
                int count = nearHexas[i].GetDataHexa().HexagonDatas.Length;
                string color = nearHexas[i].GetDataHexa().HexagonDatas[count - 1].ColorHexa;
                colors.Add(color);
                //Debug.Log(string.Format("{0}_{1}", this.name, color));
            }
            this.RandomColorHexa(numberHexa, countColor, colors);*/
            this.RandomColorHexa(numberHexa, countColor, nearHexas);
            T_GridController.Instance.ShowNumberHexaInHexa(this);
        }
    }

    public void RandomColorHexa(int numberHexa, int countColor, List<T_HexaInBoardObject> nearHexaObj = null)
    {
        int item = numberHexa - 1;
        if (item < 0) return;
        int id = UnityEngine.Random.Range(0, countColor);
        //Debug.Log(string.Format("CheckListColors: {0} + countColor: {1}", T_GridController.Instance.CheckListColors(), countColor));
        if (!T_GridController.Instance.CheckListColors()) return;
        string color = T_GridController.Instance.AddIdColor(this, id);
        //Debug.Log(string.Format("item: {0} + color: {1} + countColor: {2} + id: {3}", item, color, countColor, id));
        if(string.IsNullOrEmpty(color))
        {
            countColor = T_GridController.Instance.GetCountColor();
            //Debug.LogError(string.Format("item: {0} + color: {1} + countColor: {2}", item, color, countColor));
            RandomColorHexa(item + 1, countColor); 
        }
        else
        {
            if (nearHexaObj == null)
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
                if (item == this._numberHexa - 1)
                {
                    //Debug.Log(string.Format("item: {0} + this._numberHexa - 1: {1}", item, this._data.HexagonDatas.Length - 1));
                    /*string nearSameColor = colorFist.Where(x => x.Contains(this._data.HexagonDatas[item].ColorHexa)).FirstOrDefault();
                    //Debug.LogError(string.Format("ObjName: {0} + nearSameColor: {1} + item: {2} + LengthData: {3}", this.name, nearSameColor, item, this._data.HexagonDatas.Length - 1));
                    if (string.IsNullOrEmpty(nearSameColor)) // Nếu màu ô phía trên chưa giống với 1 màu xug quanh thì lấy màu giống
                    {
                        for (int i = 0; i < colorFist.Count; i++)
                        {
                            string nearColor = T_GridController.Instance.AddIdColor(this, -1, colorFist[i]);
                            Debug.Log(string.Format("ObjName: {0} + nearColor: {1} + colorFist: {2}", this.name, nearColor, colorFist[i]));
                            if (!string.IsNullOrEmpty(nearColor))
                            {
                                this._data.HexagonDatas[item].ColorHexa = nearColor;
                                return;
                            }
                        }
                    }*/
                    GetColorHexaFirst(nearHexaObj, 1, item);
                }
                RandomColorHexa(item, countColor, nearHexaObj);
            }
        }
    }

    public void GetColorHexaFirst(List<T_HexaInBoardObject> nearHexaObj, int k, int item)
    {
        List<string> colorFist = new List<string>();
        for (int i = 0; i < nearHexaObj.Count; i++)
        {
            int count = nearHexaObj[i].GetDataHexa().HexagonDatas.Length;
            T_HexaInBoardData hexaData = nearHexaObj[i].GetDataHexa().HexagonDatas[count - k];
            string colorF = hexaData.ColorHexa;
            if (string.IsNullOrEmpty(colorF)) return;
            colorFist.Add(colorF);
            //Debug.Log(string.Format("nearHexaObj[{0}]: {1} + colorF: {2} + nearHexaObj.Count: {3}", i, nearHexaObj[i].name, colorF, nearHexaObj.Count));
        }
        string nearSameColor = colorFist.Where(x => x.Contains(this._data.HexagonDatas[item].ColorHexa)).FirstOrDefault();
        //Debug.LogError(string.Format("ObjName: {0} + nearSameColor: {1} + item: {2} + LengthData: {3}", this.name, nearSameColor, item, this._data.HexagonDatas.Length - 1));
        if (string.IsNullOrEmpty(nearSameColor)) // Nếu màu ô phía trên chưa giống với 1 màu xug quanh thì lấy màu giống
        {
            for (int i = 0; i < colorFist.Count; i++)
            {
                string nearColor = T_GridController.Instance.AddIdColor(this, -1, colorFist[i]);
                //Debug.Log(string.Format("ObjName: {0} + nearColor: {1} + colorFist: {2}", this.name, nearColor, colorFist[i]));
                if (!string.IsNullOrEmpty(nearColor))
                {
                    this._data.HexagonDatas[item].ColorHexa = nearColor;
                    break;
                }
                else if (i == colorFist.Count - 1) // Nếu mấy ô đầu hết màu thì chuyển sang ô tiếp theo
                {
                    GetColorHexaFirst(nearHexaObj, k + 1, item);
                    return;
                }
            }
        }
        return;
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
        else if (this._data.State == VisualState.SHOW)
        {
            this._gridHexagonData.State = GridHexagonState.UNLOCK;

            this._data.IsSelected = true;
            this._data.ColorHexa = this._active;
            this._data.State = VisualState.SHOW;
            this._hexaColor = T_Utils.ConvertToColor(this._data.ColorHexa);
        }
        else
        {
            this._gridHexagonData.State = GridHexagonState.UNLOCK;

            this._data.IsSelected = true;
            this._data.ColorHexa = this._active;
            this._data.State = VisualState.EMPTY;
            this._hexaColor = T_Utils.ConvertToColor(this._data.ColorHexa);
        }
    }

    public T_HexaInBoardData GetDataHexa()
    {
        return this._data;
    }

    public void SetDataHexa(T_HexaInBoardData data)
    {
        this._data = data;
    }

    public GridHexagonData GetGridHexagonData()
    {
        return this._gridHexagonData;
    }
}

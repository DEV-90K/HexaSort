using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class T_GridController : MonoBehaviour
{
    public static T_GridController Instance;

    public int Width = 15;
    public int Height = 15;
    public int CellSize = 5;
    public bool CanContact = true;
    public GameObject Hexagon;

    private Grid _grid;
    private Camera _camera;
    private MeshRenderer _modelHexa;
    private List<GameObject> _childHexas; // Các item hexa của hexa được selected
    private List<GameObject> _childsGrid; // Tất cả các hexa khi Init
    private List<T_HexaInBoardObject> _hexaInBoardSelecteds;
    private T_HexaInBoardObject _hexaObjSelected; // Hexa đang được selected ra
    private List<string> _colors; // Ban đầu có bao nhiêu màu
    private List<string> _countItemColors; // Dùng để check xem màu đấy đã dùng hết chưa
    private Dictionary<string, StackHexagonData> _hexaStacks;

    private float _tileXOffset = 1.735f;
    private float _tileZOffset = 1.567f;
    private int _colorNumber;
    private int _hexaNumber;
    private float _location = 0.2f;
    private int _countColor;


    private void Awake()
    {
        Instance = this;

        this._grid = GetComponent<Grid>();
        this._camera = Camera.main;
        this._modelHexa = this.Hexagon.GetComponentInChildren<MeshRenderer>();
        _childHexas = new List<GameObject>();
        this._childsGrid = new List<GameObject>();
        this._hexaInBoardSelecteds = new List<T_HexaInBoardObject>();
        this._countItemColors = new List<string>();
        this._colors = new List<string>();
        this._hexaStacks = new Dictionary<string, StackHexagonData>();
    }
    private void Start()
    {
        this._colorNumber = T_ScreenTool.Instance.GetColorNumber();
        this._hexaNumber = T_ScreenTool.Instance.GetHexaInEachHexaNumber();
    }

    private void LateUpdate()
    {
        if (CanContact == false)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData, 1000, LayerMask.GetMask("HexaTool")))
            {
                GameObject gObj = hitData.transform.gameObject;
                T_HexaInBoardObject hexaObj = gObj.GetComponent<T_HexaInBoardObject>();
                hexaObj.CheckHexaInBoard();
            }
        }
    }

    public void Init(int numberHexa, Dictionary<string, T_HexaInBoardData> hexaObjSelected = null)
    {
        int count = this.transform.childCount;
        if (count > 0) this.DestroyChildGrid();
        this._childsGrid.Clear();
        Vector3 cellCenter = this._grid.CellToWorld(new Vector3Int(1, 0, 0));
        for (int xSwizzle = -CellSize; xSwizzle <= CellSize; xSwizzle++)
        {
            for (int zSwizzle = -CellSize; zSwizzle <= CellSize; zSwizzle++)
            {
                Vector3 cellPos = _grid.CellToWorld(new Vector3Int(xSwizzle, zSwizzle, 0));

                if (cellPos.magnitude > cellCenter.magnitude * CellSize) continue;
                GameObject gObj = Instantiate(Hexagon, transform);
                gObj.name = string.Format("{0}_{1}", xSwizzle, zSwizzle);
                gObj.transform.position = cellPos;
                T_HexaInBoardObject hexaObj = gObj.GetComponent<T_HexaInBoardObject>();
                hexaObj.Init(numberHexa);
                hexaObj.InitData(numberHexa, xSwizzle, zSwizzle);
                if(hexaObjSelected != null)
                {
                    if (hexaObjSelected.ContainsKey(gObj.name))
                    {
                        /*hexaObj.SetDataHexa(hexaObjSelected[gObj.name]);
                        if (hexaObj.GetDataHexa().State == VisualState.SHOW)
                            this.ShowEmptyHexa(hexaObj);*/
                        hexaObj.Init(hexaObjSelected[gObj.name]);
                        this._hexaInBoardSelecteds.Add(hexaObj);
                    }
                }
                this._childsGrid.Add(gObj);
            }
        }
    }

    public void InitChallenge(int numberHexa, int color)
    {
        int count = this.transform.childCount;
        if (count > 0) this.DestroyChildGrid();
        this._childsGrid.Clear();

        this._tileXOffset = 1.8f;
        this._tileZOffset = 1.6f;

        Width = 3;
        Height = 4;
        this._countColor = color;
        int sumHexa = Width * Height, sumStackHexa = color * numberHexa, quantityStackHexaInHexa = sumStackHexa / sumHexa;
        for (int i = 0; i < color; i++)
        {
            this._colors.Add(T_ConfigValue.ColorList[i]);
        }

        //this._camera.transform.position = new Vector3(1, 6, 0);
        this._camera.transform.position = new Vector3(13, 15, 0);
        this._camera.transform.rotation = Quaternion.Euler(new Vector3(40, 0, 0));

        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {
                GameObject hexa = Instantiate(Hexagon, transform);
                hexa.name = string.Format("{0} - {1}", x, z);

                if (z % 2 == 0)
                {
                    hexa.transform.position = new Vector3(x * this._tileXOffset, 0, z * this._tileZOffset);
                }
                else
                {
                    hexa.transform.position = new Vector3(x * this._tileXOffset + this._tileXOffset / 2, 0, z * this._tileZOffset);
                }
                T_HexaInBoardObject hexaObj = hexa.GetComponent<T_HexaInBoardObject>();
                List<T_HexaInBoardObject> nearHexas = new List<T_HexaInBoardObject>();
                if (this._childsGrid.Count > 0)
                {
                    for (int a = 0; a < this._childsGrid.Count; a++)
                    {
                        Transform pos = this._childsGrid[a].transform;
                        Vector3 equals = pos.localPosition - hexa.transform.localPosition;
                        if ((float)System.Math.Round(equals.magnitude, 1) == this._tileXOffset)
                        {
                            T_HexaInBoardObject nearHexaObj = pos.GetComponent<T_HexaInBoardObject>();
                            nearHexas.Add(nearHexaObj);
                            //Debug.Log(string.Format("{0} + {1}", hexaObj, nearHexaObj));
                        }
                    }
                }
                hexaObj.InitChallenge(quantityStackHexaInHexa, this._countColor, nearHexas);
                hexaObj.InitData(numberHexa, x, z);
                this._childsGrid.Add(hexa);
                //this.SetSelectedHexaObj(hexaObj, true);
                nearHexas.Clear();
            }
        }
        this._countItemColors.Clear();
        this._colors.Clear();
    }


    public void SetSelectedHexaObj(T_HexaInBoardObject hexaObj, bool isSlected)
    {
        if (isSlected) this._hexaInBoardSelecteds.Add(hexaObj);
        else this._hexaInBoardSelecteds.Remove(hexaObj);
    }

    public void DestroyChildGrid()
    {
        if(this._childsGrid.Count > 0)
        {
            foreach(var gObj in this._childsGrid)
            {
                Destroy(gObj);
            }
        }
    }

    public void ShowNumberHexaInHexa(T_HexaInBoardObject hexaObj) // Show stack hexa
    {
        GameObject gObjHexa = hexaObj.gameObject;
        int childCount = hexaObj.transform.childCount;
        T_HexaInBoardData hexaData = hexaObj.GetDataHexa();
        if (childCount > 1) this.DestroyChildHexa(this._childHexas);
        for(int i = 0; i < hexaData.HexagonDatas.Length; i++)
        {
            if (hexaData.HexagonDatas[i] != null) //
            {
                GameObject gObj = Instantiate(this._modelHexa.gameObject, gObjHexa.transform);
                gObj.transform.localPosition = new Vector3(0, (i+1) * this._location, 0);
                MeshRenderer model = gObj.GetComponent<MeshRenderer>();
                model.material.color = T_Utils.ConvertToColor(hexaData.HexagonDatas[i].ColorHexa);
                //if(hexaData.HexagonDatas[i] != null) model.material.color = T_Utils.ConvertToColor(hexaData.HexagonDatas[i].ColorHexa);
                this._childHexas.Add(gObj);
            }
        }
    }

    public void DestroyChildHexa(List<GameObject> childHexas)
    {
        if(childHexas.Count > 0)
        {
            foreach (GameObject obj in childHexas)
            {
                Destroy(obj);
            }
            childHexas.Clear();
        }
    }

    public void ShowEmptyHexa(T_HexaInBoardObject hexaObj)
    {
        List<GameObject> children = new List<GameObject>();
        int childCount = hexaObj.transform.childCount;
        if (childCount > 1)
        {
            for(int i = childCount - 1; i > 0; i--)
            {
                children.Add(hexaObj.transform.GetChild(i).gameObject);
            }
            this.DestroyChildHexa(children);
        }
    }

    public List<T_HexaInBoardObject> GetHexaObjsSelected()
    {
        return this._hexaInBoardSelecteds;
    }

    public void SetHexaObjSeleted(T_HexaInBoardObject hexaObj)
    {
        this._hexaObjSelected = hexaObj; 
    }

    public T_HexaInBoardObject GetHexaObjSelected()
    {
        return this._hexaObjSelected;
    }

    public LevelData GetLevelData()
    {
        List<T_HexaInBoardObject> hexaObjSelected = this.GetHexaObjsSelected();
        int count = hexaObjSelected.Count;
        LevelData result = new LevelData();
        GridData gridData = new GridData(new GridHexagonData[count]);
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                T_HexaInBoardObject hexaObj = hexaObjSelected[i];
                T_HexaInBoardData hexaData = hexaObj.GetDataHexa();
                GridHexagonData gridHexagonData = hexaObj.GetGridHexagonData();
                gridData.GridHexagonDatas[i] = gridHexagonData;
                StackHexagonData stackHexagonData = gridData.GridHexagonDatas[i].StackHexagon;
                if ((hexaData.HexagonDatas.Length > hexaObj.transform.childCount && hexaData.State != VisualState.HIDE) || hexaObj.GetDataHexa().HexagonDatas.Length == 0)
                {
                    gridData.GridHexagonDatas[i].UpdateStackHexagonData(null);
                }
                else
                {
                    List<int> idhexa = new List<int>();
                    for (int j = 0; j < hexaData.HexagonDatas.Length; j++)
                    {
                        int idColor = Array.IndexOf(T_ConfigValue.ColorList, hexaData.HexagonDatas[j].ColorHexa);
                        idhexa.Add(idColor);
                    }
                    stackHexagonData.IDHexes = idhexa.ToArray();
                    idhexa.Clear();
                }

            }
            result.UpdateGridData(gridData);
        }
        return result;
    }

    public string AddIdColor(T_HexaInBoardObject hexaObj,int id = -1, string idcolor = null)
    {
        string colorHexa = null;
        if (id != -1)
        {
            if (id >= this._colors.Count) return null;
            colorHexa = this._colors[id];
            //Debug.LogError(string.Format("id: {0} + this._colors.Count: {1} + colorHexa: {2}", id, this._colors.Count, colorHexa));
        }
        else if (!string.IsNullOrEmpty(idcolor))
        {
            colorHexa = this._colors.Where(a => a.Contains(idcolor)).FirstOrDefault();
            if (!string.IsNullOrEmpty(colorHexa)) this._countItemColors.RemoveAt(this._countItemColors.Count - 1);
        }
        if (!string.IsNullOrEmpty(colorHexa)) 
        {
            this._countItemColors.Add(colorHexa);
            if(this._countItemColors.Count > 0)
            {
                int count = this._countItemColors.Where(a => a.Contains(colorHexa)).Count();
                //Debug.LogError(string.Format("hexaObj: {0} + colorHexa: {1} + count: {2} + this._countItemColors.Count: {3}", hexaObj, colorHexa, count, this._countItemColors.Count));
                if (count > 10)
                {
                    this._colors.Remove(colorHexa);
                    this._countColor -= 1;
                    return null;
                }
            }
        }

        return colorHexa;
    }

    public int GetCountColor()
    {
        return this._countColor;
    }

    public bool CheckListColors()
    {
        return this._colors.Count > 0 ? true : false;
    }

    public static Dictionary<T, int> CountElements<T>(IEnumerable<T> array) // Tìm đối tượng trùng lặp trong mảng
    {
        return array.GroupBy(x => x)
                      .ToDictionary(g => g.Key, g => g.Count());
    }

    public void SetUpChallenge()
    {
        int totalHexaSelected = this._hexaInBoardSelecteds.Count, quantityInStack = 5, sameColorHexasToLose = 10;
        int sumHexa = quantityInStack * totalHexaSelected;
        int countcolor = sumHexa / sameColorHexasToLose;
        this._countColor = countcolor;
        this.ResetHexaSelected();
        if (this._colors.Count > 0) this._colors.Clear();
        for (int i = 0; i < this._countColor; i++)
        {
            this._colors.Add(T_ConfigValue.ColorList[i]);
        }
        for(int i = 0; i < totalHexaSelected; i++)
        {
            T_HexaInBoardObject hexaObj = this._hexaInBoardSelecteds[i];
            T_HexaInBoardData hexaData = hexaObj.GetDataHexa();
            hexaData.HexagonDatas = new T_HexaInBoardData[quantityInStack];
            List<T_HexaInBoardObject> neightborHexa = GetNeighborHexaObjSelected(hexaObj);
            hexaObj.InitChallenge(quantityInStack, countcolor, neightborHexa);
            neightborHexa.Clear();
        }
    }

    public void ResetHexaSelected()
    {
        int totalHexaSelected = this._hexaInBoardSelecteds.Count;
        for (int i = 0; i < totalHexaSelected; i++)
        {
            T_HexaInBoardObject hexaObj = this._hexaInBoardSelecteds[i];
            T_HexaInBoardData hexaData = hexaObj.GetDataHexa();
            hexaData.HexagonDatas = new T_HexaInBoardData[0];
        }
    }

    public List<T_HexaInBoardObject> GetNeighborHexaObjSelected(T_HexaInBoardObject hexaObj)
    {
        float radiusHexa = 1.7321f;
        radiusHexa = (float)System.Math.Round(radiusHexa, 1);
        List<T_HexaInBoardObject> neighborHexa = new List<T_HexaInBoardObject>();
        List<T_HexaInBoardObject> hexaObjs = this.GetHexaObjsSelected();
        int totalHexaSelected = hexaObjs.Count;
        if(totalHexaSelected > 0)
        {
            for (int i = 0; i < totalHexaSelected; i++)
            {
                if (hexaObjs[i].name.Contains(hexaObj.name)) 
                    continue;
                T_HexaInBoardData hexaData = hexaObjs[i].GetDataHexa();
                float radius = (hexaObjs[i].transform.localPosition - hexaObj.transform.localPosition).magnitude;
                //Debug.LogError(string.Format("hexaObj: {0} + hexaObj: {1} + hexaObjs[{2}]: {3}", (float)System.Math.Round(radius, 1), hexaObj, i, hexaObjs[i]));
                if((float)System.Math.Round(radius, 1) == radiusHexa && hexaData.HexagonDatas.Length > 0)
                {
                    neighborHexa.Add(hexaObjs[i]);
                }
            }
        }
        return neighborHexa;
    }

    public void SetStackHexa(T_HexaInBoardObject hexaObj)
    {
        StackHexagonData stackData = new StackHexagonData();
        List<int> idHexas = new List<int>();
        T_HexaInBoardData hexaData = hexaObj.GetDataHexa();
        for(int i = 0; i < hexaData.HexagonDatas.Length; i++)
        {
            int idColor = Array.IndexOf(T_ConfigValue.ColorList, hexaData.HexagonDatas[i].ColorHexa);
            idHexas.Add(idColor);
        }
        stackData.IDHexes = idHexas.ToArray();
        this._hexaStacks[hexaObj.name] = stackData;
        //Debug.Log(this._hexaStacks.Count);
        this.ShowEmptyHexa(hexaObj);
    }

    public StackQueueData GetStackQueueData()
    {
        StackQueueData stackQueueData = new StackQueueData(new StackHexagonData[this._hexaStacks.Count]);
        for(int i = 0; i < this._hexaStacks.Count; i++)
        {
            StackHexagonData data = this._hexaStacks.Values.ElementAt(i);
            stackQueueData.StackHexagonDatas[i] = data;
        }
        return stackQueueData;
    }

    internal Dictionary<string, T_HexaInBoardData> GetHexaObjsSelectedData()
    {
        Dictionary<string, T_HexaInBoardData> list = new Dictionary<string, T_HexaInBoardData>();

        foreach(T_HexaInBoardObject obj in _hexaInBoardSelecteds)
        {
            string name = obj.gameObject.name;
            T_HexaInBoardData data = obj.GetDataHexa();
            list[name] = data;
        }

        return list.CopyObject();
    }
}

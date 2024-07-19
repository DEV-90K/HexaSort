using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    }
    private void Start()
    {
        this._colorNumber = T_ScreenTool.Instance.GetColorNumber();
        this._hexaNumber = T_ScreenTool.Instance.GetHexaInEachHexaNumber();
        //this.Init();
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

    public void Init(int numberHexa)
    {
        int count = this.transform.childCount;
        if (count > 0) this.DestroyChildGrid();
        this._childsGrid.Clear();
        Vector3 cellCenter = this._grid.CellToWorld(new Vector3Int(1, 0, 0));
        for (int xSwizzle = -1; xSwizzle <= 1; xSwizzle++)
        {
            for (int zSwizzle = -2; zSwizzle <= 1; zSwizzle++)
            {
                Vector3 cellPos = _grid.CellToWorld(new Vector3Int(xSwizzle, zSwizzle, 0));

                if (cellPos.magnitude > cellCenter.magnitude * CellSize) continue;
                GameObject gObj = Instantiate(Hexagon, transform);
                gObj.name = string.Format("{0}_{1}", xSwizzle, zSwizzle);
                gObj.transform.position = cellPos;
                T_HexaInBoardObject hexaObj = gObj.GetComponent<T_HexaInBoardObject>();
                hexaObj.Init(numberHexa);
                hexaObj.InitData(numberHexa, xSwizzle, zSwizzle);
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

        this._camera.transform.position = new Vector3(1, 6, 0);
        this._camera.transform.rotation = Quaternion.Euler(new Vector3(40, 0, 0));

        //this._camera.transform.position = new Vector3(12.5f, 19, 0);
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
                        }
                    }
                }
                hexaObj.InitChallenge(quantityStackHexaInHexa, this._countColor, nearHexas);
                hexaObj.InitData(numberHexa, x, z);
                this._childsGrid.Add(hexa);
                this.SetSelectedHexaObj(hexaObj, true);
                nearHexas.Clear();
            }
        }
        this._countItemColors.Clear();
        this._colors.Clear();
    }

    public void InitDemo(int numberHexa, int color)
    {
        Width = 3;
        Height = 4;
        int sumHexa = Width * Height, sumStackHexa = color * numberHexa, quantityStackHexaInHexa = sumStackHexa / sumHexa;

        this._camera.transform.position = new Vector3(1, 6, 0);
        this._camera.transform.rotation = Quaternion.Euler(new Vector3(40, 0, 0));

        //this._camera.transform.position = new Vector3(12.5f, 19, 0);
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
                T_HexaInBoardObject currenthexa = this._hexaInBoardSelecteds.Where(a => a.name.Contains(hexa.name)).FirstOrDefault();
                hexaObj.InitDemo(currenthexa.GetDataHexa());
            }
        }

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
            GameObject gObj = Instantiate(this._modelHexa.gameObject, gObjHexa.transform);
            gObj.transform.localPosition = new Vector3(0, (i+1) * this._location, 0);
            MeshRenderer model = gObj.GetComponent<MeshRenderer>();
            model.material.color = T_Utils.ConvertToColor(hexaData.HexagonDatas[i].ColorHexa);
            //if(hexaData.HexagonDatas[i] != null) model.material.color = T_Utils.ConvertToColor(hexaData.HexagonDatas[i].ColorHexa);
            this._childHexas.Add(gObj);
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
                 //   Debug.Log(colorHexa);
                    this._colors.Remove(colorHexa);
                  //  Debug.LogError(string.Format("this._colors: {0}", this._colors.Count));
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
}

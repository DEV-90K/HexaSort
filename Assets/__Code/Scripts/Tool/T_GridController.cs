using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;
using static UnityEditor.FilePathAttribute;

public class T_GridController : MonoBehaviour
{
    public static T_GridController Instance;

    public int Width = 5;
    public int Height = 5;
    public int CellSize = 5;
    public bool CanContact = true;
    public GameObject Hexagon;

    public int ColorChallenge = 0;

    private Grid _grid;
    private Camera _camera;
    private MeshRenderer _modelHexa;
    private List<GameObject> _childHexas; // Các item hexa của hexa được selected
    private List<GameObject> _childsGrid; // Tất cả các hexa khi Init
    private List<GameObject> _childsGridChallenge;
    private List<T_HexaInBoardObject> _hexaInBoardSelecteds;
    private T_HexaInBoardObject _hexaObjSelected; // Hexa đang được selected ra

    private float _tileXOffset = 1.735f;
    private float _tileZOffset = 1.567f;
    private int _colorNumber;
    private int _hexaNumber;
    private float _location = 0.2f;


    private void Awake()
    {
        Instance = this;

        this._grid = GetComponent<Grid>();
        this._camera = Camera.main;
        this._modelHexa = this.Hexagon.GetComponentInChildren<MeshRenderer>();
        _childHexas = new List<GameObject>();
        this._childsGrid = new List<GameObject>();
        this._childsGridChallenge = new List<GameObject>();
        this._hexaInBoardSelecteds = new List<T_HexaInBoardObject>();
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
        this.ColorChallenge = 0;
        CellSize = 1; // Challenge
        int count = this.transform.childCount;
        if (count > 0) this.DestroyChildGrid();
        this._childsGrid.Clear();
        //this._childsGridChallenge.Clear();
        Vector3 cellCenter = this._grid.CellToWorld(new Vector3Int(1, 0, 0));
        for (int xSwizzle = -CellSize; xSwizzle <= CellSize; xSwizzle++)
        {
            for (int zSwizzle = -CellSize; zSwizzle <= CellSize; zSwizzle++)
            {
                Vector3 cellPos = _grid.CellToWorld(new Vector3Int(xSwizzle, zSwizzle, 0));

                //Debug.Log(string.Format("{0}_{1}", new Vector3Int(xSwizzle, zSwizzle, 0), cellPos));
                if (cellPos.magnitude > cellCenter.magnitude * CellSize)
                { 
                    Debug.Log(string.Format("{0}_{1}", new Vector3Int(xSwizzle, zSwizzle, 0), cellPos));
                    continue;
                }
                GameObject gObj = Instantiate(Hexagon, transform);
                gObj.name = string.Format("{0}_{1}", xSwizzle, zSwizzle);
                gObj.transform.position = cellPos;
                T_HexaInBoardObject hexaObj = gObj.GetComponent<T_HexaInBoardObject>();
                hexaObj.Init(numberHexa);
                hexaObj.InitData(numberHexa, xSwizzle, zSwizzle);
                this._childsGrid.Add(gObj);
            }
        }
        //this._childsGridChallenge = this._childsGrid;
        //this.RandomGridChallenge(this._childsGridChallenge);

        /*this._camera.transform.position = new Vector3(12, 30, 10);
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {
                GameObject hexa = Instantiate(Hexagon, transform);
                hexa.name = string.Format("{0} - {1}", x, z);

                if (z % 2 == 0)
                {
                    hexa.transform.position = new Vector3(x * this._tileXOffset, 0, z * 1.5f);
                }
                else
                {
                    hexa.transform.position = new Vector3(x * this._tileXOffset + this._tileXOffset / 2, 0, z * 1.5f);
                }
            }
        }*/
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

    public void RandomGridChallenge(List<GameObject> childs)
    {
        int count = childs.Count;
        if (count == 0)
            return;
        GameObject gObj = childs[count - 1];
        T_HexaInBoardObject hexaObj = gObj.GetComponent<T_HexaInBoardObject>();
        hexaObj.RandomItemHexaChallenge(count - 1);
        childs.RemoveAt(count - 1);
        RandomGridChallenge(childs);
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
}

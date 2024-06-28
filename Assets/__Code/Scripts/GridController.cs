using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityUtils;

//#if UNITY_EDITOR
using UnityEngine;
using Newtonsoft.Json;

public interface IGridPortability
{
    public Vector3Int ConvertToGridPos(Vector3 worldPos);
    public Vector3 ConvertToWorldPos(Vector3Int gridPos);
}

public class GridController : MonoBehaviour, IGridPortability //Only using inside of the Editor. At product only load Prefab Level contain Grid
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private int gridSize = 2;    
    [SerializeField]
    private Transform hexagon;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Start()
    {
        //Generating();
        //Generating(ResourceManager.Instance.GetLevelByID());
        //string data = 'fsfs';
        LevelData levelData = ResourceManager.Instance.GetLevelByID();
        Generating(levelData);
    }

    private void Generating(LevelData levelData)
    {
        Debug.Log("Generating: ");
        levelData.DebugLogObject();
        GridHexagonData[] gridHexagonDatas = levelData.Grid.GridHexagonDatas;
        transform.DestroyChildrenImmediate();

        for (int i = 0; i < gridHexagonDatas.Length; i++)
        {
            Debug.Log("Grid: " + i);
            //Vector3 cellPos = grid.CellToWorld(new Vector3Int(gridHexagon.Row, gridHexagon.Column, 0));
            GridHexagon instance = PoolManager.Spawn<GridHexagon>(PoolType.GRID_HEXAGON, Vector3.zero, Quaternion.identity);
            instance.transform.SetParent(transform);
            instance.OnInitialize(gridHexagonDatas[i], this);
        }
    }

    private void Generating()
    {
        List<GridHexagonData> gridHexagonDatas = new List<GridHexagonData>();

        transform.DestroyChildrenImmediate();
        Vector3 cellCenter = grid.CellToWorld(new Vector3Int(1, 0, 0));
        for(int xSwizzle = -gridSize; xSwizzle <= gridSize; xSwizzle++)
        {
            for(int zSwizzle = -gridSize; zSwizzle <= gridSize; zSwizzle++)
            {
                Vector3 cellPos = grid.CellToWorld(new Vector3Int(xSwizzle, zSwizzle, 0));

                if(cellPos.magnitude > cellCenter.magnitude * gridSize)
                {
                    continue;
                }

                //1
                //Transform instance = Instantiate(hexagon, transform);
                //instance.position = cellPos;
                GridHexagon instance = PoolManager.Spawn<GridHexagon>(PoolType.GRID_HEXAGON, cellPos, Quaternion.identity);
                instance.transform.SetParent(transform);

                gridHexagonDatas.Add(new GridHexagonData(GridHexagonState.UNLOCK, xSwizzle, zSwizzle, "#c4c4c4" ,null));

                //2
                //GameObject gridCellIns = (GameObject)PrefabUtility.InstantiatePrefab(hexagon);
                //gridCellIns.transform.SetParent(transform);
                //gridCellIns.transform.position = cellPos;
                //gridCellIns.transform.rotation = Quaternion.identity;
            }
        }

        GridData gridData = new GridData(gridHexagonDatas.ToArray());
        LevelData levelData = new LevelData(gridData);
        levelData.DebugLogObject<LevelData>();
        levelData.CopyObject<LevelData>().DebugLogObject<LevelData>();
    }

    public Vector3Int ConvertToGridPos(Vector3 worldPos)
    {
        return grid.WorldToCell(worldPos);
    }

    public Vector3 ConvertToWorldPos(Vector3Int gridPos)
    {
        return grid.CellToWorld(gridPos);
    }
}
//#endif

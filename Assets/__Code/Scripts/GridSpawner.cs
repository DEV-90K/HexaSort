using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class GridSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform hexagon;

    IGridPortability iGrid;

    public void OnInit(IGridPortability iGrid)
    {
        this.iGrid = iGrid;
    }

    public GridHexagon[] Spawn(GridData grid)
    {
        GridHexagonData[] gridHexagonDatas = grid.GridHexagonDatas;
        GridHexagon[] gridHexagons = new GridHexagon[gridHexagonDatas.Length];
        for (int i = 0; i < gridHexagonDatas.Length; i++)
        {
            GridHexagon instance = PoolManager.Spawn<GridHexagon>(PoolType.GRID_HEXAGON, Vector3.zero, Quaternion.identity);
            instance.OnInitialize(gridHexagonDatas[i], iGrid);
            gridHexagons[i] = instance;
        }

        return gridHexagons;
    }
}

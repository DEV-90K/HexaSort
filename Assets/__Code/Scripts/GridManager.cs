using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mul21_Lib;

public interface IGridPortability
{
    public Vector3Int ConvertToGridPos(Vector3 worldPos);
    public Vector3 ConvertToWorldPos(Vector3Int gridPos);
}

public class GridManager : MonoBehaviour, IGridPortability
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private GridController _gridControl;
    [SerializeField]
    private GridSpawner _gridSpawner;

    private GridData _gridData;
    private GridHexagon[] _gridHexagons;

    protected void Awake()
    {
        _gridSpawner.OnInit(this);
    }

    public void OnInit(GridData gridData)
    {
        _gridData = gridData;

        GridHexagon[] gridHexagons = _gridSpawner.Spawn(_gridData);
        _gridHexagons = gridHexagons;
        _gridControl.OnInit(gridHexagons);
    }

    public GridHexagon[] GetGridHexagonContainStack()
    {
        List<GridHexagon> gridHexagons = new List<GridHexagon> ();

        foreach (GridHexagon grid in _gridHexagons)
        {
            if(grid.CheckOccupied())
            {
                gridHexagons.Add(grid);
            }
        }

        return gridHexagons.ToArray();
    }

    public void CollectOccupied()
    {
        _gridControl.Collect();
    }

    public void CollectGridImmediate()
    {
        for (int i = 0; i < _gridHexagons.Length; i++)
        {
            _gridHexagons[i].CollectImmediate();
        }

        _gridHexagons = null;
    }

    #region Impl IGridPortability
    public Vector3Int ConvertToGridPos(Vector3 worldPos)
    {
        return grid.WorldToCell(worldPos);
    }

    public Vector3 ConvertToWorldPos(Vector3Int gridPos)
    {
        return grid.CellToWorld(gridPos);
    }
    #endregion Impl IGridPortability
}

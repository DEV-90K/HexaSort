using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mul21_Lib;
using System;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private GridUnit _GridUnit;
    [SerializeField]
    private GridController _gridControl;
    [SerializeField]
    private GridSpawner _gridSpawner;

    private GridData _gridData;
    private GridHexagon[] _gridHexagons;

    protected void Awake()
    {
        _gridSpawner.OnInit(_GridUnit);
    }

    public void OnInit(GridData gridData)
    {
        _gridData = gridData;

        GridHexagon[] gridHexagons = _gridSpawner.Spawn(_gridData);
        _gridHexagons = gridHexagons;
        _gridControl.OnInit(gridHexagons);
        _GridUnit.OnInit();
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

    public bool CheckEmptyGrid()
    {
        for(int i = 0; i < _gridHexagons.Length; i++)
        {
            if (_gridHexagons[i].CheckOccupied() == false)
            {
                return true;
            }
        }

        return false;
    }

    #region Grid Hexagon Data
    internal GridHexagonData[] GetCurrentGridPlayingData()
    {
        List<GridHexagonData> datas = new List<GridHexagonData> ();

        foreach (GridHexagon grid in _gridHexagons)
        {
            GridHexagonData data = grid.GetCurrentGridHexagonPlayingData();
            datas.Add(data);
        }

        return datas.ToArray();
    }
    #endregion Grid Hexagon Data
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static Action<float> OnInitCompleted;

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

        float radius = GetMaxRadius();
        Debug.Log("Radius: " + radius);
        OnInitCompleted?.Invoke(radius);
    }

    private float GetMaxRadius()
    {
        float radius = 0;
        Vector2 center = _GridUnit.transform.position.With(y: 0);
        foreach (GridHexagon gridData in _gridHexagons)
        {            
            Vector2 pos = gridData.transform.position.With(y: 0);
            float newRadis = Vector2.Distance(center, pos);
            if (radius < newRadis) 
                radius = newRadis;            
        }

        return radius;
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
        _gridControl.CollectAllOccupied();
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

    public GridHexagon[] GetGridHexagonsUnLockByHexagon(int amount)
    {
        return _gridControl.UnLockByHexagon(amount);
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

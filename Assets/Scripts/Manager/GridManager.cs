using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoSingleton<GridManager>
{
    public static Action<float> OnInitCompleted;

    [SerializeField]
    private GridUnit _GridUnit;
    [SerializeField]
    private GridSpawner _gridSpawner;

    private GridData _gridData;
    private GridHexagon[] _gridHexagons;

    private List<GridHexagon> _gridCollects;
    private List<GridHexagon> _gridLocks;


    protected override void Awake()
    {
        base.Awake();
        _gridSpawner.OnInit(_GridUnit);
    }

    public void CollectOccupied()
    {
        CollectAllOccupied();
    }

    private void CollectAllOccupied()
    {
        _gridCollects = new List<GridHexagon>();
        foreach (GridHexagon grid in _gridHexagons)
        {
            if (grid.CheckOccupied())
            {
                _gridCollects.Add(grid);
                grid.StackOfCell.CollectPlayerHexagon(() => OnCollectCompleted(grid));
            }
        }

        if (_gridCollects.Count == 0)
        {
            this.Invoke(() => CollectAllImmediate(), 1f);
        }
    }

    private void CollectAllImmediate()
    {
        foreach (GridHexagon grid in _gridHexagons)
        {
            grid.CollectImmediate();
        }
    }

    private void OnCollectCompleted(GridHexagon gridHexagon)
    {
        _gridCollects.Remove(gridHexagon);

        if (_gridCollects.Count <= 0)
        {
            foreach (GridHexagon grid in _gridHexagons)
            {
                grid.CollectImmediate();
            }

            _gridCollects.Clear();
            _gridHexagons = null;
        }
    }

    private GridHexagon[] UnLockByHexagon(int amount)
    {
        List<GridHexagon> gridHexagons = new List<GridHexagon>();

        for (int i = _gridLocks.Count - 1; i >= 0; i--)
        {
            if (_gridLocks[i].CheckUnLockByHexagon(amount))
            {
                _gridLocks[i].OnUnLock();
                gridHexagons.Add(_gridLocks[i]);
                _gridLocks.RemoveAt(i);
            }
        }

        return gridHexagons.ToArray();
    }

    public void OnInit(GridData gridData)
    {
        _gridData = gridData;
        _gridHexagons = _gridSpawner.Spawn(_gridData);
        _gridLocks = GetGridHexagonLock();
        _GridUnit.OnInit();

        OnInitCompleted?.Invoke(GetMaxRadius());
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

    private List<GridHexagon> GetGridHexagonLock()
    {
        List<GridHexagon> list = new List<GridHexagon>();
        foreach (GridHexagon grid in _gridHexagons)
        {
            if (grid.State == GridHexagonState.LOCK_BY_GOAL || grid.State == GridHexagonState.LOCK_BY_ADS)
            {
                list.Add(grid);
            }
        }

        return list;
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
        return UnLockByHexagon(amount);
    }

    public GridHexagon[] GetGridHexagonContainStack()
    {
        List<GridHexagon> gridHexagons = new List<GridHexagon>();

        foreach (GridHexagon grid in _gridHexagons)
        {
            if (grid.CheckOccupied())
            {
                gridHexagons.Add(grid);
            }
        }

        return gridHexagons.ToArray();
    }

    private GridHexagon GetRandomGridHexagon()
    {
        int idx = UnityEngine.Random.Range(0, _gridHexagons.Length);
        return _gridHexagons[idx];
    }

    public GridHexagon GetRandomGridHexagonContainStack()
    {
        GridHexagon grid = null;
        int max = _gridHexagons.Length;
        while (max > 0)
        {
            max--;
            grid = GetRandomGridHexagon();
            if(grid.CheckOccupied())
            {
                break;
            }
        }

        return grid;
    }

    public GridHexagon[] GetGridHexagonByRadius(Vector3 centerPoint, float radius)
    {
        return null;
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

    public List<GridHexagon> GetGridHexagonByTopColor(Color color)
    {
        List<GridHexagon> list = new List<GridHexagon>();

        foreach (GridHexagon grid in _gridHexagons)
        {
            if(grid.CheckOccupied())
            {
                StackHexagon stack = grid.StackOfCell;
                if(ColorUtils.ColorEquals(color, stack.GetTopHexagonColor()))
                {
                    list.Add(grid);
                }
            }
        }

        return list;
    }
    #endregion Grid Hexagon Data
}

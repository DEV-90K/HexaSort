using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoSingleton<GridManager>
{
    public static Action<float> OnInitCompleted;

    [SerializeField]
    private LayerMask gridHexagonLayerMask;
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

    public GridHexagon[] GetGridHexagonNotContainStack()
    {
        List<GridHexagon> gridHexagons = new List<GridHexagon>();

        foreach (GridHexagon grid in _gridHexagons)
        {
            if (!grid.CheckOccupied())
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
    
    public GridHexagon[] GetGridHexagonsContainColor(Color color)
    {
        List<GridHexagon> grids = new List<GridHexagon>();
        foreach (GridHexagon grid in _gridHexagons)
        {
            if(grid.CheckOccupied() && grid.StackOfCell.CheckContainColor(color))
            {
                grids.Add(grid);
            }
        }

        return grids.ToArray();
    }

    public bool CheckCanPlaceStack(GridHexagon gridHexagon, StackHexagon stackHexagon)
    {
        Color topColor = stackHexagon.GetTopHexagonColor();
        List<GridHexagon> neighbors = GetNeighborGidHexagon(gridHexagon);
        if(neighbors.Count == 0)
        {
            return false;
        }

        List<GridHexagon> neighborsSameTop = GetHexagonStackOfNeighborSameTopColor(neighbors, topColor);
        return neighborsSameTop.Count > 0 ? true : false;
    }

    #region Legancy
    public List<GridHexagon> GetHexagonStackOfNeighborSameTopColor(List<GridHexagon> listNeighborGridHexagons, Color topColorOfStackAtGridHexagon)
    {
        //Get List Neighbor have stack have same top color
        List<GridHexagon> neighborGridHexagonSameTopColor = new List<GridHexagon>();
        foreach (GridHexagon neighborGridHexagon in listNeighborGridHexagons)
        {
            Color color1 = neighborGridHexagon.StackOfCell.GetTopHexagonColor();
            Color color2 = topColorOfStackAtGridHexagon;
            if (ColorUtils.ColorEquals(color1, color2))
            {
                neighborGridHexagonSameTopColor.Add(neighborGridHexagon);
            }
        }

        return neighborGridHexagonSameTopColor;
    }

    public List<GridHexagon> GetNeighborGidHexagon(GridHexagon gridHexagon)
    {
        //float distance = _IStackSphereRadius.GetRadiusByGrid().x * 2;
        float distance = _GridUnit.transform.localScale.x * 2f;
        Collider[] neighborGridCellColliders = Physics.OverlapSphere(gridHexagon.transform.position, distance, gridHexagonLayerMask);

        //Get A list of neighbor grid hexagon, that are occupied
        List<GridHexagon> listNeighborGridHexagons = new List<GridHexagon>();
        foreach (Collider collider in neighborGridCellColliders)
        {
            GridHexagon neighborGridHexagon = collider.GetComponent<GridHexagon>();

            if (neighborGridHexagon.CheckOccupied() == false)
            {
                continue;
            }
            if (neighborGridHexagon == gridHexagon)
            {
                continue;
            }

            if (neighborGridHexagon.State == GridHexagonState.LOCK_BY_GOAL || neighborGridHexagon.State == GridHexagonState.LOCK_BY_ADS)
                continue;

            listNeighborGridHexagons.Add(neighborGridHexagon);
        }

        return listNeighborGridHexagons;
    }

    public List<GridHexagon> GetNeighborGridHexagonHaveOneSimilarColor(List<GridHexagon> neighborGridHexagonSameTopColor)
    {
        List<GridHexagon> gridHexagons = new List<GridHexagon>();
        foreach (GridHexagon gridHexagon in neighborGridHexagonSameTopColor)
        {
            if (gridHexagon.StackOfCell.GetNumberSimilarColor() == 1)
            {
                gridHexagons.Add(gridHexagon);
            }
        }

        return gridHexagons;
    }

    public List<GridHexagon> GetNeighborGridHexagonHaveThanOneSimilarColor(List<GridHexagon> neighborGridHexagonSameTopColor)
    {
        List<GridHexagon> gridHexagons = new List<GridHexagon>();
        foreach (GridHexagon gridHexagon in neighborGridHexagonSameTopColor)
        {
            if (gridHexagon.StackOfCell.GetNumberSimilarColor() > 1)
            {
                gridHexagons.Add(gridHexagon);
            }
        }

        return gridHexagons;
    }

    public bool CheckContainGrid(List<GridHexagon> gridHexagons, GridHexagon grid)
    {
        for (int i = 0; i < gridHexagons.Count; i++)
        {
            GridHexagon grid1 = gridHexagons[i];

            if (grid1.gameObject.CompareObject(grid.gameObject))
            {
                return true;
            }
        }

        return false;
    }

    public List<Hexagon> GetPlayerHexagonNeedMerge(Color topColorOfStackAtGridHexagon, List<GridHexagon> neighborGridHexagonSameTopColor)
    {
        List<Hexagon> listPlayerHexagonMerge = new List<Hexagon>();
        foreach (GridHexagon gridHex in neighborGridHexagonSameTopColor)
        {
            StackHexagon hexagonStack = gridHex.StackOfCell;
            for (int i = hexagonStack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon playerHexagon = hexagonStack.Hexagons[i];

                if (ColorUtils.ColorEquals(playerHexagon.Color, topColorOfStackAtGridHexagon))
                {
                    listPlayerHexagonMerge.Add(playerHexagon);
                    playerHexagon.SetParent(null);
                }
            }
        }

        return listPlayerHexagonMerge;
    }

    public void RemovePlayerHexagonFromOldStack(List<GridHexagon> neighborGridHexagonSameTopColor, List<Hexagon> listPlayerHexagonMerge)
    {
        //Remove Hexagon need merge from Hexagon Stack contain before
        foreach (GridHexagon gridHex in neighborGridHexagonSameTopColor)
        {
            StackHexagon hexagonStack = gridHex.StackOfCell;

            foreach (Hexagon playerHexagon in listPlayerHexagonMerge)
            {
                if (hexagonStack.CheckContainPlayerHexagon(playerHexagon))
                {
                    hexagonStack.RemovePlayerHexagon(playerHexagon);
                }
            }
        }
    }

    public IEnumerator IE_MergePlayerHexagonsToStack(StackHexagon stackHexagon, List<GridHexagon> neighborGridHexagon)
    {
        List<Hexagon> listPlayerHexagonMerge = GetPlayerHexagonNeedMerge(stackHexagon.GetTopHexagonColor(), neighborGridHexagon);
        RemovePlayerHexagonFromOldStack(neighborGridHexagon, listPlayerHexagonMerge);
        MergePlayerHexagon(stackHexagon, listPlayerHexagonMerge);
        stackHexagon.HideCanvas();
        yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (listPlayerHexagonMerge.Count - 1) * GameConstants.HexagonConstants.TIME_DELAY);
        stackHexagon.ShowCanvas();
    }

    private void MergePlayerHexagon(StackHexagon stackHexagon, List<Hexagon> listPlayerHexagonMerge)
    {
        float yOfCurrentGridHexagon = (stackHexagon.Hexagons.Count - 1) * GameConstants.HexagonConstants.HEIGHT;
        for (int i = 0; i < listPlayerHexagonMerge.Count; i++)
        {
            Hexagon playerHexagon = listPlayerHexagonMerge[i];
            stackHexagon.AddPlayerHexagon(playerHexagon);

            float yOffset = yOfCurrentGridHexagon + (i + 1) * GameConstants.HexagonConstants.HEIGHT;
            Vector3 localPos = Vector3.up * yOffset;
            playerHexagon.Configure(stackHexagon);
            playerHexagon.MoveToGridHexagon(localPos, i * GameConstants.HexagonConstants.TIME_DELAY);
        }
    }

    #endregion Legancy

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

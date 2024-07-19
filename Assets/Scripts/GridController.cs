using System.Collections.Generic;

//#if UNITY_EDITOR
using UnityEngine;
using Newtonsoft.Json;

public class GridController : MonoBehaviour //Only using inside of the Editor. At product only load Prefab Level contain Grid
{
    private GridHexagon[] gridHexagons;
    private List<GridHexagon> gridCollects;
    private List<GridHexagon> gridLocks;

    public void OnInit(GridHexagon[] gridHexagons)
    {
        this.gridHexagons = gridHexagons;   
        
        this.gridLocks = new List<GridHexagon>();
        foreach (GridHexagon grid in this.gridHexagons)
        {
            if(grid.State == GridHexagonState.LOCK_BY_GOAL || grid.State == GridHexagonState.LOCK_BY_ADS)
            {
                gridLocks.Add(grid);
            }
        }
    }  
    
    public void OnResert()
    {
        gridHexagons = null;
        gridCollects = null;
        gridLocks = null;
    }

    public void CollectAllOccupied()
    {
        gridCollects = new List<GridHexagon>();
        foreach (GridHexagon grid in gridHexagons)
        {
            if (grid.CheckOccupied())
            {
                gridCollects.Add(grid);
                grid.StackOfCell.CollectPlayerHexagon(() => OnCollectCompleted(grid));
            }
        }

        if(gridCollects.Count == 0)
        {
            this.Invoke(() => collectAllImmediate(), 1f);
        }
    }

    public GridHexagon[] UnLockByHexagon(int amount)
    {
        List<GridHexagon> gridHexagons = new List<GridHexagon>();

        for(int i = gridLocks.Count - 1; i >= 0; i--)
        {
            if (gridLocks[i].CheckUnLockByHexagon(amount))
            {
                gridLocks[i].OnUnLock();
                gridHexagons.Add(gridLocks[i]);
                gridLocks.RemoveAt(i);
            }
        }

        return gridHexagons.ToArray();
    }

    private void OnCollectCompleted(GridHexagon gridHexagon)
    {
        gridCollects.Remove(gridHexagon);

        if (gridCollects.Count <= 0)
        {
            foreach (GridHexagon grid in gridHexagons)
            {
                grid.CollectImmediate();
                Debug.Log("Grid Collect Immediate");
            }
            
            OnResert();
        }
    }

    private void collectAllImmediate()
    {
        foreach (GridHexagon grid in gridHexagons)
        {
            grid.CollectImmediate();
        }
    }
}
//#endif

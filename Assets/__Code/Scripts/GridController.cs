using System.Collections.Generic;

//#if UNITY_EDITOR
using UnityEngine;
using Newtonsoft.Json;

public class GridController : MonoBehaviour //Only using inside of the Editor. At product only load Prefab Level contain Grid
{
    private GridHexagon[] gridHexagons;
    private List<GridHexagon> gridCollects;

    public void OnInit(GridHexagon[] gridHexagons)
    {
        this.gridHexagons = gridHexagons;        
    }  
    
    public void OnResert()
    {
        gridHexagons = null;
        gridCollects = null;
    }

    public void Collect()
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
    }

    private void OnCollectCompleted(GridHexagon gridHexagon)
    {
        gridCollects.Remove(gridHexagon);

        if (gridCollects.Count <= 0)
        {
            foreach (GridHexagon grid in gridHexagons)
            {
                grid.CollectImmediate();
            }
            
            OnResert();
        }
    }
}
//#endif

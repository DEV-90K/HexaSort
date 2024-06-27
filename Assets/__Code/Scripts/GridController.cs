using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityUtils;

//#if UNITY_EDITOR
using UnityEngine;

public class GridController : MonoBehaviour //Only using inside of the Editor. At product only load Prefab Level contain Grid
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
        Generating();
    }

    private void Generating()
    {
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
                Transform instance = Instantiate(hexagon, transform);
                instance.position = cellPos;

                //2
                //GameObject gridCellIns = (GameObject)PrefabUtility.InstantiatePrefab(hexagon);
                //gridCellIns.transform.SetParent(transform);
                //gridCellIns.transform.position = cellPos;
                //gridCellIns.transform.rotation = Quaternion.identity;
            }
        }
    }
}
//#endif

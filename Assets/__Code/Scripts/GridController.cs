using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class GridController : MonoBehaviour
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

                Transform instance = Instantiate(hexagon, transform);
                instance.position = cellPos;
            }
        }
    }
}

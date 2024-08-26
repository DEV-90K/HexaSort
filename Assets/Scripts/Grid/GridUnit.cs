using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public interface IGridPortability
{
    public Vector3Int ConvertToGridPos(Vector3 worldPos);
    public Vector3 ConvertToWorldPos(Vector3Int gridPos);
}
public class GridUnit : MonoBehaviour, IGridPortability
{    
    [SerializeField]
    private Grid _grid;
    [SerializeField]
    private Transform _GridContainer;
    [SerializeField]
    private SwipeRotate _SwipeRotate;
    [SerializeField]
    private SwipeScale _SwipeScale;
    [SerializeField]
    private Transform[] _Followeres;
    [SerializeField]
    private LayerMask GridLayer;

    private void OnEnable()
    {
        StackController.OnStackMoving += StackController_OnStackMoving;
        PopupBoostHammer.OnStackMoving += StackController_OnStackMoving;
        PopupBoostSwap.OnStackMoving += StackController_OnStackMoving;
    }

    private void OnDisable()
    {
        StackController.OnStackMoving -= StackController_OnStackMoving;
        PopupBoostHammer.OnStackMoving -= StackController_OnStackMoving;
        PopupBoostSwap.OnStackMoving -= StackController_OnStackMoving;
    }

    private void StackController_OnStackMoving(bool isEnable)
    {
        _SwipeRotate.enabled = isEnable;
        _SwipeScale.enabled = isEnable;
    }

    private void Start()
    {
        _SwipeRotate.enabled = true;
        _SwipeScale.enabled = true;
        _SwipeScale.Register(_Followeres);
    }

    public void OnInit()
    {
        _SwipeRotate.OnInit();
        _SwipeScale.OnInit();
    }

    public void OnSetup(GridData gridData)
    {   
        if(gridData == null)
        {            
            return;
        }
        //_GridContainer.DestroyChildrenImmediate();
        _GridContainer.localPosition = Vector3.zero;

        int maxZ = -100;
        int minZ = 100;

        int maxX = -100;
        int minX = 100;

        for (int i = 0; i < gridData.GridHexagonDatas.Length; i++)
        {
            GridHexagonData gridHexagon = gridData.GridHexagonDatas[i];
            int col = gridHexagon.Row;
            int row = gridHexagon.Column;

            if (row > maxZ)
            {
                maxZ = row;
            }


            if (gridHexagon.Column < minZ) { minZ = row; }
            if (col > maxX) { maxX = col; }
            if (col < minX) { minX = col; }
        }

        Debug.Log($"maxZ: {maxZ}, minZ: {minZ}, maxX: {maxX}, minX: {minX}");

        Vector3 topLeft = _grid.CellToWorld(new Vector3Int(minX, maxZ, 0));
        Debug.Log("topLeft: " + _grid.WorldToCell(topLeft) + " " + topLeft.ToString());
        Vector3 bottomLeft = _grid.CellToWorld(new Vector3Int(minX, minZ, 0));
        Debug.Log("bottomLeft: " + _grid.WorldToCell(bottomLeft) + " " + bottomLeft.ToString());
        Vector3 bottomRight = _grid.CellToWorld(new Vector3Int(maxX, minZ, 0));
        Debug.Log("bottomRight: " + _grid.WorldToCell(bottomRight) + " " + bottomRight.ToString());

        Debug.Log(_grid.transform.position);
        float xCenter = GetXCenterPosition(bottomRight, bottomLeft);
        Debug.Log("xCenter: " + xCenter);
        float zCenter = GetZCenterPosition(topLeft, bottomLeft);
        Debug.Log("zCenter: " + zCenter);

        Debug.DrawLine(_grid.transform.position, new Vector3(xCenter, 0, zCenter), Color.red, 5f);

        //Vector3 offset = new Vector3(xCenter, 0, zCenter) - _Grid.transform.position.With(y: 0);
        //_GridContainer.transform.localPosition -= offset;

        Vector3 offset = new Vector3(xCenter, 0, zCenter);
        _GridContainer.transform.localPosition -= offset;
    }

    private float GetZCenterPosition(Vector3 maxHeight, Vector3 minHeight)
    {
        Vector3 center = Vector3.Lerp(minHeight, maxHeight, 0.5f);
        return center.z;
    }

    private float GetXCenterPosition(Vector3 maxWidth, Vector3 minWidth)
    {
        Vector3 center = Vector3.Lerp(minWidth, maxWidth, 0.5f);
        return center.x;
    }


    #region Impl IGridPortability
    public Vector3Int ConvertToGridPos(Vector3 worldPos)
    {
        return _grid.WorldToCell(worldPos);
    }

    public Vector3 ConvertToWorldPos(Vector3Int gridPos)
    {
        return _grid.CellToWorld(gridPos);
    }
    #endregion Impl IGridPortability
}

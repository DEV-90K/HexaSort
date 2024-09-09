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
        _SwipeRotate.Register(_Followeres);
        _SwipeScale.enabled = true;
        _SwipeScale.Register(_Followeres);
    }

    public void OnInit()
    {
        _SwipeRotate.OnInit();
        _SwipeScale.OnInit();
    }


    public void FindCenter(Transform[] gridHexs)
    {
        Vector3 pos = Vector3.zero;
        foreach (Transform gridHex in gridHexs)
        {
            pos += gridHex.position;
        }
        pos = pos / gridHexs.Length;
        _GridContainer.transform.localPosition -= pos;
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

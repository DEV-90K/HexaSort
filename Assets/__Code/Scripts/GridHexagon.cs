using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class GridHexagon : PoolMember
{
    [SerializeField]
    private new Renderer renderer;
    public Color Color
    {
        get => renderer.material.color;
        set => renderer.material.color = value;
    }
    public StackHexagon StackOfCell { get; private set; }

    public Color cacheColor;
    private Color contactColor;

    private GridHexagonData _data;

    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#525252", out contactColor);
    }

    public void OnInitialize(GridHexagonData gridHexagon, IGridPortability gridPortability)
    {
        _data = gridHexagon;

        Vector3 cellPos = gridPortability.ConvertToWorldPos(new Vector3Int(gridHexagon.Row, gridHexagon.Column, 0));
        transform.position = cellPos;

        HexagonData hexData = ResourceManager.Instance.GetHexagonDataByID(gridHexagon.IDHex);

        if (ColorUtility.TryParseHtmlString(hexData.HexColor, out Color color))
        {
            cacheColor = color;
            Color = color;
        }

        if (gridHexagon.StackHexagon != null)
            GenerateInitialHexagonStack(gridHexagon.StackHexagon);
    }

    public void OnResert()
    {
        StackOfCell = null;
    }

    public void CollectImmediate()
    {
        if(StackOfCell != null)
        {
            StackOfCell.CollectImmediate();
        }

        OnResert();
        PoolManager.Despawn(this);
    }

    public bool CheckOccupied()
    {
        if (StackOfCell == null)
            return false;

        if(StackOfCell.gameObject.activeInHierarchy == false)
        {
            StackOfCell = null;
            return false;
        }

        if(StackOfCell.Hexagons == null)
        {
            StackOfCell.CollectImmediate();
            StackOfCell = null;
            return false;
        }

        return true;
    }

    public void SetStackOfCell(StackHexagon stack)
    {
        StackOfCell = stack;

        if(stack != null)
        {
            stack.transform.localEulerAngles = Vector3.zero;
        }
    }

    private void GenerateInitialHexagonStack(StackHexagonData stackHexagonDatas)
    {
        StackHexagon stackHexagon = PoolManager.Spawn<StackHexagon>(PoolType.STACK_HEXAGON, Vector3.zero , Quaternion.identity);
        StackOfCell = stackHexagon;
        StackOfCell.transform.SetParent(transform);
        StackOfCell.transform.localScale = Vector3.one;
        StackOfCell.transform.localPosition = Vector3.up * GameConstants.HexagonConstants.HEIGHT;

        stackHexagon.OnInit(stackHexagonDatas);
        stackHexagon.PlaceOnGridHexagon();
    }

    public void ShowColor()
    {
        Color = cacheColor;
    }

    public void ShowColorContact()
    {
        Color = contactColor;
    }

    #region Grid Hexagon Data
    internal GridHexagonData GetCurrentGridHexagonPlayingData()
    {
        if(CheckOccupied())
        {
            _data.UpdateStackHexagonData(StackOfCell.GetCurrentStackHexagonPlayingData());
        }   
        else
        {
            _data.UpdateStackHexagonData(null);
        }

        return _data;
    }
    #endregion Grid Hexagon Data
}

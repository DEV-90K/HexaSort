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

    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#525252", out contactColor);
    }

    public void OnInitialize(GridHexagonData gridHexagon, IGridPortability gridPortability)
    {
        Vector3 cellPos = gridPortability.ConvertToWorldPos(new Vector3Int(gridHexagon.Row, gridHexagon.Column, 0));
        transform.position = cellPos;

        if(ColorUtility.TryParseHtmlString(gridHexagon.HexColor, out Color color))
        {
            cacheColor = color;
            Color = color;
        }

        if(gridHexagon.StackHexagon != null)
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

        if(StackOfCell.gameObject.activeSelf == false)
        {
            StackOfCell = null;
            return false;
        }

        return true;
    }

    public void SetStackOfCell(StackHexagon stack)
    {
        StackOfCell = stack;
    }

    private void GenerateInitialHexagonStack(StackHexagonData stackHexagonDatas)
    {
        StackHexagon stackHexagon = PoolManager.Spawn<StackHexagon>(PoolType.STACK_HEXAGON, Vector3.zero, Quaternion.identity);
        StackOfCell = stackHexagon;
        StackOfCell.transform.SetParent(transform);
        StackOfCell.transform.localPosition = Vector3.zero;

        stackHexagon.OnInit(stackHexagonDatas);
    }

    public void ShowColor()
    {
        Color = cacheColor;
    }

    public void ShowColorContact()
    {
        Color = contactColor;
    }
}

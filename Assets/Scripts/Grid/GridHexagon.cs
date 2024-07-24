using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHexagon : PoolMember
{
    [SerializeField]
    private new Renderer renderer;

    [SerializeField]
    private CanvasGridHexagon _Canvas;

    public Color Color
    {
        get => renderer.material.color;
        set => renderer.material.color = value;
    }
    public StackHexagon StackOfCell { get; private set; }

    public Color normalColor;
    private Color contactColor;
    private Color lockColor;

    private GridHexagonState state;
    public GridHexagonState State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
            updateColorByState();
        }
    } 
        
    
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

        HexagonData hexData = ResourceManager.Instance.GetHexagonDataByID(20);
        ColorUtility.TryParseHtmlString(hexData.HexColor, out normalColor);
        HexagonData hexLockData = ResourceManager.Instance.GetHexagonDataByID(_data.IDHexLock);
        ColorUtility.TryParseHtmlString(hexLockData.HexColor, out lockColor);               

        if (gridHexagon.StackHexagon != null)
            GenerateInitialHexagonStack(gridHexagon.StackHexagon);

        State = _data.State;
    }

    public void OnResert()
    {
        StackOfCell = null;
    }

    public IEnumerator IE_RemoveStack()
    {
        List<Hexagon> hexagons = StackOfCell.Hexagons;
        int numberOfPlayerHexagon = hexagons.Count;
        float offsetDelayTime = 0;
        while (hexagons.Count > 0)
        {
            Hexagon playerHexagon = hexagons[0];
            playerHexagon.SetParent(null);
            playerHexagon.TweenVanish(offsetDelayTime);
            offsetDelayTime += GameConstants.HexagonConstants.TIME_DELAY;
            StackOfCell.RemovePlayerHexagon(playerHexagon);
            //hexagons.RemoveAt(0);
        }

        yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (numberOfPlayerHexagon - 1) * GameConstants.HexagonConstants.TIME_DELAY);
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

        if(StackOfCell.Hexagons == null)
        {
            StackOfCell.CollectImmediate();
            StackOfCell = null;
            return false;
        }

        if(StackOfCell.Hexagons.Count == 0)
        {
            StackOfCell = null;
            return false;
        }

        if (!gameObject.CompareObject(StackOfCell.transform.parent.gameObject))
        {
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
        Color = normalColor;
    }

    public void ShowColorContact()
    {
        Color = contactColor;
    }

    private void updateColorByState()
    {
        if(State == GridHexagonState.NONE || State == GridHexagonState.UNLOCK || State == GridHexagonState.NORMAL)
        {
            Color = normalColor;

            _Canvas.gameObject.SetActive(false);

            if(StackOfCell)
            {
                StackOfCell.ShowCanvas();
            }
        }
        else if(State == GridHexagonState.HOVER)
        {
            Color = contactColor;
        }
        else
        {
            Color = lockColor;           

            _Canvas.gameObject.SetActive(true);            
            _Canvas.UpdateTxtNumber(_data.UnLockGoal);

            if (StackOfCell)
            {
                StackOfCell.HideCanvas();
                _Canvas.transform.position = StackOfCell.GetTopPosition();
            }
            else
            {
                _Canvas.transform.position = transform.position + Vector3.up * (GameConstants.HexagonConstants.HEIGHT / 2f + 0.01f);
            }
        }
    }

    public bool CheckUnLockByHexagon(int amount)
    {
        return amount >= _data.UnLockGoal;
    }
    public void OnUnLock()
    {
        State = GridHexagonState.NORMAL;
    }

    #region Grid Hexagon Data
    internal GridHexagonData GetCurrentGridHexagonPlayingData()
    {
        if (CheckOccupied())
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

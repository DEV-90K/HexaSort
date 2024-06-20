using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHexagon : MonoBehaviour
{
    public HexagonStack StackOfCell { get; private set; }
    public bool IsOccupied 
    {  get => StackOfCell != null; 
       private set { } 
    }

    public void SetStackOfCell(HexagonStack stack)
    {
        StackOfCell = stack;
    }
}

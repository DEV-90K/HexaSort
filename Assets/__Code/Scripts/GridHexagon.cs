using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridHexagon : MonoBehaviour
{
    [SerializeField]
    private PlayerHexagon playerHexagonPrefab;

    [SerializeField]
    private Color[] hexagonColors;
    public HexagonStack StackOfCell { get; private set; }
    public bool IsOccupied 
    {  get => StackOfCell != null; 
       private set { } 
    }

    private void Start()
    {
        if(hexagonColors.Length > 0)
            GenerateInitialHexagonStack();
    }

    public void SetStackOfCell(HexagonStack stack)
    {
        StackOfCell = stack;
    }

    private void GenerateInitialHexagonStack()
    {

        // One Renderer Child and One Hexagon Stack is child of GridHexagon
        while(transform.childCount > 1)
        {
            Transform tf = transform.GetChild(1);
            tf.SetParent(null);
            DestroyImmediate(tf.gameObject);
        }

        StackOfCell = new GameObject("Level Generate Stack").AddComponent<HexagonStack>();
        StackOfCell.transform.SetParent(transform);
        StackOfCell.transform.localPosition = Vector3.zero;

        for(int i = 0; i < hexagonColors.Length; i++)
        {
            Vector3 spawnPosition = StackOfCell.transform.TransformPoint(Vector3.up * i * 0.2f);

            PlayerHexagon hexagonIns = Instantiate(playerHexagonPrefab, spawnPosition, Quaternion.identity);
            hexagonIns.Color = hexagonColors[i];

            StackOfCell.AddPlayerHexagon(hexagonIns);
        }
    }
}

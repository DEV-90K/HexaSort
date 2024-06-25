using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class HexagonStack : MonoBehaviour
{
    public List<PlayerHexagon> Hexagons { get; private set; }

    private void Start()
    {
        //OnInitialize();
    }

    public void OnInitialize()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            PlayerHexagon playerHexagon = transform.GetChild(i).GetComponent<PlayerHexagon>();
            AddPlayerHexagon(playerHexagon);
        }

        PlaceOnGridHexagon();
    }

    public int GetNumberSimilarColor()
    {
        Debug.Log("GetNumberSimilarColor");
        if(Hexagons == null && Hexagons.Count == 0)
        {
            Debug.LogError("No Hexagon in stack " + gameObject.GetInstanceID());
            return 0;
        }

        int amount = 1;
        for(int i = 0; i < Hexagons.Count - 1; i++)
        {
            if (Hexagons[i].Color != Hexagons[i + 1].Color)
            {
                amount++;
            }
        }
        Debug.Log("Amount: " + amount);
        return amount;
    }

    public int GetNumberTopPlayerHexagonSameColor()
    {
        Debug.Log("GetNumberTopPlayerHexagonSameColor");
        Color color = GetTopHexagonColor();
        int amount = 0;
        for (int i = Hexagons.Count - 1; i >= 0; i--)
        {
            if (Hexagons[i].Color == color)
            {
                amount++;
            }
            else
            {
                break;
            }
        }
        Debug.Log("Amount: " + amount);
        return amount;
    }

    public void AddPlayerHexagon(PlayerHexagon playerHexagon)
    {
        if(Hexagons == null)
        {  
            Hexagons = new List<PlayerHexagon>();
        }

        Hexagons.Add(playerHexagon);
        playerHexagon.SetParent(transform);
    }

    public Color GetTopHexagonColor()
    {
        return Hexagons[^1].Color;
    }

    public void PlaceOnGridHexagon()
    {
        foreach(PlayerHexagon playerHexagon in Hexagons)
        {
            playerHexagon.DisableCollider();
        }
    }

    public bool CheckContainPlayerHexagon(PlayerHexagon playerHexagon) => Hexagons.Contains(playerHexagon);
    public void RemovePlayerHexagon(PlayerHexagon playerHexagon)
    {
        Hexagons.Remove(playerHexagon);

        if (Hexagons.Count <= 0)
            DestroyImmediate(gameObject);
    }
}

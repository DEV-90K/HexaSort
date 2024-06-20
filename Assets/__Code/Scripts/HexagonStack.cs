using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonStack : MonoBehaviour
{
    public List<PlayerHexagon> Hexagons { get; private set; }

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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class StackHexagon : PoolMember
{
    public List<Hexagon> Hexagons { get; private set; }

    private void Start()
    {
        //OnInitialize();
    }

    public void OnInitialize(StackHexagonData data)
    {
        Debug.Log("Stack Hexagon Data");
        data.DebugLogObject();
        List<Color> colors = new List<Color>();
        for (int i = 0; i < data.HexColors.Length; i++)
        {
            if (ColorUtility.TryParseHtmlString(data.HexColors[i], out Color color))
            {
                colors.Add(color);
            }
        }

        Color[] hexagonColors = colors.ToArray();

        for (int i = 0; i < hexagonColors.Length; i++)
        {
            Vector3 spawnPosition = transform.TransformPoint(Vector3.up * i * 0.2f);

            Hexagon hexagonIns = PoolManager.Spawn<Hexagon>(PoolType.HEXAGON, spawnPosition, Quaternion.identity);
            hexagonIns.SetParent(transform);
            hexagonIns.OnSetUp();
            hexagonIns.Color = hexagonColors[i];
            hexagonIns.Configure(this);
            AddPlayerHexagon(hexagonIns);
        }

        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    Hexagon playerHexagon = transform.GetChild(i).GetComponent<Hexagon>();
        //    AddPlayerHexagon(playerHexagon);
        //}

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

    public void AddPlayerHexagon(Hexagon playerHexagon)
    {
        if(Hexagons == null)
        {  
            Hexagons = new List<Hexagon>();
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
        foreach(Hexagon playerHexagon in Hexagons)
        {
            playerHexagon.DisableCollider();
        }
    }

    public bool CheckContainPlayerHexagon(Hexagon playerHexagon) => Hexagons.Contains(playerHexagon);
    public void RemovePlayerHexagon(Hexagon playerHexagon)
    {
        Hexagons.Remove(playerHexagon);

        //TEST
        if (Hexagons.Count <= 0)
        {
            //DestroyImmediate(gameObject);
            Debug.Log("Despawn: " + GetInstanceID());
            PoolManager.Despawn(this);
        }
    }
}

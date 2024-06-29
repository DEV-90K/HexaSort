using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] pointSpawns;

    [SerializeField]
    private StackSpawner stackSpawner;

    private List<StackHexagon> stackHexagons;
    private List<StackHexagon> stackCollects;

    private const int NUMBER_OF_STACK = 3;
    private int stackCount = 0;    

    private void Awake()
    {
        stackCount = 0;
        stackHexagons = new List<StackHexagon>();
    }

    public void OnInit()
    {
        StackController.OnStackPlaced += StackController_OnStackPlaced;
        GenerateStacks();
    }

    public void OnResert()
    {
        stackCount = 0;
        stackHexagons = new List<StackHexagon>();
        StackController.OnStackPlaced -= StackController_OnStackPlaced;
    }

    private void StackController_OnStackPlaced()
    {
        stackCount++;

        if(stackCount == NUMBER_OF_STACK)
        {
            stackCount = 0;
            //stackSpawner.GenerateStacks();
            stackHexagons = new List<StackHexagon>();
            GenerateStacks();
        }
    }

    private void GenerateStacks()
    {
        stackHexagons.Clear();
        for (int i = 0; i < pointSpawns.Length; i++)
        {
           StackHexagon stack = stackSpawner.SpawnStack(pointSpawns[i]);
           stackHexagons.Add(stack);
        }
    }

    public void CollectRandomed()
    {
        stackCollects = new List<StackHexagon>();
        foreach (StackHexagon stack in stackHexagons)
        {
            stackCollects.Add(stack);
            stack.CollectPlayerHexagon(null);
        }

        OnResert();
    }

    public void CollectRandomImmediate()
    {
        foreach (StackHexagon stack in stackHexagons)
        {
            stack.CollectImmediate();
        }

        OnResert();
    }
}

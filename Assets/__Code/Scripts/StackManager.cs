using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStackOnPlaced
{
    public void OnStackPlaced();
}

public class StackManager : MonoBehaviour, IStackOnPlaced
{    
    [SerializeField]
    private Transform[] pointSpawns;

    [SerializeField]
    private StackController stackController;
    [SerializeField]
    private StackMerger stackMerger;

    [SerializeField]
    private StackRandomSpawner randomSpawner;
    [SerializeField]
    private StackDataSpawner dataSpawner;
    private StackSpawner stackSpawner;

    private List<StackHexagon> stackHexagons;
    private List<StackHexagon> stackCollects;

    private const int NUMBER_OF_STACK = 3;
    private int stackCount = 0;

    private void Awake()
    {
        stackCount = 0;
        stackHexagons = new List<StackHexagon>();
        stackController.OnInit(this);
    }

    public void OnInit()
    {      
        stackSpawner = randomSpawner;
        GenerateStacks();
    }

    public void OnInit(StackQueueData stackData)
    {
        stackSpawner = dataSpawner;
        dataSpawner.OnInit(stackData);
        GenerateStacks();
    }

    public void OnResert()
    {
        stackCount = 0;
        stackHexagons = new List<StackHexagon>();
    }

    public void OnStackPlaced()
    {
        stackCount++;

        if (stackCount == NUMBER_OF_STACK)
        {
            OnResert();
            GenerateStacks();
        }
    }

    public void ReGenerateStacks()
    {
        foreach(StackHexagon stack in stackHexagons)
        {
            stack.CollectImmediate();
        }

        GenerateStacks();
    }

    private void GenerateStacks()
    {
        stackHexagons.Clear();
        for (int i = 0; i < pointSpawns.Length; i++)
        {
           StackHexagon stack = stackSpawner.Spawn(pointSpawns[i]);
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

    public void DisableByBooster()
    {
        stackController.enabled = false;
    }

    public void EnableByBooster()
    {
        stackController.enabled = true;
    }

    public void MergeStackIntoGrid(GridHexagon grid)
    {
        stackMerger.OnStackPlacedOnGridHexagon(grid);
    }
}

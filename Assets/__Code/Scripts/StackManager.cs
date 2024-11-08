using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStackOnPlaced
{
    public void OnStackPlaced(StackHexagon stack);
}

public interface IStackSphereRadius
{
    public Vector3 GetRadiusByGrid();
}

public class StackManager : MonoBehaviour, IStackOnPlaced, IStackSphereRadius
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

    private void Awake()
    {
        stackHexagons = new List<StackHexagon>();
        stackController.OnInit(this, this);
        stackMerger.OnInit(this);
    }

    public void OnInit()
    {      
        stackSpawner = randomSpawner;
        stackMerger.OnResert();
        GenerateStacks();
    }

    public void OnInit(StackQueueData stackData)
    {
        stackSpawner = dataSpawner;
        dataSpawner.OnInit(stackData);
        stackMerger.OnResert();
        GenerateStacks();
    }

    public void OnResert()
    {
        stackHexagons.Clear();
    }

    public void OnStackPlaced(StackHexagon stack)
    {
        stackHexagons.Remove(stack);
        if (stackHexagons.Count == 0)
        {
            OnResert();
            GenerateStacks();
        }
    }

    public void ReGenerateStacks()
    {
        foreach (StackHexagon stack in stackHexagons)
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

    public Vector3 GetRadiusByGrid()
    {
        //Grid scale = Point Spawn stack Scale
        return pointSpawns[0].localScale;
    }
}

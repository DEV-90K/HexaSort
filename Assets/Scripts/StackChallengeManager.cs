using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackChallengeManager : MonoBehaviour, IStackOnPlaced, IStackSphereRadius
{
    public static Action OnAllStackPlaced;

    [SerializeField]
    private Transform[] pointSpawns;
    [SerializeField]
    private StackController stackController;
    [SerializeField]
    private StackMerger stackMerger;

    private List<StackHexagonData> stackHexagonDatas;
    private List<StackHexagon> stackHexagons;
    private List<StackHexagon> stackHexagonsShowed;
    private List<StackHexagon> stackCollects;

    private int _idx = 0;

    private void Awake()
    {        
        stackHexagons = new List<StackHexagon>();
        stackHexagonsShowed = new List<StackHexagon>();
        stackController.OnInit(this, this);
        stackMerger.OnInit(this);
    }

    public void OnInit(StackQueueData stackData)
    {
        if (stackData == null)
        {
            Debug.Log("Stack Data is null");
            return;
        }

        stackHexagonDatas = stackData.StackHexagonDatas.OfType<StackHexagonData>().ToList();
        foreach (StackHexagonData data in stackHexagonDatas)
        {
            StackHexagon stack = PoolManager.Spawn<StackHexagon>(PoolType.STACK_HEXAGON, Vector3.zero, Quaternion.identity);
            stack.OnInit(data);
            stack.gameObject.SetActive(false);
            stackHexagons.Add(stack);
        }

        Debug.Log("Stack Hexagons Lengt: " + stackHexagons.Count);

        stackMerger.OnResert();
        GenerateStacks();
    }

    public void OnResert()
    {
        stackHexagons.Clear();
        stackHexagonsShowed.Clear();
        _idx = 0;
    }

    public void OnStackPlaced(StackHexagon stack)
    {
        stackHexagons.Remove(stack);
        stackHexagonsShowed.Remove(stack);

        Debug.Log("Stack Hexagons Lengt: " + stackHexagons.Count);
        Debug.Log("IDX: " + _idx);

        if (stackHexagons.Count == 0)
        {
            ChallengeManager.Instance.OnFinishLosed();
            return;
        }

        if (stackHexagonsShowed.Count == 0)
        {            
            OnAllStackPlaced?.Invoke();
        }
    }

    private void GenerateStacks()
    {
        Debug.Log("Geerate Stacks");
        foreach (StackHexagon stact in stackHexagonsShowed)
        {
            stact.gameObject.SetActive(false);
        }
        stackHexagonsShowed.Clear();


        int idxOfPoint = 0;
        for (int i = _idx; i < stackHexagons.Count; i++)
        {
            if (idxOfPoint == pointSpawns.Length) return;

            stackHexagons[i].gameObject.SetActive(true);
            stackHexagons[i].transform.position = pointSpawns[idxOfPoint].transform.position;
            stackHexagonsShowed.Add(stackHexagons[i]);
            idxOfPoint++;
        }
    }

    public void CollectRandomed()
    {
        //stackCollects = new List<StackHexagon>();
        //foreach (StackHexagon stack in stackHexagons)
        //{
        //    stackCollects.Add(stack);
        //    stack.CollectPlayerHexagon(null);
        //}

        foreach(StackHexagon stack in stackHexagons)
        {
            stack.CollectImmediate();
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

    public Vector3 GetRadiusByGrid()
    {
        //Grid scale = Point Spawn stack Scale
        return pointSpawns[0].localScale;
    }

    internal void ShowStackLeft()
    {
        _idx -= pointSpawns.Length;

        if(_idx < 0)
        {
            _idx = 0;
        }

        GenerateStacks();
        Debug.Log("IDX: " + _idx);
    }

    internal bool CanShowLeft()
    {
        Debug.Log("IDX: " + _idx);
        return _idx > 0;
    }

    internal void ShowStackRight()
    {
        _idx -= (stackHexagonsShowed.Count - pointSpawns.Length);

        _idx += pointSpawns.Length;

        if (_idx > stackHexagons.Count - pointSpawns.Length)
        {
            _idx = stackHexagons.Count - pointSpawns.Length;
        }

        GenerateStacks();
        Debug.Log("IDX: " + _idx);
    }

    internal bool CanShowRight()
    {
        Debug.Log("IDX: " + _idx);
        return _idx < stackHexagons.Count - pointSpawns.Length;
    }
}

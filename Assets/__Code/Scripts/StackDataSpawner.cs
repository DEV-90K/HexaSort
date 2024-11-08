using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtils;

public class StackDataSpawner : StackSpawner
{
    private int idx;
    private List<StackHexagonData> _stackDatas;
    public void OnInit(StackQueueData stackData)
    {
        Debug.Log("OnInit StackData Spawner");
        idx = 0;
        _stackDatas = stackData.StackHexagonDatas.OfType<StackHexagonData>().ToList();
        _stackDatas.DebugLogObject();
    }

    public override StackHexagon Spawn(Transform tfPos)
    {
        StackHexagon stackHexagon = SpawnStack(tfPos.position);
        Debug.Log("Spawn");
        _stackDatas.DebugLogObject();
        if (idx >= _stackDatas.Count)
        {
            idx = 0;
            _stackDatas.Shuffle();
        }

        stackHexagon.OnInit(_stackDatas[idx]);
        idx++;

        return stackHexagon;
    }
}

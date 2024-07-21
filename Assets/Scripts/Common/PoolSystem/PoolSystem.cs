using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour
{
    [Header("Infomation Pool Member")]
    public PoolAmount[] Pools;

    private void Awake()
    {
        PoolManager.dictPools.Clear();
        for (int i = 0; i < Pools.Length; i++)
        {
            PoolManager.Preload(Pools[i].root, Pools[i].prefab, Pools[i].amount, Pools[i].collect, Pools[i].clamp);
        }
    }
}

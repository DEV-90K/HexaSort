using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{
    const int DEFAULT_POOL_SIZE = 3;

    private static Dictionary<PoolType, Pool> dictPools = new Dictionary<PoolType, Pool>();

    public static void Preload(Transform parent = null, PoolMember prefab = null, int qty = DEFAULT_POOL_SIZE, bool collect = false, bool clamp = false)
    {
        Pool pool = new Pool(parent, prefab, qty, collect, clamp);
        PoolType poolType = pool.poolType();
        if(!IsHasPool(poolType))
        {
            NewPool(poolType, pool);
        }
    }

    private static void NewPool(PoolType poolType, Pool pool)
    {
        dictPools.Add(poolType, pool);
    }

    private static Pool GetPool(PoolType poolType)
    {
        return dictPools[poolType];
    }

    private static void RemovePool(PoolType poolType)
    {
        dictPools.Remove(poolType);
    }

    private static bool IsHasPool(PoolType poolType)
    {
        return dictPools.ContainsKey(poolType);
    }

    public static PoolMember Spawn(PoolType poolType, Vector3 pos, Quaternion rot)
    {        
        if (!IsHasPool(poolType))
        {
            Debug.Log("Chưa khoi tao " + poolType);
        }

        Pool pool = GetPool(poolType);
        return pool.Spawn(pos, rot);
    }

    public static T Spawn<T>(PoolType poolType, Vector3 pos, Quaternion rot) where T : PoolMember
    {
        return Spawn(poolType, pos, rot) as T;
    }

    static public void Despawn(PoolMember mem)
    {
        
        if (mem.gameObject.activeSelf)
        {            
            if (IsHasPool(mem.poolType))
                GetPool(mem.poolType).Despawn(mem);
            else
                GameObject.Destroy(mem.gameObject);
        }
    }


    static public void Release(PoolType poolType)
    {
        if (IsHasPool(poolType))
        {
            GetPool(poolType).Release();
            RemovePool(poolType);
        }
    }

    static public void CollectAll()
    {
        foreach (PoolType key in dictPools.Keys)
            Collect(key);
    }

    static public void Collect(PoolType poolType)
    {
        if (IsHasPool(poolType))
            GetPool(poolType).Collect();
    }

    static public List<PoolMember> GetActiveMembers(PoolType poolType)
    {
        if (IsHasPool(poolType))
        {
            return GetPool(poolType).GetActiveMembers();
        }
        else
        {
            throw new InvalidOperationException("Not Found: " + EnumUtils.ParseString(poolType));
        }
    }
}

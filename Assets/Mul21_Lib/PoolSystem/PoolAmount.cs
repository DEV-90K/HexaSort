using System;
using UnityEngine;

[System.Serializable]
public class PoolAmount
{
    [Header("-- Pool Amount --")]
    public Transform root;
    public PoolMember prefab;
    public int amount;
    public bool collect;
    public bool clamp;
}

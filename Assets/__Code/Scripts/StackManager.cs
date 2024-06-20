using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    [SerializeField]
    private StackSpawner stackSpawner;

    private const int NUMBER_OF_STACK = 3;
    private int stackCount = 0;

    private void Awake()
    {
        StackController.OnStackPlaced += StackController_OnStackPlaced;
    }

    private void Start()
    {
        stackCount = 0;
        stackSpawner.GenerateStacks();
    }

    private void OnDestroy()
    {
        StackController.OnStackPlaced -= StackController_OnStackPlaced;
    }

    private void StackController_OnStackPlaced()
    {
        stackCount++;

        if(stackCount == NUMBER_OF_STACK)
        {
            stackCount = 0;
            stackSpawner.GenerateStacks();
        }
    }
}

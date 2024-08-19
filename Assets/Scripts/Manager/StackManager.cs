using Audio_System;
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

public class StackManager : MonoSingleton<StackManager>, IStackOnPlaced, IStackSphereRadius
{    
    [SerializeField]
    protected Transform[] pointSpawns;
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
    
    protected override void Awake()
    {
        base.Awake();
        stackHexagons = new List<StackHexagon>();
        stackController.OnInit(this);
    }

    private void OnInit()
    {
        stackSpawner = randomSpawner;
        stackController.OnResert();
        stackController.OnInit(this);
        GenerateStacks();
    }

    public virtual void OnInit(StackQueueData stackData)
    {
        if (stackData == null)
        {
            Debug.Log("Stack Data is null");
            OnInit();
            return;
        }

        stackSpawner = dataSpawner;
        dataSpawner.OnInit(stackData);
        stackController.OnResert();
        stackController.OnInit(this);
        GenerateStacks();
    }

    public void Configure(int amount, int[] probabilities)
    {
        randomSpawner.Configure(amount, probabilities);
    }

    public virtual void OnResert()
    {
        stackHexagons.Clear();
    }

    public virtual void OnStackPlaced(StackHexagon stack)
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
        Debug.Log("Geerate Stacks");
        if(stackHexagons.Count > 0)
        {
            foreach (StackHexagon stack in stackHexagons)
            {
                stack.CollectImmediate();
            }
        }

        stackHexagons.Clear();
        stackSpawner.OnEnterSpawn();

        for (int i = 0; i < pointSpawns.Length; i++)
        {
           StackHexagon stack = stackSpawner.Spawn(pointSpawns[i]);

            LeanTween.moveLocalX(stack.gameObject, 0f, 0.1f)
                .setFrom((i + 1) * 3f)
                .setEaseInOutSine()
                .setDelay(i * 0.05f)
                .setOnComplete(() =>
                {
                    SoundManager.instance.CreateSoundBuilder()
                        .WithPosition(stack.transform.position)
                        .Play(stack.SoundSpawn);
                });

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

    public Vector3 GetRadiusByGrid()
    {
        //Grid scale = Point Spawn stack Scale
        return pointSpawns[0].localScale;
    }

    public StackHexagon[] GetStackHexagonShowing()
    {
        return stackHexagons.ToArray();
    }

    #region Legancy
    public void MergePlayerHexagon(StackHexagon stackHexagon, List<Hexagon> listPlayerHexagonMerge)
    {
        float yOfCurrentGridHexagon = (stackHexagon.Hexagons.Count - 1) * GameConstants.HexagonConstants.HEIGHT;
        for (int i = 0; i < listPlayerHexagonMerge.Count; i++)
        {
            Hexagon playerHexagon = listPlayerHexagonMerge[i];
            stackHexagon.AddPlayerHexagon(playerHexagon);

            float yOffset = yOfCurrentGridHexagon + (i + 1) * GameConstants.HexagonConstants.HEIGHT;
            Vector3 localPos = Vector3.up * yOffset;
            playerHexagon.Configure(stackHexagon);
            playerHexagon.TweenMovingToGrid(localPos, i * GameConstants.HexagonConstants.TIME_DELAY);
        }
    }

    public List<Hexagon> GetPlayerHexagonSimilarColor(StackHexagon stackHexagon)
    {
        Color color = stackHexagon.GetTopHexagonColor();
        List<Hexagon> playerHexagons = new List<Hexagon>();
        for (int i = stackHexagon.Hexagons.Count - 1; i >= 0; i--)
        {
            if (ColorUtils.ColorEquals(stackHexagon.Hexagons[i].Color, color))
            {
                playerHexagons.Add(stackHexagon.Hexagons[i]);
            }
            else
            {
                break;
            }
        }

        return playerHexagons;
    }

    public IEnumerator IE_RemovePlayerHexagonsFromStack(StackHexagon stackHexagon)
    {
        List<Hexagon> listPlayerHexagonSimilarColor = GetPlayerHexagonSimilarColor(stackHexagon);
        int numberOfPlayerHexagon = listPlayerHexagonSimilarColor.Count;
        if (numberOfPlayerHexagon < 10)
            yield break;

        yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM); //Use see number of playerHexagon

        float offsetDelayTime = 0;
        while (listPlayerHexagonSimilarColor.Count > 0)
        {
            Hexagon playerHexagon = listPlayerHexagonSimilarColor[0];
            playerHexagon.SetParent(null);
            playerHexagon.TweenVanish(offsetDelayTime);
            offsetDelayTime += GameConstants.HexagonConstants.TIME_DELAY;
            stackHexagon.RemovePlayerHexagon(playerHexagon);
            listPlayerHexagonSimilarColor.RemoveAt(0);
        }
        yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (numberOfPlayerHexagon - 1) * GameConstants.HexagonConstants.TIME_DELAY);
    }
    #endregion Legancy
}

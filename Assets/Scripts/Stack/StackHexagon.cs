using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHexagon : PoolMember
{
    [SerializeField]
    private CanvasStackHexagon canvasStack;
    [SerializeField]
    private ParticleStackHexagon particleStack;
    [SerializeField]
    private Transform tf_Ray;
    public List<Hexagon> Hexagons { get; private set; }
    private StackHexagonData _data;

    private void Awake()
    {
        particleStack.OnInit(this);
        canvasStack.OnInit(this);
    }

    public void OnInit(StackHexagonData data)
    {
        _data = data;

        for(int i = 0; i < data.IDHexes.Length; i++)
        {
            Vector3 spawnPosition = transform.TransformPoint(Vector3.up * i * GameConstants.HexagonConstants.HEIGHT);
            HexagonData hexagonData = ResourceManager.Instance.GetHexagonDataByID(data.IDHexes[i]);
            Hexagon hexagonIns = PoolManager.Spawn<Hexagon>(PoolType.HEXAGON, spawnPosition, Quaternion.identity);
            hexagonIns.SetParent(transform);
            hexagonIns.OnSetUp();
            hexagonIns.OnInit(hexagonData);
            hexagonIns.Configure(this);
            AddPlayerHexagon(hexagonIns);
        }

        canvasStack.OnShow();
    }

    private void OnResert()
    {
        Hexagons = null;

        canvasStack.OnHide();
        particleStack.OnHide();
    }

    public Transform GetTransformRay()
    {
        return tf_Ray;
    }

    public void TweenShowTrick()
    {
        foreach (Hexagon hex  in Hexagons)
        {
            hex.TweenShowTrick();
        }
    }

    public void TweenHideTrick()
    {
        foreach (Hexagon hex in Hexagons)
        {
            hex.TweenHideTrick();
        }
    }

    public int GetNumberSimilarColor()
    {
        if(Hexagons == null && Hexagons.Count == 0)
        {
            Debug.LogError("No Hexagon in stack " + gameObject.GetInstanceID());
            return 0;
        }

        int amount = 1;
        for(int i = 0; i < Hexagons.Count - 1; i++)
        {
            if (Hexagons[i].Color != Hexagons[i + 1].Color)
            {
                amount++;
            }
        }

        return amount;
    }

    public int GetNumberSimilarTopColor()
    {
        int amount = 0;
        Color color = GetTopHexagonColor();

        for (int i = Hexagons.Count - 1; i >= 0; i--)
        {
            if (ColorUtils.ColorEquals(color, Hexagons[i].Color))
            {
                amount++;
            }
        }

        return amount;
    }

    public void AddPlayerHexagon(Hexagon playerHexagon)
    {
        if(Hexagons == null)
        {  
            Hexagons = new List<Hexagon>();
        }

        Hexagons.Add(playerHexagon);
        playerHexagon.SetParent(transform);

        UpdateCanvas();
    }

    public Color GetTopHexagonColor()
    {
        return Hexagons[^1].Color;
    }

    public void PlaceOnGridHexagon()
    {
        foreach(Hexagon playerHexagon in Hexagons)
        {
            playerHexagon.DisableCollider();
        }
    }

    public bool CheckContainPlayerHexagon(Hexagon playerHexagon) => Hexagons.Contains(playerHexagon);
    public bool CheckContainColor(Color color)
    {
        for(int i = 0; i < Hexagons.Count;i++)
        {
            if (Hexagons[i].CheckColor(color))
            {
                return true;
            }
        }

        return false;
    }
    public void RemovePlayerHexagon(Hexagon playerHexagon)
    {
        Hexagons.Remove(playerHexagon);
        if (Hexagons.Count <= 0)
        {
            Hexagons = new List<Hexagon>();
            PoolManager.Despawn(this);
        }

        UpdateCanvas();
    }

    public void CollectImmediate()
    {
        if(Hexagons != null)
        {
            for(int i = 0; i < Hexagons.Count; i++)
            {
                Hexagons[i].CollectImmediate();
            }
        }

        OnResert();
        PoolManager.Despawn(this);
    }

    public void CollectPlayerHexagon(System.Action callback = null)
    {
        if (Hexagons.Count <= 0)
        {
            Debug.LogWarning("Cannot Collect");
            return;
        }

        StartCoroutine(IE_CollectPlayerHexagon(callback));
    }

    public IEnumerator IE_CollectPlayerHexagon(System.Action callback = null)
    {
        int numberOfPlayerHexagon = Hexagons.Count;
        float offsetDelayTime = 0;
        //Remove bottom to top
        while (Hexagons.Count > 0)
        {
            Hexagon playerHexagon = Hexagons[Hexagons.Count - 1];
            playerHexagon.SetParent(null);
            playerHexagon.TweenVanish(offsetDelayTime);
            offsetDelayTime += GameConstants.HexagonConstants.TIME_DELAY;
            RemovePlayerHexagon(playerHexagon);
        }

        yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (numberOfPlayerHexagon - 1) * GameConstants.HexagonConstants.TIME_DELAY);
        callback?.Invoke();
        OnResert();
        PoolManager.Despawn(this);
    }

    internal IEnumerator IE_Remove()
    {
        int numberOfPlayerHexagon = 0;
        float offsetDelayTime = 0;
        while (Hexagons.Count > 0)
        {
            Hexagon playerHexagon = Hexagons[Hexagons.Count - 1];
            numberOfPlayerHexagon++;
            playerHexagon.SetParent(null);
            playerHexagon.TweenVanish(offsetDelayTime);
            offsetDelayTime += GameConstants.HexagonConstants.TIME_DELAY;
            RemovePlayerHexagon(playerHexagon);
        }

        yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (numberOfPlayerHexagon - 1) * GameConstants.HexagonConstants.TIME_DELAY);
    }

    public float GetTimeRemove()
    {        
        return GameConstants.HexagonConstants.TIME_ANIM + (Hexagons.Count - 1) * GameConstants.HexagonConstants.TIME_DELAY;        
    }

    #region Canvas  
    public Vector3 GetTopPosition()
    {
        Vector3 result = 
            transform.position + Vector3.up * (Hexagons.Count - 1) * GameConstants.HexagonConstants.HEIGHT 
            + Vector3.up * (GameConstants.HexagonConstants.HEIGHT / 2f + 0.01f);
        return result;
    }

    private void UpdateCanvas()
    {
        if (Hexagons == null || Hexagons.Count == 0)
        {
            return;
        }

        int amount = 0;
        Color color = GetTopHexagonColor();                

        for (int i = Hexagons.Count - 1; i >= 0; i--)
        {
            if(ColorUtils.ColorEquals(color, Hexagons[i].Color))
            {
                amount++;
            }
        }        

        canvasStack.OnShow();
    }

    //When merge and remove completed
    public void ShowCanvas()
    {
        if (Hexagons == null || Hexagons.Count == 0)
        {
            return;
        }

        UpdateCanvas();
    }

    //When in processing merge and remove > 10;
    public void HideCanvas()
    {
        canvasStack.OnHide();
    }

    public IEnumerator PlayParticles(VanishType type)
    {
        yield return particleStack.PlayParticle(type);
    }
    #endregion Canvas

    #region Stack Hexagon Data
    internal StackHexagonData GetCurrentStackHexagonPlayingData()
    {
        if(Hexagons == null || Hexagons.Count == 0)
        {
            return null;
        }

        int[] idHexs = new int[Hexagons.Count];

        for(int i = 0; i < Hexagons.Count; i++)
        {
            HexagonData hexagon = Hexagons[i].GetCurrentHexagonPlayingData();
            idHexs[i] = hexagon.ID;
        }

        _data.UpdateIDHexs(idHexs);

        return _data;
    }

    internal void SetData(StackHexagonData stackData)
    {
        _data = stackData;        
    }    
    #endregion Stack Hexagon Data
}

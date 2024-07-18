using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHexagon : PoolMember
{
    [SerializeField]
    private CanvasStackHexagon canvasStack;
    public List<Hexagon> Hexagons { get; private set; }
    private StackHexagonData _data;
    public void OnInit(StackHexagonData data)
    {
        Debug.Log("On Init StackHexagon");
        data.DebugLogObject();
        _data = data;
        //List<Color> colors = new List<Color>();
        //for (int i = 0; i < data.IDHexes.Length; i++)
        //{
        //    HexagonData hexData = ResourceManager.Instance.GetHexagonDataByID(data.IDHexes[i]);
        //    if (ColorUtility.TryParseHtmlString(hexData.HexColor, out Color color))
        //    {
        //        colors.Add(color);
        //    }
        //}

        //Color[] hexagonColors = colors.ToArray();

        //for (int i = 0; i < hexagonColors.Length; i++)
        //{
        //    Vector3 spawnPosition = transform.TransformPoint(Vector3.up * i * GameConstants.HexagonConstants.HEIGHT);

        //    Hexagon hexagonIns = PoolManager.Spawn<Hexagon>(PoolType.HEXAGON, spawnPosition, Quaternion.identity);
        //    hexagonIns.SetParent(transform);
        //    hexagonIns.OnSetUp();            
        //    hexagonIns.Color = hexagonColors[i];
        //    hexagonIns.Configure(this);
        //    AddPlayerHexagon(hexagonIns);
        //}

        for(int i = 0; i < data.IDHexes.Length; i++)
        {
            Vector3 spawnPosition = transform.TransformPoint(Vector3.up * i * GameConstants.HexagonConstants.HEIGHT);
            HexagonData hexagonData = ResourceManager.Instance.GetHexagonDataByID(data.IDHexes[i]);
            Hexagon hexagonIns = PoolManager.Spawn<Hexagon>(PoolType.HEXAGON, spawnPosition, Quaternion.identity);
            hexagonIns.SetParent(transform);
            hexagonIns.OnSetUp();
            hexagonIns.OnInit(hexagonData);
            //hexagonIns.Color = hexagonColors[i];
            hexagonIns.Configure(this);
            AddPlayerHexagon(hexagonIns);
        }
    }

    public void OnResert()
    {
        Hexagons = null;
        ShowCanvas();
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

    public Vector3 GetTopPositon()
    {
        return canvasStack.transform.position;
    }

    public void PlaceOnGridHexagon()
    {
        foreach(Hexagon playerHexagon in Hexagons)
        {
            playerHexagon.DisableCollider();
        }
    }

    public bool CheckContainPlayerHexagon(Hexagon playerHexagon) => Hexagons.Contains(playerHexagon);
    public void RemovePlayerHexagon(Hexagon playerHexagon)
    {
        Hexagons.Remove(playerHexagon);

        //TEST
        if (Hexagons.Count <= 0)
        {
            //DestroyImmediate(gameObject);
            Debug.Log("Despawn: " + GetInstanceID());
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
            Hexagon playerHexagon = Hexagons[0];
            playerHexagon.SetParent(null);
            playerHexagon.TweenVanish(offsetDelayTime);
            offsetDelayTime += GameConstants.HexagonConstants.TIME_DELAY;
            Hexagons.RemoveAt(0);
        }

        yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (numberOfPlayerHexagon - 1) * GameConstants.HexagonConstants.TIME_DELAY);
        callback?.Invoke();
        OnResert();
        PoolManager.Despawn(this);
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

        canvasStack.transform.position = GetTopPosition();
        canvasStack.UpdateTxtNumber(amount);
    }

    //When merge and remove completed
    public void ShowCanvas()
    {
        if (Hexagons == null || Hexagons.Count == 0)
        {
            return;
        }

        canvasStack.gameObject.SetActive(true);
        UpdateCanvas();
    }

    //When in processing merge and remove > 10;
    public void HideCanvas()
    {
        canvasStack.gameObject.SetActive(false);
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
            idHexs[i] = Hexagons[i].GetCurrentHexagonPlayingData().ID;
        }

        _data.UpdateIDHexs(idHexs);

        return _data;
    }

    public StackHexagonData GetData()
    {
        return _data;
    }
    #endregion Stack Hexagon Data
}

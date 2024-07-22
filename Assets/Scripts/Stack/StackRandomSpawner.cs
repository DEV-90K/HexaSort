using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class StackRandomSpawner : StackSpawner
{
    [SerializeField]
    private Vector2Int hexagonClampf;

    private List<StackHexagon> cacheStacks = new List<StackHexagon>();
    private int _amountOfColor;
    private int[] _probabilitiesOfSimilarColor;
    //private Color[] _cacheColors;
    //private Color[] _colors;

    private HexagonData[] _hexagonDatas;
    private HexagonData[] _cacheHexagonDatas;

    private void Start()
    {
        LoadConfig();

        HexagonData[] datas = ResourceManager.Instance.GetAllHexagonData();
        //List<Color> listColors = new List<Color>();

        //for (int i = 0; i < datas.Length; i++)
        //{
        //    HexagonData data = datas[i];

        //    if (data.ID >= 20)
        //        continue;

        //    if (ColorUtility.TryParseHtmlString(data.HexColor, out Color color))
        //    {
        //        listColors.Add(color);
        //    }
        //}

        //_cacheColors = listColors.ToArray();

        List<HexagonData> list = new List<HexagonData>();
        foreach (HexagonData data in datas)
        {
            if(data.ID >= 20)
            {
                continue;
            }

            list.Add(data);
        }

        _cacheHexagonDatas = list.ToArray();
    }

    private void LoadConfig()
    {
        StackConfig stackConfig = ResourceManager.Instance.GetStackConfig();
        //NUMBER_COLOR_IN_STACK = stackConfig.NumberOfColor;
        hexagonClampf = new Vector2Int(stackConfig.AmountClampf[0], stackConfig.AmountClampf[1]);
    }

    public override StackHexagon Spawn(Transform stack, int COUNT = 0)
    {
        if (COUNT > 100)
        {
            Debug.LogError("Some thing wrong");
            return null;
        }
        else
        {
            COUNT++;
        }

        HexagonData[] hexagons = GetRandomHexagonDatas();
        int[] IDHexs = new int[hexagons.Length];

        for (int i = 0; i < hexagons.Length; i++)
        {
            IDHexs[i] = hexagons[i].ID;
        }
        StackHexagonData stackData = new StackHexagonData(IDHexs);

        StackHexagon insHexagonStack = SpawnStack(stack.position);
        insHexagonStack.name = $"Hexagon Stack"; //{stack.GetSiblingIndex()}
        insHexagonStack.transform.SetParent(stack);
        insHexagonStack.transform.localPosition = Vector3.zero;
        insHexagonStack.transform.localScale = Vector3.one;
        insHexagonStack.SetData(stackData);

        int numberOfHexagon = Random.Range(hexagonClampf.x, hexagonClampf.y);
        int numberOfSimilar = GetNumberOfSimilar();
        int[] arrHexagon = GetRandomHexagons(numberOfHexagon, numberOfSimilar);

        int amount = 0;
        for (int i = 0; i < arrHexagon.Length; i++)
        {
            HexagonData hexagonData = hexagons[i];

            for (int j = 0; j < arrHexagon[i]; j++)
            {
                Vector3 localPos = Vector3.up * amount * GameConstants.HexagonConstants.HEIGHT;
                Vector3 pos = insHexagonStack.transform.TransformPoint(localPos);
                Hexagon insPlayerHexagon = SpawnHexagon(insHexagonStack, hexagonData, pos);
                insPlayerHexagon.transform.localScale = Vector3.one;
                amount++;
            }
        }

        if (CheckStackSimilar(insHexagonStack))
        {
            insHexagonStack.CollectImmediate();
            return this.Spawn(stack, COUNT);
        }

        insHexagonStack.ShowCanvas();
        cacheStacks.Add(insHexagonStack);
        return insHexagonStack;
    }

    public void Configure(int amount, int[] probabilities)
    {
        _amountOfColor = amount;              
        
        int total = probabilities.Sum();

        if(total < 100)
        {
            Debug.LogWarning("Total probabilities not equal 100");
            List<int> list = new List<int>();
            list.AddRange(probabilities);
            list.Add(100 - total);

            _probabilitiesOfSimilarColor = list.ToArray();
        }
        else
        {
            _probabilitiesOfSimilarColor = probabilities;
        }

        if(_amountOfColor < _probabilitiesOfSimilarColor.Length)
        {
            Debug.LogError($"Color does not exist for the case of {_probabilitiesOfSimilarColor.Length} colors in the stack");
            _amountOfColor = _probabilitiesOfSimilarColor.Length;
        }

        //_colors = _cacheColors.Take(_amountOfColor).ToArray();
        _hexagonDatas = _cacheHexagonDatas.Take(_amountOfColor).ToArray();
    }

    private int GetNumberOfSimilar()
    {
        int k = 1000;
        while(k > 0)
        {
            k--;

            int rand = Random.Range(0, 101); // [0, 100]

            int probability = 0;
            for(int i = 0; i < _probabilitiesOfSimilarColor.Length; i++)
            {
                probability += _probabilitiesOfSimilarColor[i];

                if(rand <= probability)
                {
                    return i + 1;
                }
            }
        }

        Debug.LogError("Something wrong");
        return 0;
    }

    private bool CheckStackSimilar(StackHexagon stackCompare)
    {
        if (cacheStacks.Count == 0)
        {
            return false;
        }

        List<Hexagon> hexsCompare = stackCompare.Hexagons;

        for (int i = 0; i < cacheStacks.Count; i++)
        {
            StackHexagon stack = cacheStacks[i];
            List<Hexagon> hexs = stack.Hexagons;

            if (hexs.Count != hexsCompare.Count)
            {
                return false;
            }

            for (int j = 0; j < hexsCompare.Count; j++)
            {
                if (ColorUtils.ColorEquals(hexs[j].Color, hexsCompare[j].Color))
                {
                    //Similar last Hex => all stack similar
                    if(j == hexsCompare.Count - 1)
                    {
                        return true;
                    }                    
                }
                else
                {
                    break;
                }
            }
        }
        
        return false;
    }

    //Get Random Color base on config amount = _colors.length();
    //private Color[] GetRandomColors_v2()
    //{
    //    return _colors.OrderBy(x => Random.value).ToArray();
    //}

    private HexagonData[] GetRandomHexagonDatas()
    {
        return _hexagonDatas.OrderBy(x => Random.value).ToArray();
    }


    //Hexagons same color
    private int[] GetRandomHexagons(int totalHexagon, int numberSplit)
    {
        int[] arrHexagon = new int[numberSplit];
        for (int i = 0; i < numberSplit; i++)
        {
            if (totalHexagon < 0)
            {
                Debug.LogError("Something wrong");
            }
            else if (totalHexagon == 0)
            {
                arrHexagon[i] = 0;
            }
            else if (i == numberSplit - 1)
            {
                arrHexagon[i] = totalHexagon;
                totalHexagon = 0;
            }
            else
            {
                arrHexagon[i] = Random.Range(0, totalHexagon);
                totalHexagon -= arrHexagon[i];
            }
        }

        return arrHexagon;
    }

    private void ClearCacheStacks()
    {
        Debug.Log("Clear Cache Stacks");
        cacheStacks.Clear();
    }

    public override void OnEnterSpawn()
    {
        base.OnEnterSpawn();
        ClearCacheStacks();
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtils;

public class StackRandomSpawner : StackSpawner
{
    [Header("Player Hexagon Colors")]
    [SerializeField]
    private Color[] colors;
    [SerializeField]
    private Vector2Int hexagonClampf;

    private int NUMBER_COLOR_IN_STACK = 3;
    //private Vector2Int HEXAGON_CLAMPF;

    private void Start()
    {
        LoadConfig();

        HexagonData[] datas = ResourceManager.Instance.GetAllHexagonData();
        List<Color> listColors = new List<Color>();
        foreach (HexagonData data in datas)
        {
            if (data.ID == 1) continue; //1 is color of grid
            if (ColorUtility.TryParseHtmlString(data.HexColor, out Color color))
            {
                listColors.Add(color);
            }
        }

        //List<HexagonData> listData = new List<HexagonData>();
        //listData.AddRange(datas);
        //listData.Shuffle();

        //for (int i = 2; i < listData.Count; i++)
        //{
        //    HexagonData data = datas[i];
        //    if (ColorUtility.TryParseHtmlString(data.HexColor, out Color color))
        //    {
        //        listColors.Add(color);
        //    }

        //    if(listColors.Count == 3)
        //    {
        //        break;
        //    }
        //}


        colors = listColors.ToArray();
    }

    private void LoadConfig()
    {
        StackConfig stackConfig = ResourceManager.Instance.GetStackConfig();
        NUMBER_COLOR_IN_STACK = stackConfig.NumberOfColor;
        hexagonClampf = new Vector2Int(stackConfig.AmountClampf[0], stackConfig.AmountClampf[1]);
    }

    public override StackHexagon Spawn(Transform stack)
    {
        StackHexagon insHexagonStack = SpawnStack(stack.position);
        insHexagonStack.name = $"Hexagon Stack"; //{stack.GetSiblingIndex()}
        insHexagonStack.transform.SetParent(stack);
        insHexagonStack.transform.localPosition = Vector3.zero;
        insHexagonStack.transform.localScale = Vector3.one;

        Color[] colors = GetRandomColors(NUMBER_COLOR_IN_STACK);        

        int numberOfHexagon = Random.Range(hexagonClampf.x, hexagonClampf.y);
        int[] arrHexagon = GetRandomHexagons(numberOfHexagon, NUMBER_COLOR_IN_STACK);
        int amount = 0;
        for (int i = 0; i < arrHexagon.Length; i++)
        {
            Color color = colors[i];
            for (int j = 0; j < arrHexagon[i]; j++)
            {
                Vector3 localPos = Vector3.up * amount * GameConstants.HexagonConstants.HEIGHT;
                Vector3 pos = insHexagonStack.transform.TransformPoint(localPos);
                Hexagon insPlayerHexagon = SpawnHexagon(insHexagonStack, color, pos);
                //insPlayerHexagon.transform.SetParent(insHexagonStack.transform);
                insPlayerHexagon.transform.localScale = Vector3.one;
                amount++;
            }
        }

        return insHexagonStack;
    }

    //Each stack have many than one color
    private Color[] GetRandomColors(int numberOfColor)
    {
        List<Color> listColors = new List<Color>();
        listColors.AddRange(colors);

        List<Color> listResults = new List<Color>();
        while (numberOfColor > 0)
        {
            numberOfColor--;

            if (listColors.Count <= 0)
                break;

            Color color = listColors.OrderBy(x => Random.value).First();
            listColors.Remove(color);
            listResults.Add(color);
        }

        return listResults.ToArray();
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
}

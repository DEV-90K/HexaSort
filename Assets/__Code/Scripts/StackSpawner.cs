using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] pointSpawns;
    [SerializeField]
    private StackHexagon hexagonStack;
    [SerializeField]
    private Hexagon playerHexagon;

    [Header("Player Hexagon Colors")]
    [SerializeField]
    private Color[] colors;
    [SerializeField]
    private Vector2Int hexagonClampf;

    public StackHexagon SpawnStack(StackHexagonData stackHexagonDatas)
    {
        StackHexagon stackHexagon = PoolManager.Spawn<StackHexagon>(PoolType.STACK_HEXAGON, Vector3.zero, Quaternion.identity);
        return stackHexagon;
    }

    public StackHexagon SpawnStack(Transform stack)
    {
        //TEST
        //StackHexagon insHexagonStack = Instantiate(hexagonStack, stack.position, Quaternion.identity, stack);
        StackHexagon insHexagonStack = PoolManager.Spawn<StackHexagon>(PoolType.STACK_HEXAGON, stack.position, Quaternion.identity);
        insHexagonStack.transform.SetParent(stack);
        insHexagonStack.name = $"Hexagon Stack"; //{stack.GetSiblingIndex()}

        const int NUMBER_COLOR_IN_STACK = 2;

        Color[] colors = GetRandomColors(NUMBER_COLOR_IN_STACK);        

        int numberOfHexagon = Random.Range(hexagonClampf.x, hexagonClampf.y);
        int[] arrHexagon = GetRandomHexagons(numberOfHexagon, NUMBER_COLOR_IN_STACK);
        int amount = 0;
        List<string> hexColors = new List<string>();

        for (int i = 0; i < arrHexagon.Length; i++)
        {
            Color color = colors[i];
            for (int j = 0; j < arrHexagon[i]; j++)
            {
                amount++;
                Vector3 localPos = Vector3.up * amount * 0.2f;
                Vector3 pos = insHexagonStack.transform.TransformPoint(localPos);
                //TEST
                //Hexagon insPlayerHexagon = Instantiate(playerHexagon, pos, Quaternion.identity, insHexagonStack.transform);
                Hexagon insPlayerHexagon = PoolManager.Spawn<Hexagon>(PoolType.HEXAGON, pos, Quaternion.identity);
                insPlayerHexagon.OnSetUp();
                insPlayerHexagon.Color = color;                
                insPlayerHexagon.SetParent(insHexagonStack.transform);
                insPlayerHexagon.Configure(insHexagonStack);
                insHexagonStack.AddPlayerHexagon(insPlayerHexagon);

                hexColors.Add(ColorUtility.ToHtmlStringRGB(color));
            }
        }

        //Debug.Log(JsonConvert.SerializeObject(hexColors.ToArray()));

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

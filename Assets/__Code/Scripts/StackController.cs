using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtils;

public class StackController : MonoBehaviour
{
    [SerializeField]
    private Transform containHexagonStack;
    [SerializeField]
    private Transform hexagonStack;
    [SerializeField]
    private PlayerHexagon playerHexagon;

    [Header("Player Hexagon Colors")]
    [SerializeField]
    private Color[] colors;
    [SerializeField]
    private Vector2Int hexagonClampf;

    private void Start()
    {
        GenerateStacks();
    }

    //private void Genereting()
    //{
    //    foreach (Transform stack in stacks) { TransformExtensions.DestroyChildrenImmediate(stack); }

    //    foreach (Transform stack in stacks)
    //    {
    //        int numberOfCell = Random.Range(1, 4);

    //        for(int i = 0; i <= numberOfCell; i++)
    //        {
    //            Vector3 pos = new Vector3(stack.position.x, stack.position.y + i * 0.2f, stack.position.z);
    //            Instantiate(playerCell,pos, Quaternion.identity, stack);
    //        }
    //    }
    //}

    private void GenerateStacks()
    {
        for (int i = 0; i < containHexagonStack.childCount; i++)
        {
            GenerateStack(containHexagonStack.GetChild(i));
        }
    }

    private void GenerateStack(Transform stack)
    {
        Transform insHexagonStack = Instantiate(hexagonStack, stack.position, Quaternion.identity, stack);
        insHexagonStack.name = $"Hexagon Stack"; //{stack.GetSiblingIndex()}

        //Color stackColor = colors[Random.Range(0, colors.Length)];

        const int NUMBER_COLOR_IN_STACK = 3;

        Color[] colors = GetRandomColors(NUMBER_COLOR_IN_STACK);
        int numberOfHexagon = Random.Range(hexagonClampf.x, hexagonClampf.y);
        Debug.Log("Number Of Hexagon " + numberOfHexagon);
        int[] arrHexagon = GetRandomHexagons(numberOfHexagon, NUMBER_COLOR_IN_STACK);
        Debug.Log(arrHexagon[0] + " " + arrHexagon[1]);

        //for (int i = 0; i < numberOfHexagon; i++)
        //{
        //    Vector3 localPos = Vector3.up * i * 0.2f;
        //    Vector3 pos = insHexagonStack.TransformPoint(localPos);
        //    PlayerHexagon insPlayerHexagon = Instantiate(playerHexagon, pos, Quaternion.identity, insHexagonStack);
        //    insPlayerHexagon.Color = stackColor;
        //}

        int amount = 0;
        for(int i = 0; i < arrHexagon.Length; i++)
        {
            Color color = colors[i];
            for (int j = 0; j < arrHexagon[i]; j++)
            {
                amount++;
                Vector3 localPos = Vector3.up * amount * 0.2f;
                Vector3 pos = insHexagonStack.TransformPoint(localPos);
                PlayerHexagon insPlayerHexagon = Instantiate(playerHexagon, pos, Quaternion.identity, insHexagonStack);
                insPlayerHexagon.Color = color;
            } 
                
        }
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
        for(int i = 0; i < numberSplit; i++)
        {
            if (totalHexagon < 0)
            {
                Debug.LogError("Something wrong");
            }
            else if (totalHexagon == 0)
            {
                arrHexagon[i] = 0;
            }
            else if(i == numberSplit - 1)
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

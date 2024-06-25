using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class StackMerger : MonoBehaviour
{
    [SerializeField]
    private LayerMask gridHexagonLayerMask;

    public List<GridHexagon> listGridHexagonNeedUpdate = new List<GridHexagon>();

    private void Awake()
    {
        StackController.OnStackPlacedOnGridHexagon += StackController_OnStackPlacedOnGridHexagon;
    }

    private void OnDestroy()
    {
        StackController.OnStackPlacedOnGridHexagon -= StackController_OnStackPlacedOnGridHexagon;
    }
    private void StackController_OnStackPlacedOnGridHexagon(GridHexagon gridHexagon)
    {
        StartCoroutine(IE_OnStackPlacedOnGridHexagon(gridHexagon));
    }

    private IEnumerator IE_OnStackPlacedOnGridHexagon(GridHexagon gridHexagon)
    {
        listGridHexagonNeedUpdate.Add(gridHexagon);
        while (listGridHexagonNeedUpdate.Count > 0)
        {
            GridHexagon merge = listGridHexagonNeedUpdate[listGridHexagonNeedUpdate.Count - 1];

            listGridHexagonNeedUpdate.Remove(merge);
            if(merge.IsOccupied)
                yield return IE_CheckForMerge(merge);
        }
    }

    private IEnumerator IE_CheckForMerge(GridHexagon gridHexagon)
    {
        //Get Top Color of Stack
        HexagonStack stackHexagon = gridHexagon.StackOfCell;
        Color topColorOfStackAtGridHexagon = stackHexagon.GetTopHexagonColor();

        //Check this GridHexagon has neighbors?                
        List<GridHexagon> listNeighborGridHexagons = GetNeighborGidHexagon(gridHexagon);        
        //No neighbor Occupied (All neighbor not contain any stack)
        if (listNeighborGridHexagons.Count <= 0)
        {
            Debug.Log("No neighbor Occupied (All neighbor not contain any stack)");
            yield break;
        }
        //End

        //Check if one of those neighbors or multiple of them have the same top hexagon color [Do these neighbors have the same top hex colors]
        List<GridHexagon> neighborGridHexagonSameTopColor = GetHexagonStackOfNeighborSameTopColor(listNeighborGridHexagons, topColorOfStackAtGridHexagon);

        if (neighborGridHexagonSameTopColor.Count <= 0)
        {
            Debug.Log("No neighbor have same color");
            yield break;
        }
        else
        {
            listGridHexagonNeedUpdate.AddRange(neighborGridHexagonSameTopColor); // [1]
        }
        //End

        //Merge action
        //Merge solution 1: Merge neighbor cells towards this cell {merge everything inside current grid hexagon}
        //List<PlayerHexagon> listPlayerHexagonMerge = GetPlayerHexagonNeedMerge(topColorOfStackAtGridHexagon, neighborGridHexagonSameTopColor);
        //RemovePlayerHexagonFromOldStack(neighborGridHexagonSameTopColor, listPlayerHexagonMerge);
        //MergePlayerHexagon(stackHexagon, listPlayerHexagonMerge);
        //yield return new WaitForSeconds(0.2f + listPlayerHexagonMerge.Count * 0.01f); //0.2f time anim + 0.01 time delay.setDelay(transform.GetSiblingIndex() * 0.01f);

        //Merge solution 2: Merge everyting on the cell that has the lowest amount of that smame hexagon color or the one that has a bigger amount [using the one that has a smaller amount is actually better]
        //neighborGridHexagonSameTopColor = UpdateNeighborGridHexagonSameTopColor_S2(gridHexagon, neighborGridHexagonSameTopColor).ToList();
        //gridHexagon = neighborGridHexagonSameTopColor[^1];
        //neighborGridHexagonSameTopColor.Remove(gridHexagon);
        //stackHexagon = gridHexagon.StackOfCell;
        //topColorOfStackAtGridHexagon = stackHexagon.GetTopHexagonColor();

        //Merge solution 3: Target Merge craete stack have One Color if can. Get Current Grid is center of merge process 
        yield return IE_AlgorithmMerge_3(gridHexagon, neighborGridHexagonSameTopColor);
        //neighborGridHexagonSameTopColor = UpdateNeighborGridHexagonSameTopColor_S3(gridHexagon, neighborGridHexagonSameTopColor);
        //yield return IE_MergePlayerHexagonsToStack(stackHexagon, neighborGridHexagonSameTopColor);

        ////Check stack completed when >= 10 similar hexagons
        //List<PlayerHexagon> listPlayerHexagonSimilarColor = GetPlayerHexagonSimilarColor_V2(stackHexagon, topColorOfStackAtGridHexagon);
        //if (listPlayerHexagonSimilarColor.Count >= 10)
        //{
        //    yield return IE_RemovePlayerHexagonsFromStack_v2(listPlayerHexagonSimilarColor, stackHexagon);
        //    //After delete some PlayerHexagon from stack so need recheck
        //    //if (gridHexagon.IsOccupied)
        //    //    listGridHexagonNeedUpdate.Add(gridHexagon);
        //    listGridHexagonNeedUpdate.Add(gridHexagon);
        //}
    }

    private IEnumerator IE_RemovePlayerHexagonsFromStack(List<PlayerHexagon> listPlayerHexagonSimilarColor, HexagonStack stackHexagon)
    {
        Debug.Log("RemovePlayerHexagonsFromStack" + listPlayerHexagonSimilarColor.Count);
        int numberOfPlayerHexagon = listPlayerHexagonSimilarColor.Count;
        float offsetDelayTime = 0;
        //Remove bottom to top
        while(listPlayerHexagonSimilarColor.Count > 0)
        {            
            PlayerHexagon playerHexagon = listPlayerHexagonSimilarColor[0];
            playerHexagon.SetParent(null);
            playerHexagon.TweenVanish(offsetDelayTime);            
            offsetDelayTime += 0.01f;

            stackHexagon.RemovePlayerHexagon(playerHexagon);
            listPlayerHexagonSimilarColor.RemoveAt(0);
        }

        yield return new WaitForSeconds(0.2f + (numberOfPlayerHexagon + 1) * 0.01f);
    }

    private IEnumerator IE_RemovePlayerHexagonsFromStack_v2(HexagonStack stackHexagon)
    {
        List<PlayerHexagon> listPlayerHexagonSimilarColor = GetPlayerHexagonSimilarColor_V2(stackHexagon);
        Debug.Log("RemovePlayerHexagonsFromStack" + listPlayerHexagonSimilarColor.Count);
        int numberOfPlayerHexagon = listPlayerHexagonSimilarColor.Count;
        if (numberOfPlayerHexagon < 10) 
            yield break;

        float offsetDelayTime = 0;
        //Remove bottom to top
        while (listPlayerHexagonSimilarColor.Count > 0)
        {
            PlayerHexagon playerHexagon = listPlayerHexagonSimilarColor[0];
            playerHexagon.SetParent(null);
            playerHexagon.TweenVanish(offsetDelayTime);
            offsetDelayTime += 0.01f;

            stackHexagon.RemovePlayerHexagon(playerHexagon);
            listPlayerHexagonSimilarColor.RemoveAt(0);
        }

        yield return new WaitForSeconds(0.2f + (numberOfPlayerHexagon + 1) * 0.01f);
    }

    private List<PlayerHexagon> GetPlayerHexagonSimilarColor_V2(HexagonStack stackHexagon)
    {
        Color color = stackHexagon.GetTopHexagonColor();
        List<PlayerHexagon> playerHexagons = new List<PlayerHexagon>();
        for (int i = stackHexagon.Hexagons.Count - 1; i >= 0; i--)
        {
            if (stackHexagon.Hexagons[i].Color == color)
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
        

    private IEnumerator IE_MergePlayerHexagonsToStack(HexagonStack stackHexagon, List<GridHexagon> neighborGridHexagon)
    {
        List<PlayerHexagon> listPlayerHexagonMerge = GetPlayerHexagonNeedMerge(stackHexagon.GetTopHexagonColor(), neighborGridHexagon);
        RemovePlayerHexagonFromOldStack(neighborGridHexagon, listPlayerHexagonMerge);
        MergePlayerHexagon(stackHexagon, listPlayerHexagonMerge);
        yield return new WaitForSeconds(0.2f + listPlayerHexagonMerge.Count * 0.01f + 0.01f);
    }

    private List<PlayerHexagon> GetPlayerHexagonSimilarColor(HexagonStack stackHexagon, Color topColorOfStackAtGridHexagon)
    {
        List<PlayerHexagon> playerHexagons = new List<PlayerHexagon>();
        for(int i = stackHexagon.Hexagons.Count - 1; i >= 0; i--)
        {
            if (stackHexagon.Hexagons[i].Color == topColorOfStackAtGridHexagon)
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

    private void MergePlayerHexagon(HexagonStack stackHexagon, List<PlayerHexagon> listPlayerHexagonMerge)
    {
        float yOfCurrentGridHexagon = stackHexagon.Hexagons.Count * 0.2f + 0.2f; //0.2f of GridHexagon
        for (int i = 0; i < listPlayerHexagonMerge.Count; i++)
        {
            PlayerHexagon playerHexagon = listPlayerHexagonMerge[i];
            stackHexagon.AddPlayerHexagon(playerHexagon); //have setparent inside addPlayerHexagon

            float yOffset = yOfCurrentGridHexagon + i * 0.2f;
            Vector3 localPos = Vector3.up * yOffset; //(0, yOffset, 0)
            //playerHexagon.transform.position = stackHexagon2.transform.position.Add(0, yOffset, 0);
            //playerHexagon.transform.localPosition = localPos;
            playerHexagon.MoveToGridHexagon(localPos);
        }
    }



    private static void RemovePlayerHexagonFromOldStack(List<GridHexagon> neighborGridHexagonSameTopColor, List<PlayerHexagon> listPlayerHexagonMerge)
    {
        //Remove PlayerHexagon need merge from Hexagon Stack contain before
        foreach (GridHexagon gridHex in neighborGridHexagonSameTopColor)
        {
            HexagonStack hexagonStack = gridHex.StackOfCell;

            foreach (PlayerHexagon playerHexagon in listPlayerHexagonMerge)
            {
                if (hexagonStack.CheckContainPlayerHexagon(playerHexagon))
                {
                    hexagonStack.RemovePlayerHexagon(playerHexagon);
                }
            }
        }
    }

    private List<PlayerHexagon> GetPlayerHexagonNeedMerge(Color topColorOfStackAtGridHexagon, List<GridHexagon> neighborGridHexagonSameTopColor)
    {
        //Get All PlayerHexagon need merge
        List<PlayerHexagon> listPlayerHexagonMerge = new List<PlayerHexagon>();
        foreach (GridHexagon gridHex in neighborGridHexagonSameTopColor)
        {
            HexagonStack hexagonStack = gridHex.StackOfCell;
            for (int i = hexagonStack.Hexagons.Count - 1; i >= 0; i--)
            {
                PlayerHexagon playerHexagon = hexagonStack.Hexagons[i];
                if (playerHexagon.Color == topColorOfStackAtGridHexagon)
                {
                    listPlayerHexagonMerge.Add(playerHexagon);
                    playerHexagon.SetParent(null);
                }
            }
        }

        return listPlayerHexagonMerge;
    }

    private List<GridHexagon> GetHexagonStackOfNeighborSameTopColor(List<GridHexagon> listNeighborGridHexagons, Color topColorOfStackAtGridHexagon)
    {
        //Get List Neighbor have stack have same top color
        List<GridHexagon> neighborGridHexagonSameTopColor = new List<GridHexagon>();
        foreach (GridHexagon neighborGridHexagon in listNeighborGridHexagons)
        {
            if (neighborGridHexagon.StackOfCell.GetTopHexagonColor() == topColorOfStackAtGridHexagon)
            {
                neighborGridHexagonSameTopColor.Add(neighborGridHexagon);
            }
        }

        return neighborGridHexagonSameTopColor;
    }

    private List<GridHexagon> GetNeighborGidHexagon(GridHexagon gridHexagon)
    {

        Collider[] neighborGridCellColliders = Physics.OverlapSphere(gridHexagon.transform.position, 2, gridHexagonLayerMask);
        //OverlapSphere: Computes and stores colliders touching or inside the sphere. 
        // 2 because gridHexagon size (1.7321, 2, 0) 2 radius is good for detecech neighbors

        //Get A list of neighbor grid hexagon, that are occupied
        List<GridHexagon> listNeighborGridHexagons = new List<GridHexagon>();
        foreach (Collider collider in neighborGridCellColliders)
        {
            GridHexagon neighborGridHexagon = collider.GetComponent<GridHexagon>();

            if (neighborGridHexagon.IsOccupied == false) continue;
            if (neighborGridHexagon == gridHexagon) continue;

            listNeighborGridHexagons.Add(neighborGridHexagon);            
        }

        return listNeighborGridHexagons;
    }

    private List<GridHexagon> UpdateNeighborGridHexagonSameTopColor_S2(GridHexagon gridHexagon, List<GridHexagon> neighborGridHexagonSameTopColor)
    {
        //Add gridHexagon to list
        //Compare and by number simillar color
        //Compare and by number of top color
        //List<GridHexagon> listTest = new List<GridHexagon>();
        //listTest.AddRange(neighborGridHexagonSameTopColor);
        //listTest.Add(gridHexagon);

        //listTest.OrderByDescending(gridHex => gridHex.StackOfCell.GetNumberSimilarColor()).ThenBy(gridHex => gridHex.StackOfCell.GetNumberTopPlayerHexagonSameColor());
        //return listTest;

        //Add gridHexagon to list
        List<GridHexagon> allGridHexagon = new List<GridHexagon>();
        allGridHexagon.AddRange(neighborGridHexagonSameTopColor);
        allGridHexagon.Add(gridHexagon);

        return allGridHexagon
            //Compare and by number simillar color
            .OrderByDescending(gridHex => gridHex.StackOfCell.GetNumberSimilarColor())
            //Compare and by number of top color
            .ThenBy(gridHex => gridHex.StackOfCell.GetNumberTopPlayerHexagonSameColor())
            .ToList();
    }

    //private IEnumerator UpdateNeighborGridHexagonSameTopColor_S3(GridHexagon gridHexagon, List<GridHexagon> neighborGridHexagonSameTopColor)
    //{
    //    Debug.Log("UpdateNeighborGridHexagonSameTopColor_S3");
    //    //At [1] ensure all neighbor already add for loop merge
    //    //Test case all neighbor have similar color > 1, current == 1 => Merge all inside Curr Grid
    //    //Test case all neighbor have similar color > 1, current > 1 => Merge all inside Curr Grid
    //    //Test case all neighbor have similar color == 1, current == 1 => Merge all inside Curr Grid
    //    //Test case all neighbor have similar color == 1, current > 1 => Merge (all - 1) inside current => Merge current to the remaing Neigbor (add Remaining to last of listGridHexagonNeedUpdate)
    //    //Test case all have similar color > 1. Only one neighbor == 1 => Merge all inside Curr Grid and continue merge inside neibor == 1

    //    //List<GridHexagon> neighborClone = new List<GridHexagon>();
    //    //neighborClone.AddRange(neighborGridHexagonSameTopColor);

    //    List<GridHexagon> listOne = GetNeighborGridHexagonHaveOneSimilarColor(neighborGridHexagonSameTopColor);
    //    List<GridHexagon> listThanOne = GetNeighborGridHexagonHaveThanOneSimilarColor(neighborGridHexagonSameTopColor);

    //    if(listThanOne.Count == neighborGridHexagonSameTopColor.Count)
    //    {
    //        return listThanOne;
    //    }
    //    else if(listOne.Count == neighborGridHexagonSameTopColor.Count)
    //    {
    //        if(gridHexagon.StackOfCell.GetNumberSimilarColor() > 1)
    //        {
    //            GridHexagon firstGridHex = listOne[0];
    //            listOne.RemoveAt(0);
    //            listGridHexagonNeedUpdate.Add(firstGridHex);
    //        }
    //        return listOne;
    //    }
    //}

    private IEnumerator IE_AlgorithmMerge_3(GridHexagon gridHexagon, List<GridHexagon> neighborGridHexagonSameTopColor)
    {
        //At [1] ensure all neighbor already add for loop merge
        //Test case all neighbor have similar color > 1, current == 1 => Merge all inside Curr Grid
        //Test case all neighbor have similar color > 1, current > 1 => Merge all inside Curr Grid
        //Test case all neighbor have similar color == 1, current == 1 => Merge all inside Curr Grid
        //Test case all neighbor have similar color == 1, current > 1 => Merge (all - 1) inside current => Merge current to the remaing Neigbor (add Remaining to last of listGridHexagonNeedUpdate)
        //Test case than 1 neighbor > 1 and than 1 neibor == 1, current == 1 => Merge all inside Curr Grid
        //Test case than 1 neighbor > 1 and than 1 neibor == 1, current > 1 => Merge (all - 1 neighbor = 1) inside Curr Grid => (add Remaining to last of listGridHexagonNeedUpdate)

        //List<GridHexagon> neighborClone = new List<GridHexagon>();
        //neighborClone.AddRange(neighborGridHexagonSameTopColor);

        HexagonStack stack = gridHexagon.StackOfCell;

        List<GridHexagon> listOne = GetNeighborGridHexagonHaveOneSimilarColor(neighborGridHexagonSameTopColor);
        List<GridHexagon> listThanOne = GetNeighborGridHexagonHaveThanOneSimilarColor(neighborGridHexagonSameTopColor);

        if (listThanOne.Count == neighborGridHexagonSameTopColor.Count)
        {
            yield return IE_MergePlayerHexagonsToStack(stack, listThanOne);
        }
        else if (listOne.Count == neighborGridHexagonSameTopColor.Count)
        {
            if (stack.GetNumberSimilarColor() > 1)
            {
                GridHexagon firstGridHex = listOne[0];
                listOne.RemoveAt(0);
                yield return IE_MergePlayerHexagonsToStack(stack, listOne);
                listGridHexagonNeedUpdate.Add(firstGridHex);
                yield break; //Process continue with firstGridHex;
            }
            else
            {
                yield return IE_MergePlayerHexagonsToStack(stack, listOne);
            }
        }
        else
        {
            if (stack.GetNumberSimilarColor() > 1)
            {
                GridHexagon firstGridHexOneSimilar = listOne[0];
                neighborGridHexagonSameTopColor.Remove(firstGridHexOneSimilar);
                yield return IE_MergePlayerHexagonsToStack(stack, neighborGridHexagonSameTopColor);
                listGridHexagonNeedUpdate.Add(firstGridHexOneSimilar);
                yield break;
            }  
            else
            {
                yield return IE_MergePlayerHexagonsToStack(stack, neighborGridHexagonSameTopColor);
            }
        }

        yield return IE_RemovePlayerHexagonsFromStack_v2(stack);
        listGridHexagonNeedUpdate.Add(gridHexagon);
    }

    private List<GridHexagon> GetNeighborGridHexagonHaveOneSimilarColor(List<GridHexagon> neighborGridHexagonSameTopColor)
    {
        List<GridHexagon> gridHexagons = new List<GridHexagon>();
        foreach (GridHexagon gridHexagon in neighborGridHexagonSameTopColor)
        {
            if (gridHexagon.StackOfCell.GetNumberSimilarColor() == 1)
            {  
                gridHexagons.Add(gridHexagon);
            }
        }

        return gridHexagons;
    }

    private List<GridHexagon> GetNeighborGridHexagonHaveThanOneSimilarColor(List<GridHexagon> neighborGridHexagonSameTopColor)
    {
        List<GridHexagon> gridHexagons = new List<GridHexagon>();
        foreach (GridHexagon gridHexagon in neighborGridHexagonSameTopColor)
        {
            if (gridHexagon.StackOfCell.GetNumberSimilarColor() > 1)
            {
                gridHexagons.Add(gridHexagon);
            }
        }

        return gridHexagons;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

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
            Debug.Log(merge.GetInstanceID());
            listGridHexagonNeedUpdate.Remove(merge);
            if(merge.IsOccupied)
                yield return IE_CheckForMerge(merge);
            Debug.Log("2222");
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
            listGridHexagonNeedUpdate.AddRange(neighborGridHexagonSameTopColor);
        }
        //End

        List<PlayerHexagon> listPlayerHexagonMerge = GetPlayerHexagonNeedMerge(topColorOfStackAtGridHexagon, neighborGridHexagonSameTopColor);
        RemovePlayerHexagonFromOldStack(neighborGridHexagonSameTopColor, listPlayerHexagonMerge);

        //Merge action
        //Merge solution 1: Merge neighbor cells towards this cell {merge everything inside current grid hexagon}
        MergePlayerHexagon(stackHexagon, listPlayerHexagonMerge);
        yield return new WaitForSeconds(0.2f + listPlayerHexagonMerge.Count * 0.01f); //0.2f time anim + 0.01 time delay.setDelay(transform.GetSiblingIndex() * 0.01f);
        //Merge solution 2: Merge everyting on the cell that has the lowest amount of that smame hexagon color or the one that has a bigger amount [using the one that has a smaller amount is actually better]
        //Check stack completed when >= 10 similar hexagons
        List<PlayerHexagon> listPlayerHexagonSimilarColor = GetPlayerHexagonSimilarColor(stackHexagon, topColorOfStackAtGridHexagon);
        if (listPlayerHexagonSimilarColor.Count >= 10)
        {
            yield return IE_RemovePlayerHexagonsFromStack(listPlayerHexagonSimilarColor, stackHexagon);
            //After delete some PlayerHexagon from stack so need recheck
            //if (gridHexagon.IsOccupied)
            //    listGridHexagonNeedUpdate.Add(gridHexagon);
            Debug.Log("here");
            listGridHexagonNeedUpdate.Add(gridHexagon);
        }
        //Check and update neighbor cells
        //repeat processing
        //foreach(GridHexagon gridHex in listNeighborGridHexagons)
        //{
        //    if(gridHex.IsOccupied)
        //    {
        //        listGridHexagonNeedUpdate.Add(gridHex);
        //    }
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
            //DestroyImmediate(playerHexagon.gameObject);
            offsetDelayTime += 0.01f;

            stackHexagon.RemovePlayerHexagon(playerHexagon);
            listPlayerHexagonSimilarColor.RemoveAt(0);
        }

        yield return new WaitForSeconds(0.2f + (numberOfPlayerHexagon + 1) * 0.01f);
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
        float yOfCurrentGridHexagon = stackHexagon.Hexagons.Count * 0.2f;
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackMerger : MonoBehaviour
{
    public Action OnStackMergeCompleted;

    [SerializeField]
    private LayerMask gridHexagonLayerMask;
    public List<GridHexagon> listGridHexagonNeedUpdate = new List<GridHexagon>();
    public Stack<GridHexagonNode> nodeVisited = new Stack<GridHexagonNode>();
    public int idxRootVisited = 0;

    private IStackSphereRadius _IStackSphereRadius; 
    private bool _CanMerge = false;
    private bool _HasIEProcessing = false;
    public void OnInit(IStackSphereRadius StackSphereRadius)
    {
        _IStackSphereRadius = StackSphereRadius;
    }
    public void OnResert()
    {
        listGridHexagonNeedUpdate.Clear();
    }

    public void OnPauseGame()
    {
        _CanMerge = false;
    }

    public void OnPlayGame()
    {
        _CanMerge = true;
    }

    public void EventOnStackPlacedOnGridHexagon(GridHexagon gridHexagon)
    {
        //listGridHexagonNeedUpdate.Add(gridHexagon);        
        listGridHexagonNeedUpdate.Insert(idxRootVisited, gridHexagon);
        if (listGridHexagonNeedUpdate.Count == 1 && _CanMerge == true && _HasIEProcessing == false)
        {
            StartCoroutine(IE_OnStackPlacedOnGridHexagon(gridHexagon));
        }
    }
    public void OnStackPlacedOnGridHexagon(GridHexagon gridHexagon)
    {
        Debug.Log("OnStackPlacedOnGridHexagon");
        listGridHexagonNeedUpdate.Insert(idxRootVisited, gridHexagon);
        //listGridHexagonNeedUpdate.Add(gridHexagon);

        if (listGridHexagonNeedUpdate.Count == 1 && _CanMerge == true && _HasIEProcessing == false)
        {
            StartCoroutine(IE_OnStackPlacedOnGridHexagon(gridHexagon));
        }
    }
    private IEnumerator IE_OnStackPlacedOnGridHexagon(GridHexagon gridHexagon)
    {
        _HasIEProcessing = true;
        while (listGridHexagonNeedUpdate.Count > 0)
        {
            idxRootVisited = listGridHexagonNeedUpdate.Count - 1;
            GridHexagon merge = listGridHexagonNeedUpdate[idxRootVisited];
            listGridHexagonNeedUpdate.Remove(merge);

            if (merge.CheckOccupied())
            {                
                CreateTree(merge);               
                yield return IE_HandleTree_Algorithm_1();
                //yield return IE_HandleTree();
                //yield return IE_CheckForMerge(merge);
            }
        }

        _HasIEProcessing = false;
        OnStackMergeCompleted?.Invoke();
    }

    private IEnumerator IE_CheckForMerge(GridHexagon gridHexagon)
    {
        Debug.Log("IE_Check For Merge");
        //Get Top Color of Stack
        StackHexagon stackHexagon = gridHexagon.StackOfCell;
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
        List<Hexagon> listPlayerHexagonMerge = GetPlayerHexagonNeedMerge(topColorOfStackAtGridHexagon, neighborGridHexagonSameTopColor);
        RemovePlayerHexagonFromOldStack(neighborGridHexagonSameTopColor, listPlayerHexagonMerge);
        MergePlayerHexagon(stackHexagon, listPlayerHexagonMerge);
        yield return new WaitForSeconds(0.2f + listPlayerHexagonMerge.Count * 0.01f); //0.2f time anim + 0.01 time delay.setDelay(transform.GetSiblingIndex() * 0.01f);

        //Merge solution 2: Merge everyting on the cell that has the lowest amount of that smame hexagon color or the one that has a bigger amount [using the one that has a smaller amount is actually better]
        //neighborGridHexagonSameTopColor = UpdateNeighborGridHexagonSameTopColor_S2(gridHexagon, neighborGridHexagonSameTopColor).ToList();
        //gridHexagon = neighborGridHexagonSameTopColor[^1];
        //neighborGridHexagonSameTopColor.Remove(gridHexagon);
        //stackHexagon = gridHexagon.StackOfCell;
        //topColorOfStackAtGridHexagon = stackHexagon.GetTopHexagonColor();

        //Merge solution 3: Target Merge craete stack have One Color if can. Get Current Grid is center of merge process 
        //yield return IE_AlgorithmMerge_3(gridHexagon, neighborGridHexagonSameTopColor);
        //neighborGridHexagonSameTopColor = UpdateNeighborGridHexagonSameTopColor_S3(gridHexagon, neighborGridHexagonSameTopColor);
        //yield return IE_MergePlayerHexagonsToStack(stackHexagon, neighborGridHexagonSameTopColor);

        ////Check stack completed when >= 10 similar hexagons
        List<Hexagon> listPlayerHexagonSimilarColor = GetPlayerHexagonSimilarColor(stackHexagon);
        if (listPlayerHexagonSimilarColor.Count >= 10)
        {
            yield return IE_RemovePlayerHexagonsFromStack(listPlayerHexagonSimilarColor, stackHexagon);
            //After delete some Hexagon from stack so need recheck
            if (gridHexagon.CheckOccupied())
                listGridHexagonNeedUpdate.Add(gridHexagon);
            //listGridHexagonNeedUpdate.Add(gridHexagon);
        }
    }

    private IEnumerator IE_RemovePlayerHexagonsFromStack(List<Hexagon> listPlayerHexagonSimilarColor, StackHexagon stackHexagon)
    {
        Debug.Log("RemovePlayerHexagonsFromStack" + listPlayerHexagonSimilarColor.Count);
        int numberOfPlayerHexagon = listPlayerHexagonSimilarColor.Count;
        float offsetDelayTime = 0;
        //Remove bottom to top
        while (listPlayerHexagonSimilarColor.Count > 0)
        {
            Hexagon playerHexagon = listPlayerHexagonSimilarColor[0];
            playerHexagon.SetParent(null);
            playerHexagon.TweenVanish(offsetDelayTime);
            offsetDelayTime += 0.01f;

            stackHexagon.RemovePlayerHexagon(playerHexagon);
            listPlayerHexagonSimilarColor.RemoveAt(0);
        }

        yield return new WaitForSeconds(0.2f + (numberOfPlayerHexagon + 1) * 0.01f);
    }

    private IEnumerator IE_RemovePlayerHexagonsFromStack_v2(StackHexagon stackHexagon)
    {
        List<Hexagon> listPlayerHexagonSimilarColor = GetPlayerHexagonSimilarColor(stackHexagon);
        int numberOfPlayerHexagon = listPlayerHexagonSimilarColor.Count;
        if (numberOfPlayerHexagon < 10)
            yield break;

        float offsetDelayTime = 0;
        //Remove bottom to top
        while (listPlayerHexagonSimilarColor.Count > 0)
        {
            Hexagon playerHexagon = listPlayerHexagonSimilarColor[0];
            playerHexagon.SetParent(null);
            playerHexagon.TweenVanish(offsetDelayTime);
            offsetDelayTime += 0.01f;

            stackHexagon.RemovePlayerHexagon(playerHexagon);
            listPlayerHexagonSimilarColor.RemoveAt(0);
        }

        yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (numberOfPlayerHexagon - 1) * GameConstants.HexagonConstants.TIME_DELAY);
    }

    private List<Hexagon> GetPlayerHexagonSimilarColor(StackHexagon stackHexagon)
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

    private IEnumerator IE_MergePlayerHexagonsToStack(StackHexagon stackHexagon, List<GridHexagon> neighborGridHexagon)
    {
        List<Hexagon> listPlayerHexagonMerge = GetPlayerHexagonNeedMerge(stackHexagon.GetTopHexagonColor(), neighborGridHexagon);
        RemovePlayerHexagonFromOldStack(neighborGridHexagon, listPlayerHexagonMerge);
        MergePlayerHexagon(stackHexagon, listPlayerHexagonMerge);
        stackHexagon.HideCanvas();
        yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (listPlayerHexagonMerge.Count - 1) * GameConstants.HexagonConstants.TIME_DELAY);
        stackHexagon.ShowCanvas();
    }

    private void MergePlayerHexagon(StackHexagon stackHexagon, List<Hexagon> listPlayerHexagonMerge)
    {
        float yOfCurrentGridHexagon = (stackHexagon.Hexagons.Count - 1) * GameConstants.HexagonConstants.HEIGHT;
        for (int i = 0; i < listPlayerHexagonMerge.Count; i++)
        {
            Hexagon playerHexagon = listPlayerHexagonMerge[i];
            stackHexagon.AddPlayerHexagon(playerHexagon); //have setparent inside addPlayerHexagon

            float yOffset = yOfCurrentGridHexagon + (i + 1) * GameConstants.HexagonConstants.HEIGHT;
            Vector3 localPos = Vector3.up * yOffset; //(0, yOffset, 0)
            playerHexagon.Configure(stackHexagon);
            playerHexagon.MoveToGridHexagon(localPos, i * GameConstants.HexagonConstants.TIME_DELAY);
        }
    }

    private static void RemovePlayerHexagonFromOldStack(List<GridHexagon> neighborGridHexagonSameTopColor, List<Hexagon> listPlayerHexagonMerge)
    {
        //Remove Hexagon need merge from Hexagon Stack contain before
        foreach (GridHexagon gridHex in neighborGridHexagonSameTopColor)
        {
            StackHexagon hexagonStack = gridHex.StackOfCell;

            foreach (Hexagon playerHexagon in listPlayerHexagonMerge)
            {
                if (hexagonStack.CheckContainPlayerHexagon(playerHexagon))
                {
                    hexagonStack.RemovePlayerHexagon(playerHexagon);
                }
            }
        }
    }

    private List<Hexagon> GetPlayerHexagonNeedMerge(Color topColorOfStackAtGridHexagon, List<GridHexagon> neighborGridHexagonSameTopColor)
    {
        //Get All Hexagon need merge
        List<Hexagon> listPlayerHexagonMerge = new List<Hexagon>();
        foreach (GridHexagon gridHex in neighborGridHexagonSameTopColor)
        {
            StackHexagon hexagonStack = gridHex.StackOfCell;
            for (int i = hexagonStack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon playerHexagon = hexagonStack.Hexagons[i];
                //if (playerHexagon.Color == topColorOfStackAtGridHexagon)
                //{
                //    listPlayerHexagonMerge.Add(playerHexagon);
                //    playerHexagon.SetParent(null);
                //}

                if (ColorUtils.ColorEquals(playerHexagon.Color, topColorOfStackAtGridHexagon))
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
            Color color1 = neighborGridHexagon.StackOfCell.GetTopHexagonColor();
            Color color2 = topColorOfStackAtGridHexagon;
            if (ColorUtils.ColorEquals(color1, color2))
            {
                neighborGridHexagonSameTopColor.Add(neighborGridHexagon);
            }
        }

        return neighborGridHexagonSameTopColor;
    }

    private List<GridHexagon> GetNeighborGidHexagon(GridHexagon gridHexagon)
    {
        float distance = _IStackSphereRadius.GetRadiusByGrid().x * 2;
        Collider[] neighborGridCellColliders = Physics.OverlapSphere(gridHexagon.transform.position, distance, gridHexagonLayerMask);

        //Get A list of neighbor grid hexagon, that are occupied
        List<GridHexagon> listNeighborGridHexagons = new List<GridHexagon>();
        foreach (Collider collider in neighborGridCellColliders)
        {
            GridHexagon neighborGridHexagon = collider.GetComponent<GridHexagon>();

            if (neighborGridHexagon.CheckOccupied() == false)
            {
                continue;
            }
            if (neighborGridHexagon == gridHexagon)
            {
                continue;
            }

            if (neighborGridHexagon.State == GridHexagonState.LOCK_BY_GOAL || neighborGridHexagon.State == GridHexagonState.LOCK_BY_ADS)
                continue;

            listNeighborGridHexagons.Add(neighborGridHexagon);
        }

        return listNeighborGridHexagons;
    }

    //private List<GridHexagon> UpdateNeighborGridHexagonSameTopColor_S2(GridHexagon gridHexagon, List<GridHexagon> neighborGridHexagonSameTopColor)
    //{
    //    //Add gridHexagon to list
    //    //Compare and by number simillar color
    //    //Compare and by number of top color
    //    //List<GridHexagon> listTest = new List<GridHexagon>();
    //    //listTest.AddRange(neighborGridHexagonSameTopColor);
    //    //listTest.Add(gridHexagon);

    //    //listTest.OrderByDescending(gridHex => gridHex.StackOfCell.GetNumberSimilarColor()).ThenBy(gridHex => gridHex.StackOfCell.GetNumberTopPlayerHexagonSameColor());
    //    //return listTest;

    //    //Add gridHexagon to list
    //    List<GridHexagon> allGridHexagon = new List<GridHexagon>();
    //    allGridHexagon.AddRange(neighborGridHexagonSameTopColor);
    //    allGridHexagon.Add(gridHexagon);

    //    return allGridHexagon
    //        //Compare and by number simillar color
    //        .OrderByDescending(gridHex => gridHex.StackOfCell.GetNumberSimilarColor())
    //        //Compare and by number of top color
    //        .ThenBy(gridHex => gridHex.StackOfCell.GetNumberTopPlayerHexagonSameColor())
    //        .ToList();
    //}

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

    //Merge and remove neighbor
    private IEnumerator IE_AlgorithmMerge_3(GridHexagon gridHexagon, List<GridHexagon> neighborGridHexagonSameTopColor)
    {
        Debug.Log("IE_AlgorithmMerge_3");
        //At [1] ensure all neighbor already add for loop merge
        //Test case all neighbor have similar color > 1, current == 1 => Merge all inside Curr Grid
        //Test case all neighbor have similar color > 1, current > 1 => Merge all inside Curr Grid
        //Test case all neighbor have similar color == 1, current == 1 => Merge all inside Curr Grid
        //Test case all neighbor have similar color == 1, current > 1 => Merge (all - 1) inside current => Merge current to the remaing Neigbor (add Remaining to last of listGridHexagonNeedUpdate)
        //Test case than 1 neighbor > 1 and than 1 neibor == 1, current == 1 => Merge all inside Curr Grid
        //Test case than 1 neighbor > 1 and than 1 neibor == 1, current > 1 => Merge (all - 1 neighbor = 1) inside Curr Grid => (add Remaining to last of listGridHexagonNeedUpdate)

        StackHexagon stack = gridHexagon.StackOfCell;

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
        if (gridHexagon.CheckOccupied())
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

    private void CreateTree(GridHexagon grid)
    {
        GridHexagonNode rootNode = new GridHexagonNode(grid, null);
        Stack<GridHexagonNode> queueNode = new Stack<GridHexagonNode>();
        queueNode.Push(rootNode);

        int k = 100;
        while (queueNode.Count > 0)
        {
            k--;

            if (k <= 0)
            {
                Debug.LogError("Some thing wrong here");
                return;
            }

            GridHexagonNode nodeVisiting = queueNode.Pop();

            if (nodeVisited.Contains(nodeVisiting))
            {
                continue;
            }

            nodeVisited.Push(nodeVisiting);

            List<GridHexagonNode> chilNodes = CreateEdge(nodeVisiting);

            foreach (GridHexagonNode node in chilNodes)
            {
                //if (queueNode.Contains(node))
                //    continue;

                //if (nodeVisited.Contains(node))
                //    continue;

                if (CheckContainNode(queueNode.ToList(), node))
                {
                    continue;
                }

                if (CheckContainNode(nodeVisited.ToList(), node))
                {
                    continue;
                }

                queueNode.Push(node);
                nodeVisiting.AddChildNode(node);
            }
        }
    }

    private bool CheckContainNode(List<GridHexagonNode> listNode, GridHexagonNode node)
    {
        for (int i = 0; i < listNode.Count; i++)
        {
            GridHexagon grid1 = listNode[i].GetGridHexagon();
            GridHexagon grid2 = node.GetGridHexagon();

            if (grid1.gameObject.CompareObject(grid2.gameObject))
            {
                return true;
            }
        }

        return false;
    }

    private List<GridHexagonNode> CreateEdge(GridHexagonNode EdgeNode)
    {
        List<GridHexagonNode> result = new List<GridHexagonNode>();
        GridHexagon grid = EdgeNode.GetGridHexagon();
        Color topColor = grid.StackOfCell.GetTopHexagonColor();
        List<GridHexagon> neighbor = GetNeighborGidHexagon(grid);

        if (neighbor.Count == 0)
        {
            return result;
        }

        List<GridHexagon> neighborSameColor = GetHexagonStackOfNeighborSameTopColor(neighbor, topColor);

        //Ưu tiên xử lý OneSimiler
        //Đảm bảo ThanOne Similar vào Queue trước => được xem xét trước => vào visited trước => được handle sau 
        List<GridHexagon> neighborThanOneSimilar = GetNeighborGridHexagonHaveThanOneSimilarColor(neighborSameColor);
        foreach (GridHexagon item in neighborThanOneSimilar)
        {
            GridHexagonNode node = new GridHexagonNode(item, EdgeNode);
            result.Add(node);
        }

        List<GridHexagon> neighborOneSimilar = GetNeighborGridHexagonHaveOneSimilarColor(neighborSameColor);
        foreach (GridHexagon item in neighborOneSimilar)
        {
            GridHexagonNode node = new GridHexagonNode(item, EdgeNode);
            result.Add(node);
        }

        return result;
    }

    private IEnumerator IE_HandleTree_Algorithm_1()
    {
        GridHexagonNode edgeNode = null;
        int k = 100;
        while (nodeVisited.Count > 0)
        {
            k--;

            if (k <= 0)
            {
                Debug.LogError("Some thing wrong here IE_HandleTree");
                yield break;
            }

            GridHexagonNode node = nodeVisited.Pop();

            if (node.IsRootNode)
            {
                edgeNode = node;
                continue;
            }
            else if (node.IsLeafNode)
            {
                continue;
            }
            else if (edgeNode == null)
            {
                edgeNode = node;
                yield return IE_MergeHexagonToEdgeNode(edgeNode);
            }
            else if (edgeNode != null)
            {
                GridHexagon gridOfEdge = edgeNode.GetGridHexagon();
                GridHexagon gridOfNode = node.GetGridHexagon();

                if (gridOfEdge.gameObject.CompareObject(gridOfNode.gameObject))
                {
                    continue;
                }
                else
                {
                    edgeNode = node;
                    yield return IE_MergeHexagonToEdgeNode(edgeNode);
                }
            }
        }

        //Root Node
        yield return IE_MergeRootNode_Algorithm_1(edgeNode);
    }

    //Duyệt cây từ cạnh lên
    private IEnumerator IE_HandleTree()
    {
        GridHexagonNode edgeNode = null;
        int k = 100;
        while (nodeVisited.Count > 0)
        {
            k--;

            if (k <= 0)
            {
                Debug.LogError("Some thing wrong here IE_HandleTree");
                yield break;
            }

            GridHexagonNode node = nodeVisited.Pop();

            if (node.IsRootNode)
            {
                edgeNode = node;
                continue;
            }
            else if (node.IsLeafNode)
            {
                continue;
            }
            else if (edgeNode == null)
            {
                edgeNode = node;
                yield return IE_MergeHexagonToEdgeNode(edgeNode);
            }
            else if (edgeNode != null)
            {
                GridHexagon gridOfEdge = edgeNode.GetGridHexagon();
                GridHexagon gridOfNode = node.GetGridHexagon();

                if (gridOfEdge.gameObject.CompareObject(gridOfNode.gameObject))
                {
                    continue;
                }
                else
                {
                    edgeNode = node;
                    yield return IE_MergeHexagonToEdgeNode(edgeNode);
                }
            }
        }

        //Root Node
        //yield return IE_CheckForMerge(edgeNode.GetGridHexagon());
        yield return IE_MergeRootNode(edgeNode);
    }

    private IEnumerator IE_MergeHexagonToEdgeNode(GridHexagonNode edgeNode)
    {
        GridHexagon grid = edgeNode.GetGridHexagon();
        Color color = grid.StackOfCell.GetTopHexagonColor();

        GridHexagonNode[] childNodes = edgeNode.GetChildNodes();
        List<GridHexagon> gridHexagons = new List<GridHexagon>();
        foreach (GridHexagonNode node in childNodes)
        {
            gridHexagons.Add(node.GetGridHexagon());
        }

        List<Hexagon> hexagons = GetPlayerHexagonNeedMerge(color, gridHexagons);
        RemovePlayerHexagonFromOldStack(gridHexagons, hexagons);
        MergePlayerHexagon(grid.StackOfCell, hexagons);
        grid.StackOfCell.HideCanvas();
        yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (hexagons.Count - 1) * GameConstants.HexagonConstants.TIME_DELAY);
        grid.StackOfCell.ShowCanvas();

        foreach (GridHexagon gridChild in gridHexagons)
        {
            if (gridChild.CheckOccupied() && !CheckContainGrid(listGridHexagonNeedUpdate, gridChild))
            {
                listGridHexagonNeedUpdate.Add(gridChild);
            }
        }
    }

    private bool CheckContainGrid(List<GridHexagon> gridHexagons, GridHexagon grid)
    {
        for (int i = 0; i < gridHexagons.Count; i++)
        {
            GridHexagon grid1 = gridHexagons[i];

            if (grid1.gameObject.CompareObject(grid.gameObject))
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator IE_MergeRootNode_Algorithm_1(GridHexagonNode rootNode)
    {
        GridHexagon grid = rootNode.GetGridHexagon();
        StackHexagon stack = grid.StackOfCell;
        Color color = grid.StackOfCell.GetTopHexagonColor();

        GridHexagonNode[] childNodes = rootNode.GetChildNodes();

        if (childNodes.Length == 0)
        {
            Debug.Log("Root not have any child");
            yield break;
        }

        List<GridHexagon> gridHexagons = new List<GridHexagon>();
        foreach (GridHexagonNode node in childNodes)
        {
            gridHexagons.Add(node.GetGridHexagon());
        }
        
        yield return IE_MergePlayerHexagonsToStack(stack, gridHexagons);
        yield return IE_RemovePlayerHexagonsFromStack_v2(stack);

        foreach (GridHexagon gridHexagon in gridHexagons)
        {
            if(gridHexagon.CheckOccupied())
            {
                listGridHexagonNeedUpdate.Add(gridHexagon);
            }
        }

        if (grid.CheckOccupied())
        {
            listGridHexagonNeedUpdate.Add(grid);
        }
    }

    private IEnumerator IE_MergeRootNode(GridHexagonNode rootNode)
    {
        GridHexagon grid = rootNode.GetGridHexagon();
        StackHexagon stack = grid.StackOfCell;
        Color color = grid.StackOfCell.GetTopHexagonColor();

        GridHexagonNode[] childNodes = rootNode.GetChildNodes();

        if(childNodes.Length == 0)
        {
            Debug.Log("Root not have any child");
            yield break;
        }

        List<GridHexagon> gridHexagons = new List<GridHexagon>();
        foreach (GridHexagonNode node in childNodes)
        {
            gridHexagons.Add(node.GetGridHexagon());
        }

        List<GridHexagon> listOne = GetNeighborGridHexagonHaveOneSimilarColor(gridHexagons);
        List<GridHexagon> listThanOne = GetNeighborGridHexagonHaveThanOneSimilarColor(gridHexagons);

        if (listThanOne.Count == gridHexagons.Count)
        {
            yield return IE_MergePlayerHexagonsToStack(stack, listThanOne);
        }
        else if (listOne.Count == gridHexagons.Count)
        {
            if (stack.GetNumberSimilarColor() > 1)
            {
                GridHexagon firstGrid = listOne[0];
                listOne.RemoveAt(0);
                yield return IE_MergePlayerHexagonsToStack(stack, listOne);

                foreach (GridHexagon gridH in gridHexagons)
                {
                    if (gridH.CheckOccupied())
                        listGridHexagonNeedUpdate.Add(gridH);
                }

                listGridHexagonNeedUpdate.Add(firstGrid);

                yield break;
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
                gridHexagons.Remove(firstGridHexOneSimilar);
                yield return IE_MergePlayerHexagonsToStack(stack, gridHexagons);

                foreach (GridHexagon gridH in gridHexagons)
                {
                    if(gridH.CheckOccupied())
                        listGridHexagonNeedUpdate.Add(gridH);
                }

                listGridHexagonNeedUpdate.Add(firstGridHexOneSimilar);
                yield break;
            }
            else
            {
                yield return IE_MergePlayerHexagonsToStack(stack, gridHexagons);
            }
        }
        Debug.Log("Omg");
        yield return IE_RemovePlayerHexagonsFromStack_v2(stack);

        if (grid.CheckOccupied())
        {
            listGridHexagonNeedUpdate.Add(grid);
        }

        foreach (GridHexagon gridH in gridHexagons)
        {
            if (gridH.CheckOccupied())
                listGridHexagonNeedUpdate.Add(gridH);
        }
    }
}

public class GridHexagonNode
{
    private GridHexagonNode _EdgeNode;
    private GridHexagon _GridHexagon;
    private List<GridHexagonNode> _ChildNodes;
    public bool IsRootNode => _EdgeNode == null;
    public bool IsLeafNode => _ChildNodes.Count == 0;

    public GridHexagonNode(GridHexagon Content, GridHexagonNode EdgeNode = null)
    {
        this._GridHexagon = Content;
        this._EdgeNode = EdgeNode;
        this._ChildNodes = new List<GridHexagonNode>();
    }    

    public GridHexagonNode[] GetChildNodes()
    {
        return _ChildNodes.ToArray();
    }

    public void AddChildNode(GridHexagonNode childNode)
    {
        this._ChildNodes.Add(childNode);
    }

    public void UpdateChildNode(List<GridHexagonNode> childNodes)
    {
        this._ChildNodes = childNodes;
    }

    public GridHexagonNode GetEdgeNode()
    {
        return _EdgeNode;
    }

    public GridHexagon GetGridHexagon()
    {
        return _GridHexagon;
    }
}

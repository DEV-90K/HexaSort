using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeController : MonoBehaviour
{
    public static Action OnTurnCompleted;

    private TreeController treeController = new TreeController();
    private List<GridHexagon> listGridHexagonNeedUpdate = new List<GridHexagon>();
    private int idxRootVisited = 0;
    private bool _hasProcessing = false;

    private void OnEnable()
    {
        StackController.OnStackPlaced += StackController_OnStackPlaced;
    }

    private void OnDisable()
    {
        StackController.OnStackPlaced -= StackController_OnStackPlaced;
    }

    private void StackController_OnStackPlaced(GridHexagon grid)
    {
        Debug.Log("Challenge Controller StackController_OnStackPlaced");
        OnStackPlacedOnGridHexagon(grid);
    }
    private void OnStackPlacedOnGridHexagon(GridHexagon gridHexagon)
    {
        listGridHexagonNeedUpdate.Insert(idxRootVisited, gridHexagon);

        if (listGridHexagonNeedUpdate.Count == 1 && _hasProcessing == false)
        {
            StartCoroutine(IE_OnStackPlacedOnGridHexagon());
        }
    }

    private IEnumerator IE_OnStackPlacedOnGridHexagon()
    {
        _hasProcessing = true;
        while (listGridHexagonNeedUpdate.Count > 0)
        {
            idxRootVisited = listGridHexagonNeedUpdate.Count - 1;
            GridHexagon merge = listGridHexagonNeedUpdate[idxRootVisited];
            listGridHexagonNeedUpdate.Remove(merge);

            if (merge.CheckOccupied())
            {
                Stack<GridHexagonNode> nodeVisited = treeController.CreateTree(merge);
                yield return IE_MergeTree(nodeVisited);
            }
        }
        _hasProcessing = false;
        OnTurnCompleted?.Invoke();
    }

    private IEnumerator IE_MergeTree(Stack<GridHexagonNode> nodeVisited)
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
                yield return IE_MergeEdge(edgeNode);
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
                    yield return IE_MergeEdge(edgeNode);
                }
            }
        }

        //Root Node
        yield return IE_MergeRoot(edgeNode);
    }

    private IEnumerator IE_MergeEdge(GridHexagonNode edgeNode)
    {
        GridHexagon grid = edgeNode.GetGridHexagon();
        Color color = grid.StackOfCell.GetTopHexagonColor();

        GridHexagonNode[] childNodes = edgeNode.GetChildNodes();
        List<GridHexagon> gridHexagons = new List<GridHexagon>();
        foreach (GridHexagonNode node in childNodes)
        {
            gridHexagons.Add(node.GetGridHexagon());
        }

        List<Hexagon> hexagons = GridManager.Instance.GetPlayerHexagonNeedMerge(color, gridHexagons);
        GridManager.Instance.RemovePlayerHexagonFromOldStack(gridHexagons, hexagons);
        StackManager.Instance.MergePlayerHexagon(grid.StackOfCell, hexagons);

        grid.StackOfCell.HideCanvas();
        yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (hexagons.Count - 1) * GameConstants.HexagonConstants.TIME_DELAY);
        grid.StackOfCell.ShowCanvas();

        foreach (GridHexagon gridChild in gridHexagons)
        {
            if (gridChild.CheckOccupied() && !GridManager.Instance.CheckContainGrid(listGridHexagonNeedUpdate, gridChild))
            {
                listGridHexagonNeedUpdate.Add(gridChild);
            }
        }
    }

    private IEnumerator IE_MergeRoot(GridHexagonNode rootNode)
    {
        GridHexagon grid = rootNode.GetGridHexagon();
        StackHexagon stack = grid.StackOfCell;

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

        yield return GridManager.Instance.IE_MergePlayerHexagonsToStack(stack, gridHexagons);

        List<Hexagon> listPlayerHexagonSimilarColor = StackManager.Instance.GetPlayerHexagonSimilarColor(stack);
        int numberOfPlayerHexagon = listPlayerHexagonSimilarColor.Count;
        if (numberOfPlayerHexagon >= 10)
        {
            yield return StackManager.Instance.IE_RemovePlayerHexagonsFromStack(stack);
        }


        foreach (GridHexagon gridHexagon in gridHexagons)
        {
            if (gridHexagon.CheckOccupied())
            {
                listGridHexagonNeedUpdate.Add(gridHexagon);
            }
            else if (listGridHexagonNeedUpdate.Contains(gridHexagon))
            {
                listGridHexagonNeedUpdate.Remove(gridHexagon);
            }
        }

        if (grid.CheckOccupied())
        {
            listGridHexagonNeedUpdate.Add(grid);
        }
        else if (listGridHexagonNeedUpdate.Contains(grid))
        {
            listGridHexagonNeedUpdate.Remove(grid);
        }
    }
}

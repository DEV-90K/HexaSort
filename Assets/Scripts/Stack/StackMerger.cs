using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackMerger : MonoBehaviour
{    
    public IEnumerator IE_Merger(StackHexagon source, StackHexagon target)
    {
        List<Hexagon> hexagons = GetHexagons(source);
        yield break;
    }

    private List<Hexagon> GetHexagons(StackHexagon stack)
    {
        List<Hexagon> hexagons = new List<Hexagon>();
        Color color = stack.GetTopHexagonColor();
        foreach (Hexagon hex in stack.Hexagons)
        {
            if(ColorUtils.ColorEquals(color, hex.Color))
            {
                hexagons.Add(hex);
            }
        }

        return hexagons;
    }

    //[SerializeField]
    //private LayerMask gridHexagonLayerMask;
    //public List<GridHexagon> listGridHexagonNeedUpdate = new List<GridHexagon>();
    //public Stack<GridHexagonNode> nodeVisited = new Stack<GridHexagonNode>();
    //public int idxRootVisited = 0;

    //private IStackSphereRadius _IStackSphereRadius; 
    //private bool _CanMerge = false;
    //private bool _HasIEProcessing = false;
    //public void OnInit(IStackSphereRadius StackSphereRadius)
    //{
    //    _IStackSphereRadius = StackSphereRadius;
    //}
    //public void OnResert()
    //{
    //    listGridHexagonNeedUpdate.Clear();
    //}

    //public void OnPauseGame()
    //{
    //    _CanMerge = false;
    //}

    //public void OnPlayGame()
    //{
    //    _CanMerge = true;
    //}

    //public void EventOnStackPlacedOnGridHexagon(GridHexagon gridHexagon)
    //{
    //    //listGridHexagonNeedUpdate.Add(gridHexagon);        
    //    listGridHexagonNeedUpdate.Insert(idxRootVisited, gridHexagon);
    //    if (listGridHexagonNeedUpdate.Count == 1 && _CanMerge == true && _HasIEProcessing == false)
    //    {
    //        StartCoroutine(IE_OnStackPlacedOnGridHexagon(gridHexagon));
    //    }
    //}
    //public void OnStackPlacedOnGridHexagon(GridHexagon gridHexagon)
    //{
    //    Debug.Log("OnStackPlacedOnGridHexagon");
    //    listGridHexagonNeedUpdate.Insert(idxRootVisited, gridHexagon);
    //    //listGridHexagonNeedUpdate.Add(gridHexagon);

    //    if (listGridHexagonNeedUpdate.Count == 1 && _CanMerge == true && _HasIEProcessing == false)
    //    {
    //        StartCoroutine(IE_OnStackPlacedOnGridHexagon(gridHexagon));
    //    }
    //}
    //private IEnumerator IE_OnStackPlacedOnGridHexagon(GridHexagon gridHexagon)
    //{
    //    _HasIEProcessing = true;
    //    while (listGridHexagonNeedUpdate.Count > 0)
    //    {
    //        idxRootVisited = listGridHexagonNeedUpdate.Count - 1;
    //        GridHexagon merge = listGridHexagonNeedUpdate[idxRootVisited];
    //        listGridHexagonNeedUpdate.Remove(merge);

    //        if (merge.CheckOccupied())
    //        {
    //            //CreateTree(merge);               
    //            TreeController tree = new TreeController();
    //            nodeVisited = tree.CreateTree(merge);
    //            yield return IE_HandleTree_Algorithm_1();
    //            //yield return IE_HandleTree();
    //            //yield return IE_CheckForMerge(merge);
    //        }
    //    }

    //    _HasIEProcessing = false;
    //    OnStackMergeCompleted?.Invoke();
    //}
    //private IEnumerator IE_RemovePlayerHexagonsFromStack(StackHexagon stackHexagon)
    //{
    //    List<Hexagon> listPlayerHexagonSimilarColor = GetPlayerHexagonSimilarColor(stackHexagon);
    //    int numberOfPlayerHexagon = listPlayerHexagonSimilarColor.Count;
    //    if (numberOfPlayerHexagon < 10)
    //        yield break;

    //    yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM); //Use see number of playerHexagon

    //    float offsetDelayTime = 0;
    //    while (listPlayerHexagonSimilarColor.Count > 0)
    //    {
    //        Hexagon playerHexagon = listPlayerHexagonSimilarColor[0];
    //        playerHexagon.SetParent(null);
    //        playerHexagon.TweenVanish(offsetDelayTime);
    //        offsetDelayTime += GameConstants.HexagonConstants.TIME_DELAY;
    //        stackHexagon.RemovePlayerHexagon(playerHexagon);
    //        listPlayerHexagonSimilarColor.RemoveAt(0);
    //    }
    //    yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (numberOfPlayerHexagon - 1) * GameConstants.HexagonConstants.TIME_DELAY);
    //}

    //private IEnumerator IE_RemoveRandomStack()
    //{
    //    GridHexagon grid = GridManager.Instance.GetRandomGridHexagonContainStack();

    //    if(grid == null)
    //    {
    //        Debug.Log("Not Have Any Stack On Grid");
    //        yield break;
    //    }

    //    yield return IE_RemoveStackOnGrid(grid);
    //}

    //private IEnumerator IE_RemoveNeighborStack(GridHexagon grid)
    //{
    //    //List<GridHexagon> grids = GetNeighborGidHexagon(grid);
    //    List<GridHexagon> grids = GridManager.Instance.GetNeighborGidHexagon(grid);
    //    for (int i = 0; i < grids.Count; i++)
    //    {
    //        //yield return IE_RemoveStack(grids[i]);
    //        yield return grids[i].IE_RemoveStack();
    //    }
    //}

    //private IEnumerator IE_RemoveStacksSameTopColor(Color color)
    //{
    //    List<GridHexagon> grids = GridManager.Instance.GetGridHexagonByTopColor(color);

    //    for (int i = 0; i < grids.Count; i++)
    //    {
    //        yield return grids[i].StackOfCell.IE_RemoveByTopColor();
    //    }
    //}

    //private IEnumerator IE_RemoveStackOnGrid(GridHexagon grid)
    //{
    //    StackHexagon stack = grid.StackOfCell;
    //    List<Hexagon> hexagons = stack.Hexagons;
    //    int numberOfPlayerHexagon = hexagons.Count;
    //    float offsetDelayTime = 0;
    //    while (hexagons.Count > 0)
    //    {
    //        Hexagon playerHexagon = hexagons[0];
    //        playerHexagon.SetParent(null);
    //        playerHexagon.TweenVanish(offsetDelayTime);
    //        offsetDelayTime += GameConstants.HexagonConstants.TIME_DELAY;
    //        stack.RemovePlayerHexagon(playerHexagon);
    //        //hexagons.RemoveAt(0);
    //    }

    //    yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (numberOfPlayerHexagon - 1) * GameConstants.HexagonConstants.TIME_DELAY);
    //}

    //private List<Hexagon> GetPlayerHexagonSimilarColor(StackHexagon stackHexagon)
    //{
    //    Color color = stackHexagon.GetTopHexagonColor();
    //    List<Hexagon> playerHexagons = new List<Hexagon>();
    //    for (int i = stackHexagon.Hexagons.Count - 1; i >= 0; i--)
    //    {
    //        if (ColorUtils.ColorEquals(stackHexagon.Hexagons[i].Color, color))
    //        {
    //            playerHexagons.Add(stackHexagon.Hexagons[i]);
    //        }
    //        else
    //        {
    //            break;
    //        }
    //    }

    //    return playerHexagons;
    //}

    //private IEnumerator IE_MergePlayerHexagonsToStack(StackHexagon stackHexagon, List<GridHexagon> neighborGridHexagon)
    //{
    //    List<Hexagon> listPlayerHexagonMerge = GetPlayerHexagonNeedMerge(stackHexagon.GetTopHexagonColor(), neighborGridHexagon);
    //    RemovePlayerHexagonFromOldStack(neighborGridHexagon, listPlayerHexagonMerge);
    //    MergePlayerHexagon(stackHexagon, listPlayerHexagonMerge);
    //    stackHexagon.HideCanvas();
    //    yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (listPlayerHexagonMerge.Count - 1) * GameConstants.HexagonConstants.TIME_DELAY);
    //    stackHexagon.ShowCanvas();
    //}

    //private void MergePlayerHexagon(StackHexagon stackHexagon, List<Hexagon> listPlayerHexagonMerge)
    //{
    //    float yOfCurrentGridHexagon = (stackHexagon.Hexagons.Count - 1) * GameConstants.HexagonConstants.HEIGHT;
    //    for (int i = 0; i < listPlayerHexagonMerge.Count; i++)
    //    {
    //        Hexagon playerHexagon = listPlayerHexagonMerge[i];
    //        stackHexagon.AddPlayerHexagon(playerHexagon);

    //        float yOffset = yOfCurrentGridHexagon + (i + 1) * GameConstants.HexagonConstants.HEIGHT;
    //        Vector3 localPos = Vector3.up * yOffset;
    //        playerHexagon.Configure(stackHexagon);
    //        playerHexagon.MoveToGridHexagon(localPos, i * GameConstants.HexagonConstants.TIME_DELAY);
    //    }
    //}

    //private void RemovePlayerHexagonFromOldStack(List<GridHexagon> neighborGridHexagonSameTopColor, List<Hexagon> listPlayerHexagonMerge)
    //{
    //    //Remove Hexagon need merge from Hexagon Stack contain before
    //    foreach (GridHexagon gridHex in neighborGridHexagonSameTopColor)
    //    {
    //        StackHexagon hexagonStack = gridHex.StackOfCell;

    //        foreach (Hexagon playerHexagon in listPlayerHexagonMerge)
    //        {
    //            if (hexagonStack.CheckContainPlayerHexagon(playerHexagon))
    //            {
    //                hexagonStack.RemovePlayerHexagon(playerHexagon);
    //            }
    //        }
    //    }
    //}

    //private List<Hexagon> GetPlayerHexagonNeedMerge(Color topColorOfStackAtGridHexagon, List<GridHexagon> neighborGridHexagonSameTopColor)
    //{
    //    List<Hexagon> listPlayerHexagonMerge = new List<Hexagon>();
    //    foreach (GridHexagon gridHex in neighborGridHexagonSameTopColor)
    //    {
    //        StackHexagon hexagonStack = gridHex.StackOfCell;
    //        for (int i = hexagonStack.Hexagons.Count - 1; i >= 0; i--)
    //        {
    //            Hexagon playerHexagon = hexagonStack.Hexagons[i];

    //            if (ColorUtils.ColorEquals(playerHexagon.Color, topColorOfStackAtGridHexagon))
    //            {
    //                listPlayerHexagonMerge.Add(playerHexagon);
    //                playerHexagon.SetParent(null);
    //            }
    //        }
    //    }

    //    return listPlayerHexagonMerge;
    //}

    //GridManager
    //private List<GridHexagon> GetHexagonStackOfNeighborSameTopColor(List<GridHexagon> listNeighborGridHexagons, Color topColorOfStackAtGridHexagon)
    //{
    //    //Get List Neighbor have stack have same top color
    //    List<GridHexagon> neighborGridHexagonSameTopColor = new List<GridHexagon>();
    //    foreach (GridHexagon neighborGridHexagon in listNeighborGridHexagons)
    //    {
    //        Color color1 = neighborGridHexagon.StackOfCell.GetTopHexagonColor();
    //        Color color2 = topColorOfStackAtGridHexagon;
    //        if (ColorUtils.ColorEquals(color1, color2))
    //        {
    //            neighborGridHexagonSameTopColor.Add(neighborGridHexagon);
    //        }
    //    }

    //    return neighborGridHexagonSameTopColor;
    //}

    //GridManager
    //private List<GridHexagon> GetNeighborGidHexagon(GridHexagon gridHexagon)
    //{
    //    float distance = _IStackSphereRadius.GetRadiusByGrid().x * 2;
    //    Collider[] neighborGridCellColliders = Physics.OverlapSphere(gridHexagon.transform.position, distance, gridHexagonLayerMask);

    //    //Get A list of neighbor grid hexagon, that are occupied
    //    List<GridHexagon> listNeighborGridHexagons = new List<GridHexagon>();
    //    foreach (Collider collider in neighborGridCellColliders)
    //    {
    //        GridHexagon neighborGridHexagon = collider.GetComponent<GridHexagon>();

    //        if (neighborGridHexagon.CheckOccupied() == false)
    //        {
    //            continue;
    //        }
    //        if (neighborGridHexagon == gridHexagon)
    //        {
    //            continue;
    //        }

    //        if (neighborGridHexagon.State == GridHexagonState.LOCK_BY_GOAL || neighborGridHexagon.State == GridHexagonState.LOCK_BY_ADS)
    //            continue;

    //        listNeighborGridHexagons.Add(neighborGridHexagon);
    //    }

    //    return listNeighborGridHexagons;
    //}

    //GridManager
    //private List<GridHexagon> GetNeighborGridHexagonHaveOneSimilarColor(List<GridHexagon> neighborGridHexagonSameTopColor)
    //{
    //    List<GridHexagon> gridHexagons = new List<GridHexagon>();
    //    foreach (GridHexagon gridHexagon in neighborGridHexagonSameTopColor)
    //    {
    //        if (gridHexagon.StackOfCell.GetNumberSimilarColor() == 1)
    //        {
    //            gridHexagons.Add(gridHexagon);
    //        }
    //    }

    //    return gridHexagons;
    //}

    //GridManager
    //private List<GridHexagon> GetNeighborGridHexagonHaveThanOneSimilarColor(List<GridHexagon> neighborGridHexagonSameTopColor)
    //{
    //    List<GridHexagon> gridHexagons = new List<GridHexagon>();
    //    foreach (GridHexagon gridHexagon in neighborGridHexagonSameTopColor)
    //    {
    //        if (gridHexagon.StackOfCell.GetNumberSimilarColor() > 1)
    //        {
    //            gridHexagons.Add(gridHexagon);
    //        }
    //    }

    //    return gridHexagons;
    //}

    //private void CreateTree(GridHexagon grid)
    //{
    //    GridHexagonNode rootNode = new GridHexagonNode(grid, null);
    //    Stack<GridHexagonNode> queueNode = new Stack<GridHexagonNode>();
    //    queueNode.Push(rootNode);

    //    int k = 100;
    //    while (queueNode.Count > 0)
    //    {
    //        k--;

    //        if (k <= 0)
    //        {
    //            Debug.LogError("Some thing wrong here");
    //            return;
    //        }

    //        GridHexagonNode nodeVisiting = queueNode.Pop();

    //        if (nodeVisited.Contains(nodeVisiting))
    //        {
    //            continue;
    //        }

    //        nodeVisited.Push(nodeVisiting);

    //        List<GridHexagonNode> chilNodes = CreateEdge(nodeVisiting);

    //        foreach (GridHexagonNode node in chilNodes)
    //        {

    //            if (CheckContainNode(queueNode.ToList(), node))
    //            {
    //                continue;
    //            }

    //            if (CheckContainNode(nodeVisited.ToList(), node))
    //            {
    //                continue;
    //            }

    //            queueNode.Push(node);
    //            nodeVisiting.AddChildNode(node);
    //        }
    //    }
    //}

    //private bool CheckContainNode(List<GridHexagonNode> listNode, GridHexagonNode node)
    //{
    //    for (int i = 0; i < listNode.Count; i++)
    //    {
    //        GridHexagon grid1 = listNode[i].GetGridHexagon();
    //        GridHexagon grid2 = node.GetGridHexagon();

    //        if (grid1.gameObject.CompareObject(grid2.gameObject))
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //private List<GridHexagonNode> CreateEdge(GridHexagonNode EdgeNode)
    //{
    //    List<GridHexagonNode> result = new List<GridHexagonNode>();
    //    GridHexagon grid = EdgeNode.GetGridHexagon();
    //    Color topColor = grid.StackOfCell.GetTopHexagonColor();
    //    //List<GridHexagon> neighbor = GetNeighborGidHexagon(grid);
    //    List<GridHexagon> neighbor = GridManager.Instance.GetNeighborGidHexagon(grid);
    //    if (neighbor.Count == 0)
    //    {
    //        return result;
    //    }

    //    //List<GridHexagon> neighborSameColor = GetHexagonStackOfNeighborSameTopColor(neighbor, topColor);
    //    List<GridHexagon> neighborSameColor = GridManager.Instance.GetHexagonStackOfNeighborSameTopColor(neighbor, topColor);

    //    //Ưu tiên xử lý OneSimiler
    //    //Đảm bảo ThanOne Similar vào Queue trước => được xem xét trước => vào visited trước => được handle sau 
    //    //List<GridHexagon> neighborThanOneSimilar = GetNeighborGridHexagonHaveThanOneSimilarColor(neighborSameColor);
    //    List<GridHexagon> neighborThanOneSimilar = GridManager.Instance.GetNeighborGridHexagonHaveThanOneSimilarColor(neighborSameColor);
    //    foreach (GridHexagon item in neighborThanOneSimilar)
    //    {
    //        GridHexagonNode node = new GridHexagonNode(item, EdgeNode);
    //        result.Add(node);
    //    }

    //    //List<GridHexagon> neighborOneSimilar = GetNeighborGridHexagonHaveOneSimilarColor(neighborSameColor);
    //    List<GridHexagon> neighborOneSimilar = GridManager.Instance.GetNeighborGridHexagonHaveOneSimilarColor(neighborSameColor);
    //    foreach (GridHexagon item in neighborOneSimilar)
    //    {
    //        GridHexagonNode node = new GridHexagonNode(item, EdgeNode);
    //        result.Add(node);
    //    }

    //    return result;
    //}

    //private IEnumerator IE_HandleTree_Algorithm_1()
    //{
    //    GridHexagonNode edgeNode = null;
    //    int k = 100;
    //    while (nodeVisited.Count > 0)
    //    {
    //        k--;

    //        if (k <= 0)
    //        {
    //            Debug.LogError("Some thing wrong here IE_HandleTree");
    //            yield break;
    //        }

    //        GridHexagonNode node = nodeVisited.Pop();

    //        if (node.IsRootNode)
    //        {
    //            edgeNode = node;
    //            continue;
    //        }
    //        else if (node.IsLeafNode)
    //        {
    //            continue;
    //        }
    //        else if (edgeNode == null)
    //        {
    //            edgeNode = node;
    //            yield return IE_MergeHexagonToEdgeNode(edgeNode);
    //        }
    //        else if (edgeNode != null)
    //        {
    //            GridHexagon gridOfEdge = edgeNode.GetGridHexagon();
    //            GridHexagon gridOfNode = node.GetGridHexagon();

    //            if (gridOfEdge.gameObject.CompareObject(gridOfNode.gameObject))
    //            {
    //                continue;
    //            }
    //            else
    //            {
    //                edgeNode = node;
    //                yield return IE_MergeHexagonToEdgeNode(edgeNode);
    //            }
    //        }
    //    }

    //    //Root Node
    //    yield return IE_MergeRootNode_Algorithm_1(edgeNode);
    //}

    //private IEnumerator IE_MergeHexagonToEdgeNode(GridHexagonNode edgeNode)
    //{
    //    GridHexagon grid = edgeNode.GetGridHexagon();
    //    Color color = grid.StackOfCell.GetTopHexagonColor();

    //    GridHexagonNode[] childNodes = edgeNode.GetChildNodes();
    //    List<GridHexagon> gridHexagons = new List<GridHexagon>();
    //    foreach (GridHexagonNode node in childNodes)
    //    {
    //        gridHexagons.Add(node.GetGridHexagon());
    //    }

    //    List<Hexagon> hexagons = GetPlayerHexagonNeedMerge(color, gridHexagons);
    //    RemovePlayerHexagonFromOldStack(gridHexagons, hexagons);
    //    MergePlayerHexagon(grid.StackOfCell, hexagons);
    //    grid.StackOfCell.HideCanvas();
    //    yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (hexagons.Count - 1) * GameConstants.HexagonConstants.TIME_DELAY);
    //    grid.StackOfCell.ShowCanvas();

    //    foreach (GridHexagon gridChild in gridHexagons)
    //    {
    //        if (gridChild.CheckOccupied() && !GridManager.Instance.CheckContainGrid(listGridHexagonNeedUpdate, gridChild))
    //        {
    //            listGridHexagonNeedUpdate.Add(gridChild);
    //        }
    //    }
    //}

    //GridManager
    //private bool CheckContainGrid(List<GridHexagon> gridHexagons, GridHexagon grid)
    //{
    //    for (int i = 0; i < gridHexagons.Count; i++)
    //    {
    //        GridHexagon grid1 = gridHexagons[i];

    //        if (grid1.gameObject.CompareObject(grid.gameObject))
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //private IEnumerator IE_MergeRootNode_Algorithm_1(GridHexagonNode rootNode)
    //{
    //    GridHexagon grid = rootNode.GetGridHexagon();
    //    StackHexagon stack = grid.StackOfCell;
    //    Color color = grid.StackOfCell.GetTopHexagonColor();

    //    GridHexagonNode[] childNodes = rootNode.GetChildNodes();

    //    if (childNodes.Length == 0)
    //    {
    //        Debug.Log("Root not have any child");
    //        yield break;
    //    }

    //    List<GridHexagon> gridHexagons = new List<GridHexagon>();
    //    foreach (GridHexagonNode node in childNodes)
    //    {
    //        gridHexagons.Add(node.GetGridHexagon());
    //    }
        
    //    yield return IE_MergePlayerHexagonsToStack(stack, gridHexagons);

    //    List<Hexagon> listPlayerHexagonSimilarColor = GetPlayerHexagonSimilarColor(stack);
    //    int numberOfPlayerHexagon = listPlayerHexagonSimilarColor.Count;
    //    if (numberOfPlayerHexagon >= 10)
    //    {
    //        yield return IE_RemovePlayerHexagonsFromStack(stack);

    //        if(GameManager.Instance.IsState(GameState.LEVEL_PLAYING))
    //        {
    //            if (10 < numberOfPlayerHexagon && numberOfPlayerHexagon <= 12)
    //            {
    //                yield return IE_RemoveRandomStack();
    //            }
    //            else if (12 < numberOfPlayerHexagon && numberOfPlayerHexagon <= 15)
    //            {
    //                yield return IE_RemoveNeighborStack(grid);
    //            }
    //            else if (15 < numberOfPlayerHexagon && numberOfPlayerHexagon <= 18)
    //            {
    //                yield return IE_RemoveStacksSameTopColor(color);
    //            }
    //        }
    //    }    


    //    foreach (GridHexagon gridHexagon in gridHexagons)
    //    {
    //        if(gridHexagon.CheckOccupied())
    //        {
    //            listGridHexagonNeedUpdate.Add(gridHexagon);
    //        }
    //        else if(listGridHexagonNeedUpdate.Contains(gridHexagon))
    //        {
    //            listGridHexagonNeedUpdate.Remove(gridHexagon);
    //        }
    //    }

    //    if (grid.CheckOccupied())
    //    {
    //        listGridHexagonNeedUpdate.Add(grid);
    //    }
    //    else if (listGridHexagonNeedUpdate.Contains(grid))
    //    {
    //        listGridHexagonNeedUpdate.Remove(grid);
    //    }
    //}
}

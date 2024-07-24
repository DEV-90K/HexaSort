using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public GridHexagon GetGridHexagon()
    {
        return _GridHexagon;
    }
}

public class TreeController
{
    public Stack<GridHexagonNode> CreateTree(GridHexagon grid)
    {
        Stack<GridHexagonNode> nodeVisited = new Stack<GridHexagonNode>();

        GridHexagonNode rootNode = new GridHexagonNode(grid, null);
        Stack<GridHexagonNode> queueNode = new Stack<GridHexagonNode>();
        queueNode.Push(rootNode);

        int k = 100;
        while (queueNode.Count > 0)
        {
            k--;

            if (k <= 0)
            {
                return null;
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

        return nodeVisited;
    }

    private List<GridHexagonNode> CreateEdge(GridHexagonNode EdgeNode)
    {
        List<GridHexagonNode> result = new List<GridHexagonNode>();
        GridHexagon grid = EdgeNode.GetGridHexagon();
        Color topColor = grid.StackOfCell.GetTopHexagonColor();
        //List<GridHexagon> neighbor = GetNeighborGidHexagon(grid);
        List<GridHexagon> neighbor = GridManager.Instance.GetNeighborGidHexagon(grid);
        if (neighbor.Count == 0)
        {
            return result;
        }

        //List<GridHexagon> neighborSameColor = GetHexagonStackOfNeighborSameTopColor(neighbor, topColor);
        List<GridHexagon> neighborSameColor = GridManager.Instance.GetHexagonStackOfNeighborSameTopColor(neighbor, topColor);

        //Ưu tiên xử lý OneSimiler
        //Đảm bảo ThanOne Similar vào Queue trước => được xem xét trước => vào visited trước => được handle sau 
        //List<GridHexagon> neighborThanOneSimilar = GetNeighborGridHexagonHaveThanOneSimilarColor(neighborSameColor);
        List<GridHexagon> neighborThanOneSimilar = GridManager.Instance.GetNeighborGridHexagonHaveThanOneSimilarColor(neighborSameColor);
        foreach (GridHexagon item in neighborThanOneSimilar)
        {
            GridHexagonNode node = new GridHexagonNode(item, EdgeNode);
            result.Add(node);
        }

        //List<GridHexagon> neighborOneSimilar = GetNeighborGridHexagonHaveOneSimilarColor(neighborSameColor);
        List<GridHexagon> neighborOneSimilar = GridManager.Instance.GetNeighborGridHexagonHaveOneSimilarColor(neighborSameColor);
        foreach (GridHexagon item in neighborOneSimilar)
        {
            GridHexagonNode node = new GridHexagonNode(item, EdgeNode);
            result.Add(node);
        }

        return result;
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
}
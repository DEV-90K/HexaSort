using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static Action OnTurnCompleted;

    private TreeController treeController = new TreeController();
    private List<GridHexagon> listGridHexagonNeedUpdate = new List<GridHexagon>();
    private int idxRootVisited = 0;
    private bool _hasProcessing = false;
    private int[] SpaceSpecialEffects;

    private TimerUtils.CountdownTimer _trickCountdown = new TimerUtils.CountdownTimer(10);
    private bool _hasShowedGameTrick = false;
    private StackHexagon stackShowed = null;
    private GridHexagon gridShowed = null;

    private bool _hasShowedBoostTrick = false;

    private void OnEnable()
    {
        GameManager.OnChangeState += GameManager_OnChangeState;
        StackController.OnStackPlaced += StackController_OnStackPlaced;
        _trickCountdown.OnTimerStop += OnShowTrick;
    }

    private void OnDisable()
    {
        GameManager.OnChangeState -= GameManager_OnChangeState;
        StackController.OnStackPlaced -= StackController_OnStackPlaced;
        _trickCountdown.OnTimerStop -= OnShowTrick;
    }

    private void GameManager_OnChangeState(GameState state)
    {
        if(state == GameState.LEVEL_PLAYING)
        {
            _trickCountdown.Start();
        }
        else
        {
            _trickCountdown.Pause();
            _trickCountdown.Reset();
        }
    }

    private void OnShowTrick()
    {
        _hasShowedGameTrick = ShowGameTrick();

        if(!_hasShowedGameTrick)
        {
            ScreenLevel screenLevel = GUIManager.Instance.GetScreen<ScreenLevel>();
            screenLevel.ShowBoostTrick();
            _hasShowedBoostTrick = true;
        }        
    }

    private void OnHideTrick()
    {
        Debug.Log("OnHideTrick");
        if(_hasShowedGameTrick)
        {
            _hasShowedGameTrick = false;
            stackShowed.TweenHideTrick();
            gridShowed.TweenHideTrick();
        }
        
        if(_hasShowedBoostTrick)
        {
            _hasShowedBoostTrick = false;
            GUIManager.Instance.GetScreen<ScreenLevel>().HideBoostTrick();
        }
    }

    private bool ShowGameTrick()
    {
        GridHexagon[] gridsNotOccupied = GridManager.Instance.GetGridHexagonNotContainStack();
        int lengGrids = gridsNotOccupied.Length;
        if(lengGrids == 0)
        {
            return false;
        }

        StackHexagon[] stacksCanPlay = StackManager.Instance.GetStackHexagonShowing();
        int lengStacks = stacksCanPlay.Length;
        if (lengStacks == 0)
        {
            return false;
        }

        for (int i = 0; i < lengGrids; i++)
        {
            for(int j = 0; j < lengStacks; j++)
            {
                bool canPlace = GridManager.Instance.CheckCanPlaceStack(gridsNotOccupied[i], stacksCanPlay[j]);

                if(canPlace)
                {
                    stackShowed = stacksCanPlay[j];
                    stackShowed.TweenShowTrick();
                    gridShowed = gridsNotOccupied[i];
                    gridShowed.TweenShowTrick();
                    return true;
                }
            }
        }

        return false;
    }

    public void Update()
    {
        bool hasContact = false;
        if(Input.GetMouseButtonDown(0))
        {
            hasContact = true;
        }
        else if(Input.GetMouseButton(0))
        {
            hasContact = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            hasContact = false;
        }

        if(hasContact)
        {
            if(_hasShowedBoostTrick || _hasShowedGameTrick)
            {
                OnHideTrick();
                _trickCountdown.Start();
            }
            else
            {
                _trickCountdown.Reset();
                _trickCountdown.Pause();
            }    
        }
        else if(!hasContact && !_trickCountdown.IsFinished)
        {
            //If it's paused then recountdown
            if (!_trickCountdown.IsRunning)
            {
                _trickCountdown.Reset();
                _trickCountdown.Resume();
            }

            _trickCountdown.Tick(Time.deltaTime);
        }
    }

    public void OnSetup(int[] config, float timer = 10f)
    {
        SpaceSpecialEffects = config;
        _trickCountdown.Reset(timer);
    }

    private void StackController_OnStackPlaced(GridHexagon grid)
    {
        Debug.Log("Level Controller StackController_OnStackPlaced");
        OnStackPlacedOnGridHexagon(grid);
    }
    public void OnStackPlacedOnGridHexagon(GridHexagon gridHexagon)
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
            if(GameManager.Instance.IsState(GameState.FINISH))
            {                
                yield break;
            }

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

        yield return GridManager.Instance.IE_MergePlayerHexagonsToStack(stack, gridHexagons);

        List<Hexagon> listPlayerHexagonSimilarColor = StackManager.Instance.GetPlayerHexagonSimilarColor(stack);
        int numberOfPlayerHexagon = listPlayerHexagonSimilarColor.Count;
        if (numberOfPlayerHexagon >= 10)
        {
            yield return StackManager.Instance.IE_RemovePlayerHexagonsFromStack(stack);

            if (GameManager.Instance.IsState(GameState.LEVEL_PLAYING))
            {
                if (SpaceSpecialEffects[0] <= numberOfPlayerHexagon && numberOfPlayerHexagon < SpaceSpecialEffects[1])
                {
                    yield return IE_RemoveRandomStack();
                }
                else if (SpaceSpecialEffects[1] <= numberOfPlayerHexagon && numberOfPlayerHexagon < SpaceSpecialEffects[2])
                {
                    yield return IE_RemoveNeighborStack(grid);
                }
                else if (SpaceSpecialEffects[2] <= numberOfPlayerHexagon)
                {
                    yield return IE_RemoveStacksContainColor(color);
                }
            }
        }


        foreach (GridHexagon gridHexagon in gridHexagons)
        {
            if (gridHexagon.CheckOccupied())
            {
                listGridHexagonNeedUpdate.Add(gridHexagon);
            }
        }

        if (grid.CheckOccupied())
        {
            listGridHexagonNeedUpdate.Add(grid);
        }
    }

    private IEnumerator IE_RemoveRandomStack()
    {
        GridHexagon grid = GridManager.Instance.GetRandomGridHexagonContainStackAndNotLock();

        if (grid == null)
        {
            Debug.Log("Not Have Any Stack On Grid");
            yield break;
        }

        yield return IE_RemoveStackOnGrid(grid);
    }

    private IEnumerator IE_RemoveNeighborStack(GridHexagon grid)
    {
        List<GridHexagon> grids = GridManager.Instance.GetNeighborGidHexagon(grid);
        for (int i = 0; i < grids.Count; i++)
        {
            if (grid.State == GridHexagonState.LOCK_BY_GOAL || grid.State == GridHexagonState.LOCK_BY_ADS)
            {
                continue;
            }
            else
                yield return grids[i].IE_RemoveStack();
        }
    }

    private IEnumerator IE_RemoveStacksContainColor(Color color)
    {
        GridHexagon[] grids = GridManager.Instance.GetGridHexagonsContainColor(color);

        for(int i = 0; i < grids.Length; i++)
        {            
            if (grids[i].State == GridHexagonState.LOCK_BY_GOAL || grids[i].State == GridHexagonState.LOCK_BY_ADS)
            {
                continue;
            }
            else
                yield return grids[i].StackOfCell.IE_Remove();
        }
    }

    private IEnumerator IE_RemoveStackOnGrid(GridHexagon grid)
    {
        StackHexagon stack = grid.StackOfCell;
        List<Hexagon> hexagons = stack.Hexagons;
        int numberOfPlayerHexagon = hexagons.Count;
        float offsetDelayTime = 0;
        while (hexagons.Count > 0)
        {
            Hexagon playerHexagon = hexagons[0];
            playerHexagon.SetParent(null);
            playerHexagon.TweenVanish(offsetDelayTime);
            offsetDelayTime += GameConstants.HexagonConstants.TIME_DELAY;
            stack.RemovePlayerHexagon(playerHexagon);
            //hexagons.RemoveAt(0);
        }

        yield return new WaitForSeconds(GameConstants.HexagonConstants.TIME_ANIM + (numberOfPlayerHexagon - 1) * GameConstants.HexagonConstants.TIME_DELAY);
    }
}

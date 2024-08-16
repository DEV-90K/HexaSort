using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public interface IBoostSwap
{
    public void EnterBoostSwap();
    public void OnBoostSwap(GridHexagon[] grid);
    public void ExitBoostSwap();
}

public class PopupBoostSwap : PopupBase
{
    public static Action<bool> OnStackMoving;

    [SerializeField]
    private Button btnClose;
    private IBoostSwap _able;

    [SerializeField]
    private LayerMask playerHexagonLayerMask;
    [SerializeField]
    private LayerMask gridHexagonLayerMask;
    [SerializeField]
    private LayerMask groundLayerMask;
    private StackHexagon stackContact;    
    private GridHexagon gridContact;
    private GridHexagon gridSwap;
    private Vector3 originPosStackContact;

    private LevelPresenterData _presenterData;
    private RectTransform m_RectTransform;

    public override void OnInit(object[] paras)
    {
        base.OnInit(paras);
        _able = (IBoostSwap) paras[0];
        _presenterData = LevelManager.Instance.GetPresenterData();
    }

    public override void Show()
    {
        base.Show();
        OnStackMoving?.Invoke(false);
        _able.EnterBoostSwap();

        GameManager.Instance.ChangeState(GameState.PAUSE);
    }

    public override void Hide()
    {
        base.Hide();
        OnStackMoving?.Invoke(true);
        _able.ExitBoostSwap();
    }
    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        OnSetup();
        btnClose.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        GUIManager.Instance.ShowScreen<ScreenLevel>(_presenterData);
        Hide();
    }

    private void Update()
    {
        Controlling();
    }

    private void Controlling()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Input All Collider diffirent Hexagon Close
            ControlMouseDown();
        }
        else if (Input.GetMouseButton(0) && stackContact != null)
        {
            //Input All Collider Hexagon Close
            ControlMouseDrag();
        }
        else if (Input.GetMouseButtonUp(0) && stackContact != null)
        {
            ControlMouseUp();
        }
    }

    private void ControlMouseDown()
    {
        Debug.Log("ControlMouseDown");
        RaycastHit hit;
        Physics.Raycast(CameraUtils.GetRayFromMouseClicked(), out hit, 500, playerHexagonLayerMask);

        if (hit.collider == null)
        {
            Debug.Log("Not detected any hexagon");
            return;
        }

        StackHexagon stack = hit.collider.GetComponent<Hexagon>().HexagonStack;

        if(stack.transform.parent.TryGetComponent<GridHexagon>(out GridHexagon grid))
        {
            stackContact = stack;
            gridContact = grid;

            originPosStackContact = stackContact.transform.position;
        }
        else
        {
            stackContact = null;
            gridContact = null;
        }        
    }

    private void ControlMouseDrag()
    {
        Debug.Log("ControlMouseDrag");
        RaycastHit hit;
        Physics.Raycast(CameraUtils.GetRayFromMouseClicked(), out hit, 500, gridHexagonLayerMask);

        if (gridSwap != null)
        {
            gridSwap.ShowColor();

            if(gridSwap.CheckOccupied())
            {
                gridSwap.StackOfCell.transform.position = gridSwap.transform.position.With(y: GameConstants.HexagonConstants.HEIGHT);
            }
        }

        if (hit.collider == null)
        {
            DraggingAboveGround();
        }
        else
        {
            DraggingAboveGridHexagon(hit);
        }
    }

    private void DraggingAboveGround()
    {
        Debug.Log("DraggingAboveGround");
        RaycastHit hit;
        Physics.Raycast(CameraUtils.GetRayFromMouseClicked(), out hit, 500, groundLayerMask);

        if (hit.collider == null)
        {
            return;
        }

        Vector3 stackTargetPos = hit.point.With(y: 1.5f);
        stackContact.transform.position = Vector3.MoveTowards(stackContact.transform.position, stackTargetPos, Time.deltaTime * 30);
        gridSwap = null;
    }

    private void DraggingAboveGridHexagon(RaycastHit hit)
    {
        Debug.Log("DraggingAboveGridHexagon");        

        RaycastHit hit2;
        Physics.Raycast(CameraUtils.GetRayFromMouseClicked(), out hit2, 500, gridHexagonLayerMask);

        if (hit2.collider == null)
        {
            return;
        }

        Vector3 stackTargetPos = hit.point.With(y: 1.5f);
        stackContact.transform.position = Vector3.MoveTowards(stackContact.transform.position, stackTargetPos, Time.deltaTime * 30);

        GridHexagon gridHexagon = hit.collider.GetComponent<GridHexagon>();

        if (gridHexagon.gameObject.CompareObject(gridContact.gameObject))
        {
            if(gridSwap)
            {
                if (gridSwap.CheckOccupied())
                {
                    gridSwap.StackOfCell.transform.position = gridSwap.transform.position.With(y: GameConstants.HexagonConstants.HEIGHT);
                }
            }

            gridSwap = null;
            return;
        }

        if (gridHexagon.CheckOccupied())
        {
            DraggingAboveGridHexagonOccupied(gridHexagon);
        }
        else
        {
            DraggingAboveGridHexagonNonOccupied(gridHexagon);
        }
    }

    private void DraggingAboveGridHexagonOccupied(GridHexagon gridHexagon)
    {
        gridHexagon.ShowColorContact();
        gridSwap = gridHexagon;

        gridSwap.StackOfCell.transform.position = gridContact.transform.position.With(y: GameConstants.HexagonConstants.HEIGHT);
    }

    private void DraggingAboveGridHexagonNonOccupied(GridHexagon gridHexagon)
    {
        gridHexagon.ShowColorContact();
        gridSwap = gridHexagon;
    }

    private void ControlMouseUp()
    {
        if (gridSwap)
        {
            gridSwap.ShowColor();

            if(gridSwap.CheckOccupied())
            {
                gridContact.SetStackOfCell(gridSwap.StackOfCell);
                gridContact.StackOfCell.transform.SetParent(gridContact.transform);
                gridContact.StackOfCell.transform.position = gridContact.transform.position.With(y: GameConstants.HexagonConstants.HEIGHT);

                gridSwap.SetStackOfCell(stackContact);
                gridSwap.StackOfCell.transform.SetParent(gridSwap.transform);
                gridSwap.StackOfCell.transform.position = gridSwap.transform.position.With(y: GameConstants.HexagonConstants.HEIGHT);
                //_able.OnBoostSwap(gridSwap);
                //_able.OnBoostSwap(gridContact);
                _able.OnBoostSwap(new GridHexagon[2] { gridContact, gridSwap });
            }
            else
            {
                gridContact.SetStackOfCell(null);

                gridSwap.SetStackOfCell(stackContact);
                gridSwap.StackOfCell.transform.SetParent(gridSwap.transform);
                gridSwap.StackOfCell.transform.position = gridSwap.transform.position.With(y: GameConstants.HexagonConstants.HEIGHT);
                //_able.OnBoostSwap(gridSwap);
                _able.OnBoostSwap(new GridHexagon[1] { gridSwap });
            }
            
            gridSwap = null;
            StartCoroutine(IE_OnBoostSwapCompleted());
        }
        else
        {
            stackContact.transform.position = originPosStackContact;
        }

        stackContact = null;
    }

    private IEnumerator IE_OnBoostSwapCompleted()
    {
        yield return new WaitForSeconds(0f);
        GUIManager.Instance.ShowScreen<ScreenLevel>(_presenterData);
        Hide();
    }

    public override void OnSetup()
    {
        // xu ly tai tho
        float ratio = (float)Screen.height / (float)Screen.width;
        if (ratio > 1920 / 1080f)
        {
            Vector2 leftBottom = m_RectTransform.offsetMin;
            Vector2 rightTop = m_RectTransform.offsetMax;
            rightTop.y = -100f;
            m_RectTransform.offsetMax = rightTop;
            leftBottom.y = 0f;
            m_RectTransform.offsetMin = leftBottom;
            //m_OffsetY = 100f;
        }
    }
}

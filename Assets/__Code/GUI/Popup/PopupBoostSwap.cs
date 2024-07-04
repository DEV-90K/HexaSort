using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

public interface IBoostSwap
{
    public void EnterBoostSwap();
    public void OnBoostSwap(GridHexagon grid);
    public void ExitBoostSwap();
}

public class PopupBoostSwap : PopupBase
{
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

    public override void OnInit(object[] paras)
    {
        base.OnInit(paras);
        _able = (IBoostSwap) paras[0];
    }

    public override void Show()
    {
        base.Show();
        _able.EnterBoostSwap();
    }

    public override void Hide()
    {
        base.Hide();
        _able.ExitBoostSwap();
    }
    private void Awake()
    {
        btnClose.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        GUIManager.Instance.ShowScreen<ScreenLevel>();
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
            gridSwap.ShowColor();

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
        GridHexagon gridHexagon = hit.collider.GetComponent<GridHexagon>();

        if (gridHexagon.CheckOccupied())
        {
            DraggingAboveGridHexagonOccupied();
        }
        else
        {
            DraggingAboveGridHexagonNonOccupied(gridHexagon);
        }
    }

    private void DraggingAboveGridHexagonOccupied()
    {
        Debug.Log("DraggingAboveGridHexagonOccupied");
        DraggingAboveGround();
    }

    private void DraggingAboveGridHexagonNonOccupied(GridHexagon gridHexagon)
    {
        Debug.Log("DraggingAboveGridHexagonNonOccupied");
        //Move solution 1
        //Vector3 stackTargetPos = gridHexagon.transform.position.With(y: 1.5f);
        //stackContact.transform.position = Vector3.MoveTowards(stackContact.transform.position, stackTargetPos, Time.deltaTime * 30);
        //Move solution 2
        RaycastHit hit;
        Physics.Raycast(CameraUtils.GetRayFromMouseClicked(), out hit, 500, gridHexagonLayerMask);

        if (hit.collider == null)
        {
            return;
        }

        Vector3 stackTargetPos = hit.point.With(y: 1.5f);
        stackContact.transform.position = Vector3.MoveTowards(stackContact.transform.position, stackTargetPos, Time.deltaTime * 30);

        gridHexagon.ShowColorContact();
        gridSwap = gridHexagon;

    }

    private void ControlMouseUp()
    {
        if (gridSwap != null)
            gridSwap.ShowColor();

        if (gridSwap)
        {
            stackContact.transform.position = gridSwap.transform.position.With(y: GameConstants.HexagonConstants.HEIGHT);
            stackContact.transform.SetParent(gridSwap.transform);

            gridSwap.SetStackOfCell(stackContact);
            gridContact.SetStackOfCell(null);

            _able.OnBoostSwap(gridSwap);
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
        GUIManager.Instance.ShowScreen<ScreenLevel>();
        Hide();
    }
}

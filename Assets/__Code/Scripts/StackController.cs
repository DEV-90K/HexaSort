using System;
using UnityEngine;
using UnityUtils;

public class StackController : MonoBehaviour
{
    public static Action<GridHexagon> OnStackPlacedOnGridHexagon;

    [SerializeField]
    private LayerMask playerHexagonLayerMask;
    [SerializeField]
    private LayerMask gridHexagonLayerMask;
    [SerializeField]
    private LayerMask groundLayerMask;

    private IStackOnPlaced _stackPlaceable;

    private Vector3 originPosStackContact;
    private StackHexagon stackContact;
    private GridHexagon gridHexagonContact;

    public void OnInit(IStackOnPlaced stackPlaceable)
    {
        _stackPlaceable = stackPlaceable;
    }

    private void Update()
    {
        Controlling();
    }

    private void Controlling()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Input All Collider diffirent Hexagon Close
            ControlMouseDown();
        }
        else if(Input.GetMouseButton(0) && stackContact != null)
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
        RaycastHit hit;
        Physics.Raycast(GetRayFromMouseClicked(), out hit, 500, playerHexagonLayerMask);
        
        if (hit.collider == null)
        {
            Debug.Log("Not detected any hexagon");
            return;
        }

        stackContact = hit.collider.GetComponent<Hexagon>().HexagonStack;
        originPosStackContact = stackContact.transform.position;
    }

    private void ControlMouseDrag()
    {
        RaycastHit hit;
        Physics.Raycast(GetRayFromMouseClicked(), out hit, 500, gridHexagonLayerMask);

        if (gridHexagonContact != null)
            gridHexagonContact.ShowColor();

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
        RaycastHit hit;
        Physics.Raycast(GetRayFromMouseClicked(), out hit, 500, groundLayerMask);

        if (hit.collider == null)
        {
            return;
        }

        Vector3 stackTargetPos = hit.point.With(y: 1.5f);
        //Smooth move stack to stackTargetPos
        stackContact.transform.position = Vector3.MoveTowards(stackContact.transform.position, stackTargetPos, Time.deltaTime * 30);
        gridHexagonContact = null;
    }

    private void DraggingAboveGridHexagon(RaycastHit hit)
    {
        GridHexagon gridHexagon = hit.collider.GetComponent<GridHexagon>();

        if(gridHexagon.CheckOccupied())
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
        DraggingAboveGround();
    }

    private void DraggingAboveGridHexagonNonOccupied(GridHexagon gridHexagon)
    {
        //Move solution 1
        //Vector3 stackTargetPos = gridHexagon.transform.position.With(y: 1.5f);
        //stackContact.transform.position = Vector3.MoveTowards(stackContact.transform.position, stackTargetPos, Time.deltaTime * 30);
        //Move solution 2
        RaycastHit hit;
        Physics.Raycast(GetRayFromMouseClicked(), out hit, 500, gridHexagonLayerMask);

        if (hit.collider == null)
        {
            return;
        }

        Vector3 stackTargetPos = hit.point.With(y: 1.5f);        
        stackContact.transform.position = Vector3.MoveTowards(stackContact.transform.position, stackTargetPos, Time.deltaTime * 30);

        gridHexagon.ShowColorContact();
        gridHexagonContact = gridHexagon;

    }

    private void ControlMouseUp()
    {
        if (gridHexagonContact != null)
            gridHexagonContact.ShowColor();

        if (gridHexagonContact)
        {
            stackContact.transform.position = gridHexagonContact.transform.position.With(y: 0.2f);
            stackContact.transform.SetParent(gridHexagonContact.transform);
            stackContact.PlaceOnGridHexagon();
            gridHexagonContact.SetStackOfCell(stackContact);

            //OnStackPlaced?.Invoke();
            _stackPlaceable.OnStackPlaced();
            OnStackPlacedOnGridHexagon?.Invoke(gridHexagonContact);

            gridHexagonContact = null;
        }
        else
        {
            stackContact.transform.position = originPosStackContact;
        }

        stackContact = null;
    }

    private Ray GetRayFromMouseClicked()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}

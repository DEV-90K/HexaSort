using System;
using UnityEngine;
using UnityUtils;

public class StackController : MonoBehaviour
{
    public static Action OnStackPlaced;
    public static Action<GridHexagon> OnStackPlacedOnGridHexagon;

    [SerializeField]
    private LayerMask playerHexagonLayerMask;
    [SerializeField]
    private LayerMask gridHexagonLayerMask;
    [SerializeField]
    private LayerMask groundLayerMask;

    private HexagonStack stackContact;
    private Vector3 originPosStackContact;

    private GridHexagon gridHexagonContact;

    private void Update()
    {
        Controlling();
    }

    private void Controlling()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Input All Collider diffirent PlayerHexagon Close
            ControlMouseDown();
        }
        else if(Input.GetMouseButton(0) && stackContact != null)
        {
            //Input All Collider PlayerHexagon Close
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

        stackContact = hit.collider.GetComponent<PlayerHexagon>().HexagonStack;
        originPosStackContact = stackContact.transform.position;
    }

    private void ControlMouseDrag()
    {
        RaycastHit hit;
        Physics.Raycast(GetRayFromMouseClicked(), out hit, 500, gridHexagonLayerMask);

        if(hit.collider == null)
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

        Vector3 stackTargetPos = hit.point.With(y: 2);
        //Smooth move stack to stackTargetPos
        stackContact.transform.position = Vector3.MoveTowards(stackContact.transform.position, stackTargetPos, Time.deltaTime * 30);
        gridHexagonContact = null;
    }

    private void DraggingAboveGridHexagon(RaycastHit hit)
    {
        GridHexagon gridHexagon = hit.collider.GetComponent<GridHexagon>();
        if(gridHexagon.IsOccupied)
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
        Vector3 stackTargetPos = gridHexagon.transform.position.With(y: 2);
        stackContact.transform.position = Vector3.MoveTowards(stackContact.transform.position, stackTargetPos, Time.deltaTime * 30);

        gridHexagonContact = gridHexagon;
    }

    private void ControlMouseUp()
    {
        if(gridHexagonContact)
        {
            stackContact.transform.position = gridHexagonContact.transform.position.With(y: 0.2f);
            stackContact.transform.SetParent(gridHexagonContact.transform);
            stackContact.PlaceOnGridHexagon();
            gridHexagonContact.SetStackOfCell(stackContact);            

            OnStackPlaced?.Invoke();
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

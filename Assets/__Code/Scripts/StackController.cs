using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtils;

public class StackController : MonoBehaviour
{
    public static Action<GridHexagon> OnStackPlacedOnGridHexagon;
    public static Action<bool> OnStackMoving;


    [SerializeField]
    private LayerMask playerHexagonLayerMask;
    [SerializeField]
    private LayerMask gridHexagonLayerMask;
    [SerializeField]
    private LayerMask groundLayerMask;

    private IStackOnPlaced _stackPlaceable;
    private IStackSphereRadius _IStackSphereRadius;

    private Vector3 originPosStackContact;
    private StackHexagon stackContact;
    private GridHexagon gridHexagonContact;

    public void OnInit(IStackOnPlaced stackPlaceable, IStackSphereRadius stackSphereRadius)
    {
        _stackPlaceable = stackPlaceable;
        _IStackSphereRadius = stackSphereRadius;
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
            OnStackMoving?.Invoke(true);
            ControlMouseDown();
        }
        else if(Input.GetMouseButton(0) && stackContact != null)
        {
            //Input All Collider Hexagon Close
            OnStackMoving?.Invoke(false);
            ControlMouseDrag();
        }
        else if (Input.GetMouseButtonUp(0) && stackContact != null) 
        {
            OnStackMoving?.Invoke(false);
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
        RaycastHit hitGround;
        Physics.Raycast(GetRayFromMouseClicked(), out hitGround, 500, groundLayerMask);

        if (hitGround.collider == null)
        {
            return;
        }

        Vector3 stackTargetPos = hitGround.point.With(y: GameConstants.StackHexagonConstants.CONTACT_HEIGHT);
        stackContact.transform.position = Vector3.MoveTowards(stackContact.transform.position, stackTargetPos, Time.deltaTime * 30);
        gridHexagonContact = null;

        float radius = _IStackSphereRadius.GetRadiusByGrid().x * 1.25f;
        Collider[] neighborGridCellColliders = Physics.OverlapSphere(hitGround.point, radius, gridHexagonLayerMask);
        if(neighborGridCellColliders.Length > 0)
        {
            DraggingAboveGridHexagon(neighborGridCellColliders[0].GetComponent<GridHexagon>());
        }                    
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

    private void DraggingAboveGridHexagon(GridHexagon gridHexagon)
    {
        if (gridHexagon.CheckOccupied())
        {
            //DraggingAboveGridHexagonOccupied();
        }
        else
        {
            gridHexagon.ShowColorContact();
            gridHexagonContact = gridHexagon;
        }
    }

    private void DraggingAboveGridHexagonOccupied()
    {
        DraggingAboveGround();
    }

    private void DraggingAboveGridHexagonNonOccupied(GridHexagon gridHexagon)
    {
        RaycastHit hit;
        Physics.Raycast(GetRayFromMouseClicked(), out hit, 500, gridHexagonLayerMask);

        if (hit.collider == null)
        {
            return;
        }

        Vector3 stackTargetPos = hit.point.With(y: GameConstants.StackHexagonConstants.CONTACT_HEIGHT);        
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
            stackContact.transform.position = gridHexagonContact.transform.position.With(y: GameConstants.HexagonConstants.HEIGHT);
            stackContact.transform.SetParent(gridHexagonContact.transform);
            stackContact.PlaceOnGridHexagon();
            gridHexagonContact.SetStackOfCell(stackContact);

            _stackPlaceable.OnStackPlaced(stackContact);
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

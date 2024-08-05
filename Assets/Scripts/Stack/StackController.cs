using System;
using Unity.VisualScripting;
using UnityEngine;

public class StackController : MonoBehaviour
{
    public static Action<GridHexagon> OnStackPlaced;
    public static Action<bool> OnStackMoving;

    [SerializeField]
    private LayerMask playerHexagonLayerMask;
    [SerializeField]
    private LayerMask gridHexagonLayerMask;
    [SerializeField]
    private LayerMask groundLayerMask;

    private Transform tf_Ray;

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
            OnStackMoving?.Invoke(true);
            ControlMouseDown();
        }
        else if(Input.GetMouseButton(0) && stackContact != null)
        {
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
        Debug.Log("Here");
        tf_Ray = stackContact.GetTransformRay();
        originPosStackContact = stackContact.transform.position;
    }

    private void ControlMouseDrag()
    {
        StackDragging();
        GridDetection();

        //Ray ray = GetRayFromMouseClicked();
        //RaycastHit hit;
        //Physics.Raycast(ray, out hit, 500, gridHexagonLayerMask);

        //if (gridHexagonContact != null)
        //{
        //    gridHexagonContact.ShowColor();
        //}

        //if (hit.collider == null)
        //{
        //    DraggingAboveGround();
        //}
        //else
        //{
        //    DraggingAboveGridHexagon(hit);
        //}
    }

    private void StackDragging()
    {
        RaycastHit hitGround;
        Physics.Raycast(GetRayFromMouseClicked(), out hitGround, 500, groundLayerMask);

        if (hitGround.collider == null)
        {
            return;
        }

        Vector3 stackTargetPos = hitGround.point.With(y: GameConstants.StackHexagonConstants.CONTACT_HEIGHT);
        stackContact.transform.position = Vector3.MoveTowards(stackContact.transform.position, stackTargetPos, Time.deltaTime * 30);
    }

    private void GridDetection()
    {
        //RaycastHit hitGrid;
        //Physics.Raycast(GetRayFromMouseClicked(), out hitGrid, 500, gridHexagonLayerMask);

        //if(hitGrid.collider == null)
        //{
        //    if(gridHexagonContact != null)
        //    {
        //        gridHexagonContact.ShowColor();
        //        gridHexagonContact = null;
        //    }

        //    return;
        //}

        GridHexagon gridHex = OverlapSphere();
        if (gridHex == null)
        {
            if (gridHexagonContact != null)
            {
                gridHexagonContact.ShowColor();
                gridHexagonContact = null;
            }

            return;
        }


        //GridHexagon gridHex = hitGrid.collider.GetComponent<GridHexagon>();
        gridHexagonContact?.ShowColor();

        if(gridHex.CheckOccupied())
        {
            gridHexagonContact = null;
        }
        else
        {
            gridHexagonContact = gridHex;
            gridHexagonContact.ShowColorContact();
        }
    }

    private GridHexagon OverlapSphere()
    {
        RaycastHit hitGround;
        Physics.Raycast(GetRayFromRayPoint(), out hitGround, 500, groundLayerMask);

        if (hitGround.collider == null)
        {
            return null;
        }

        Vector3 point = hitGround.point;
        Collider[] colliders = Physics.OverlapSphere(point, 0.5f, gridHexagonLayerMask);

        float distanceMin = Mathf.Infinity;
        GridHexagon grid = null;
        foreach (Collider collider in colliders)
        {
            GridHexagon gridCol = collider.GetComponent<GridHexagon>();

            float distance = Vector3.Distance(gridCol.transform.position, tf_Ray.position);
            if (distance < distanceMin)
            {
                distanceMin = distance;
                grid = gridCol;
            }
        }

        return grid;
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
        if (neighborGridCellColliders.Length > 0)
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
            OnStackPlaced?.Invoke(gridHexagonContact);
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

    private Ray GetRayFromRayPoint()
    {
        return new Ray(tf_Ray.position, tf_Ray.up * -500f);
    }

    internal void OnResert()
    {
        //NOOB
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    private const float MIN_RADIUS = 4f;
    private const float MIN_SIZE = 10f;
    [SerializeField]
    private Camera _camera;

    private void OnEnable()
    {
        GridManager.OnInitCompleted += GridUnit_OnInitCompleted;
    }

    private void OnDisable()
    {
        GridManager.OnInitCompleted -= GridUnit_OnInitCompleted;
    }

    private void GridUnit_OnInitCompleted(float radius)
    {        
        if(radius < MIN_RADIUS)
        {
            _camera.orthographicSize = MIN_SIZE;
        }
        else
        {
            _camera.orthographicSize = MIN_SIZE + radius / MIN_RADIUS;
        }
    }
}

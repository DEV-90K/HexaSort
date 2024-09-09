using UnityEngine;

public class CameraScaler : MonoBehaviour
{
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
        //if (radius < MIN_RADIUS)
        //{
        //    _camera.orthographicSize = MIN_SIZE;
        //}
        //else
        //{
        //    _camera.orthographicSize = MIN_SIZE + radius / MIN_RADIUS;
        //}

        //OrthographicCamera();
    }

    public float GetDefaultSize()
    {
        float screenRatio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        Debug.Log("(float)Screen.currentResolution.width: " + (float)Screen.currentResolution.width);
        Debug.Log("(float)Screen.currentResolution.height: " + (float)Screen.currentResolution.height);
        if (screenRatio < 16/9)
        {
            //_camera.orthographicSize = Mathf.Max(9.4f, height * 2f * desiredRatio);
            return 9.6f;
        }
        else
        {
            return 9.4f;
        }
    }

    private void OrthographicCamera()
    {
        float height = GridManager.Instance.GetMaxWidth();
        float width = GridManager.Instance.GetMaxHeight();

        float screenRatio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        float desiredRatio = width / height;

        if (screenRatio > desiredRatio)
        {
            _camera.orthographicSize = Mathf.Max(9.4f, height * 2f * desiredRatio);
        }
        else
        {
            _camera.orthographicSize = Mathf.Max(9.4f, width * 2f);
        }
    }

    public void UpdateOrthographicSize(float size)
    {
        _camera.orthographicSize = size;
    }

    public float GetOrthographicSize()
    {
        return _camera.orthographicSize;
    }
}

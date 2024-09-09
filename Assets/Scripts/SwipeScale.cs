using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeScale : MonoBehaviour
{
    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private CameraScaler cameraScaler;

    private float speed = 5f;

    private bool isRotating = false;

    private float[] targetAngle = new float[] { 0f, 60f, 120f, 180f, 240f, 300f, 360f };
    private float[] targetScale = new float[] { 1f, 0.85f, 0.85f, 1f, 0.85f, 0.85f, 1f };
    private float[] sizes;

    private Transform[] _followeres;
    private Vector3 _targetScale = Vector3.one;
    private float targetSize;

    public void OnInit()
    {
        Invoke(nameof(OnSetup), Time.fixedDeltaTime);        
        UpdateFolloweres();
    }

    private void OnSetup()
    {
        sizes = new float[targetAngle.Length];
        float[] maxWidth = new float[targetAngle.Length];
        float defaultSize = cameraScaler.GetDefaultSize();

        for(int i = 0; i < targetAngle.Length; i++)
        {
            transform.localEulerAngles = Vector3.zero.With(y: targetAngle[i]);
            float width = GridManager.Instance.GetMaxWidth();
            float height = GridManager.Instance.GetMaxHeight();

            float screenRatio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
            float desiredRatio = width / height;

            if (screenRatio > desiredRatio)
            {
                sizes[i] = Mathf.Max(9.4f, height * 2f * desiredRatio);
            }
            else
            {
                sizes[i] = Mathf.Max(9.4f, width * 2f);
            }

        }
        transform.localEulerAngles = Vector3.zero;

        targetSize = sizes[0];
        cameraScaler.UpdateOrthographicSize(targetSize);
    }

    public void Register(Transform[] followeres)
    {
        _followeres = followeres;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject() && CheckZoneMouseDetech() && !PopupManager.Instance.CheckAnyPopupShowed())
        {
            isRotating = true;
        }
        else if (Input.GetMouseButtonUp(0) && isRotating)
        {
            isRotating = false;

            float eulerAngles = transform.rotation.eulerAngles.y;
            float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
            if (result < 0)
            {
                result += 360f;
            }

            float offset = 360f;

            for (int i = 0; i < targetAngle.Length; i++)
            {
                float angle = targetAngle[i];
                if (offset > Mathf.Abs(angle - result))
                {
                    offset = Mathf.Abs(angle - result);
                    //_targetScale = Vector3.one * targetScale[i];                    
                    targetSize = sizes[i];
                }
            }

            //StartCoroutine(IE_Scale(_targetScale));
            StartCoroutine(IE_Size(targetSize));
        }
    }

    private void UpdateFolloweres()
    {
        foreach (Transform tf in _followeres)
        {
            tf.localScale = transform.localScale;
        }
    }

    private IEnumerator IE_Scale(Vector3 targetScale)
    {
        Vector3 currScale = transform.localScale;

        float t = 0;
        while (t < 1)
        {
            t += speed * Time.deltaTime;
            transform.localScale = Vector3.Slerp(currScale, targetScale, t);
            UpdateFolloweres();
            yield return null;
        }
        transform.transform.localScale = targetScale;
        UpdateFolloweres();
    }

    private IEnumerator IE_Size(float size)
    {
        float currSize = cameraScaler.GetOrthographicSize();

        float t = 0;
        while (t < 1)
        {
            t += speed * Time.deltaTime;
            float newSize = Mathf.SmoothStep(currSize, size, t);
            cameraScaler.UpdateOrthographicSize(newSize);
            //UpdateFolloweres();
            yield return null;
        }
        //transform.transform.localScale = targetScale;
        cameraScaler.UpdateOrthographicSize(size);
    }

    private bool CheckZoneMouseDetech()
    {
        RaycastHit hit;
        Physics.Raycast(CameraUtils.GetRayFromMouseClicked(), out hit, 500, groundLayerMask);
        Vector3 mousePos = hit.point;
        if (mousePos.z > _followeres[0].position.z + 1.5f)
        {
            return true;
        }

        return false;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        //for (int i = 0; i < results.Count; i++)
        //{
        //    Debug.Log("Raycast: " + results[i].gameObject.name);
        //}

        return results.Count > 1;
    }
}

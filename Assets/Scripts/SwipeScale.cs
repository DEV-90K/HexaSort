using System.Collections;
using UnityEngine;

public class SwipeScale : MonoBehaviour
{
    [SerializeField]
    private LayerMask groundLayerMask;

    private float speed = 5f;

    private bool isRotating = false;

    private float[] targetAngle = new float[] { 0f, 60f, 120f, 180f, 240f, 300f, 360f };
    private float[] targetScale = new float[] { 1f, 0.85f, 0.85f, 1f, 0.85f, 0.85f, 1f };

    private Transform[] _followeres;
    private Vector3 _targetScale = Vector3.one;

    public void OnInit()
    {
        transform.localScale = Vector3.one;
        UpdateFolloweres();
    }

    public void Register(Transform[] followeres)
    {
        _followeres = followeres;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && CheckZoneMouseDetech() && !PopupManager.Instance.CheckAnyPopupShowed())
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
                    _targetScale = Vector3.one * targetScale[i];
                }
            }

            StartCoroutine(IE_Scale(_targetScale));
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

    private bool CheckZoneMouseDetech()
    {
        RaycastHit hit;
        Physics.Raycast(CameraUtils.GetRayFromMouseClicked(), out hit, 500, groundLayerMask);
        Vector3 mousePos = hit.point;
        if (-5f < mousePos.z && mousePos.z < 5f)
        {
            return true;
        }

        return false;
    }
}

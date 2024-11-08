using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeScale : MonoBehaviour
{
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
        if (IsPointerOverUIObject()) return;

        if (Input.GetMouseButtonDown(0))
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

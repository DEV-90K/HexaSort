using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityUtils;

public class SwipeRotate : MonoBehaviour
{
    #region With Mouse event
    private float speed = 5f;
    private float startPosX;

    private bool isRotating = false;

    private float[] targetAngle = new float[] { 0f, 60f, 120f, 180f, 240f, 300f, 360f };

    private Vector3 _targetRotate = Vector3.zero;
    private Vector3 _angleTracking = Vector3.zero;    

    public void OnInit()
    {
        transform.localEulerAngles = Vector3.zero;
    }

    private void Update()
    {
        if (IsPointerOverUIObject()) return;

        if(Input.GetMouseButtonDown(0))
        {            
            isRotating = true;            
            startPosX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0) && isRotating)
        {
            Rotate();
        }
        else if(Input.GetMouseButtonUp(0) && isRotating) 
        {
            isRotating = false;

            float eulerAngles = transform.rotation.eulerAngles.y;
            float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
            if (result < 0)
            {
                result += 360f;
            }

            this._angleTracking = Vector3.zero.With(y: result);
            this.transform.localEulerAngles = this._angleTracking;

            float minAngle = 360f;
            float offset = 360f;

            for(int i = 0; i < targetAngle.Length; i++)
            {
                float angle = targetAngle[i];
                if (offset > Mathf.Abs(angle - result))
                {
                    offset = Mathf.Abs(angle - result);
                    minAngle = angle;
                }
            }


            this._targetRotate = Vector3.zero.With(y: minAngle);
            StartCoroutine(IE_Rotate(this._targetRotate));
        }
    }

    private void Rotate()
    {
        float currPosX = Input.mousePosition.x;
        float mouseMove = currPosX - startPosX;
        transform.Rotate(Vector3.up, -mouseMove * speed * Time.deltaTime);
        startPosX = currPosX;
    }

    private IEnumerator IE_Rotate(Vector3 targetEulers)
    {
        Vector3 currEuler = transform.localEulerAngles;

        float t = 0;
        while(t < 1)
        {
            t += speed * Time.deltaTime;
            transform.localEulerAngles = Vector3.Slerp(currEuler, targetEulers, t);
            yield return null;
        }

        transform.localEulerAngles = targetEulers;
    }
    #endregion With Mouse event

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

using System.Collections;
using UnityEngine;
using UnityUtils;

public class SwipeRotate : MonoBehaviour
{
    #region With Mouse event
    private float speed = 5f;
    private float speedBack = 50f;
    private float startPosX;

    private bool isRotating = false;
    private bool _rotating = false;
    private bool hasCompleted = false;

    private float[] targetAngle = new float[] { 0f, 60f, 120f, 180f, 240f, 300f, 360f };
    private float[] targetScale = new float[] { 1f, 0.85f, 0.85f, 1f, 0.85f, 0.85f, 1f };

    private float _t = 0f;

    private Vector3 _targetRotate = Vector3.zero;
    //private Vector3 _speedBack = Vector3.zero;
    private Vector3 _angleTracking = Vector3.zero;    

    public void OnInit()
    {
        Debug.Log("On Init Rotate");
        transform.localEulerAngles = Vector3.zero;
    }

    private void Update()
    {
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
            _t = 0f;
            isRotating = false;
            hasCompleted = false;

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
                    //_targetScale = Vector3.one * targetScale[i];
                }
            }


            this._targetRotate = Vector3.zero.With(y: minAngle);
            StartCoroutine(IE_Rotate(this._targetRotate));
            //if (result < minAngle)
            //{
            //    this._speedBack = new Vector3(0, this.speedBack, 0);
            //}
            //else
            //{
            //    this._speedBack = new Vector3(0, -this.speedBack, 0);
            //}
        }

        //if (!this.isRotating && !hasCompleted)
        //{
        //    _t += speed * Time.deltaTime;
        //    Debug.Log("this.transform.localEulerAngles: " + this.transform.localEulerAngles);
        //    Debug.Log("_targetRotate: " + _targetRotate);

        //    Vector3 result = Vector3.Slerp(this.transform.localEulerAngles, _targetRotate, _t);
        //    this.transform.localEulerAngles = result.With(x: 0, z: 0);
        //    if (_t >= 1)
        //    {
        //        hasCompleted = true;
        //        this.transform.localEulerAngles = _targetRotate;
        //    }
        //}
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
}

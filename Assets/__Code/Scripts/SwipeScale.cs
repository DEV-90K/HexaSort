using UnityEngine;

public class SwipeScale : MonoBehaviour
{
    private float speed = 5f;

    private bool isRotating = false;
    private bool hasCompleted = false;

    private float[] targetAngle = new float[] { 0f, 60f, 120f, 180f, 240f, 300f, 360f };
    private float[] targetScale = new float[] { 1f, 0.85f, 0.85f, 1f, 0.85f, 0.85f, 1f };

    private float _t = 0f;
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
        if (Input.GetMouseButtonDown(0))
        {
            _t = 0f;
            isRotating = true;
        }
        else if (Input.GetMouseButtonUp(0) && isRotating)
        {
            isRotating = false;
            hasCompleted = false;

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
        }

        if (!this.isRotating && !hasCompleted)
        {
            _t += speed * Time.deltaTime;
            this.transform.localScale = Vector3.Slerp(this.transform.localScale, _targetScale, _t);

            if(_t >= 1)
            {
                hasCompleted = true;
                this.transform.localScale = _targetScale;
            }
            
            UpdateFolloweres();
        }
    }

    private void UpdateFolloweres()
    {
        foreach (Transform tf in _followeres)
        {
            tf.localScale = this.transform.localScale;
        }
    }
}

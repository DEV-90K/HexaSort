using UnityEngine;

public static class CameraUtils
{
    public static Ray GetRayFromMouseClicked()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    public static Vector3 GetWorldPosFromMouseClicked()
    {
        Vector3 inputPos = Input.mousePosition;
        inputPos.z = -Camera.main.transform.position.z;
        Vector3 pos = Camera.main.ScreenToWorldPoint(inputPos);
        return pos;
    }
}

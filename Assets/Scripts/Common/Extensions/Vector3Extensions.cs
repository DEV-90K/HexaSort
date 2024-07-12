using UnityEngine;
public static class Vector3Extensions
{
    public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
    }

    public static Vector3 Add(this Vector3 vector, float x = 0, float y = 0, float z = 0)
    {
        return new Vector3(vector.x + x, vector.y + y, vector.z + z);
    }

    public static bool InRangeOf(this Vector3 current, Vector3 target, float range)
    {
        return (current - target).sqrMagnitude <= range * range;
    }

    public static Vector3 ComponentDivide(this Vector3 v0, Vector3 v1)
    {
        return new Vector3(
            v1.x != 0 ? v0.x / v1.x : v0.x,
            v1.y != 0 ? v0.y / v1.y : v0.y,
            v1.z != 0 ? v0.z / v1.z : v0.z);
    }
}

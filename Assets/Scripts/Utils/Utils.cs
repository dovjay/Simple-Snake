using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position) {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }

    public static Vector2 DirectionThreshold(Vector2 v, float threshold) {
        if (Vector2.Dot(Vector2.up, v) > threshold) {
            return Vector2.up;
        }
        else if (Vector2.Dot(Vector2.down, v) > threshold) {
            return Vector2.down;
        }
        else if (Vector2.Dot(Vector2.left, v) > threshold) {
            return Vector2.left;
        }
        else if (Vector2.Dot(Vector2.right, v) > threshold) {
            return Vector2.right;
        }

        return Vector2.zero;
    }

    public static bool InsideBoundary(Vector2 position, Playground playground) {
        if (position.x <= playground.boundary.x &&
            position.x >= -playground.boundary.x &&
            position.y <= playground.boundary.y &&
            position.y >= -playground.boundary.y
        ) return true;

        return false;
    }
}

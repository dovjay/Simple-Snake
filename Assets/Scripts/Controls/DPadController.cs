using UnityEngine;
using System;

public class DPadController : MonoBehaviour {
    public static event Action<Vector2> OnMove;
    public static event Action OnBoost;

    public void BoostSpeed() => OnBoost?.Invoke();

    public void MoveUp() => OnMove?.Invoke(Vector2.up);

    public void MoveLeft() => OnMove?.Invoke(Vector2.left);

    public void MoveDown() => OnMove?.Invoke(Vector2.down);

    public void MoveRight() => OnMove?.Invoke(Vector2.right);
}
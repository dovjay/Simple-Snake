using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static event Action OnGameOver;
    public static event Action OnPause;
    public static event Action OnTimerThreshold;

    public bool isPaused = false;
    public bool isGameOver = false;

    [SerializeField]
    private float maxTimerThreshold = 15;
    private float timer = 0;

    private void Update() {
        timer += Time.deltaTime;
        if (timer < maxTimerThreshold) return;

        timer = 0;
        OnTimerThreshold?.Invoke();
    }

    public void TriggerPause() {
        isPaused = !isPaused;
        OnPause?.Invoke();
    }

    public void TriggerGameOver() {
        isGameOver = true;
        OnGameOver?.Invoke();
    }
}
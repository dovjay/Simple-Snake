using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    [Header("Score UI")]
    [SerializeField]
    private TMP_Text scoreText;
    private ScoreSystem scoreSystem;

    [Header("Game Over UI")]
    [SerializeField]
    private GameObject GameOverUI;
    [SerializeField]
    private TMP_Text gameOverText;

    [Header("Power Up UI")]
    [SerializeField]
    private TMP_Text powerUpText;
    [SerializeField]
    private Slider powerUpDurationBar;

    [Header("Pause UI")]
    [SerializeField]
    private GameObject pausePrompt;

    Coroutine powerUpCoroutine = null;

    private void Awake() {
        scoreSystem = FindObjectOfType<ScoreSystem>();
    }

    private void OnEnable() {
        GameManager.OnGameOver += GameOverPrompt;
        ObjectiveSpawner.OnPowerUpSpawn += ShowPowerUpExpire;
        Objective.OnPowerUpStart += ShowPowerUpLifetime;
        Objective.OnPowerUpEaten += StopPowerUpCoroutine;
    }

    private void OnDisable() {
        GameManager.OnGameOver -= GameOverPrompt;
        ObjectiveSpawner.OnPowerUpSpawn -= ShowPowerUpExpire;
        Objective.OnPowerUpStart -= ShowPowerUpLifetime;
        Objective.OnPowerUpEaten -= StopPowerUpCoroutine;
    }
    private IEnumerator StartPowerUpBar(string s, float t) {
        powerUpDurationBar.maxValue = t;
        powerUpDurationBar.value = t;
        powerUpDurationBar.gameObject.SetActive(true);

        while (t >= 0) {
            t -= Time.deltaTime;
            powerUpDurationBar.value = t;
            powerUpText.text = $"{s} {t:0.0}s";
            yield return null;
        }

        powerUpDurationBar.gameObject.SetActive(false);
    }

    private void StopPowerUpCoroutine() {
        powerUpDurationBar.value = 0;
        StopCoroutine(powerUpCoroutine);
        powerUpDurationBar.gameObject.SetActive(false);
    }

    private void ShowPowerUpExpire(GameObject obj) {
        powerUpText.text = "Bonus expire in 9.9s";
        Objective objective = obj.GetComponent<Objective>();
        powerUpCoroutine = StartCoroutine(StartPowerUpBar("Bonus expire in", objective.timeLimit));
    }

    private void ShowPowerUpLifetime(Objective obj) {
        powerUpText.text = $"{obj.objectName} 9.9s";
        powerUpCoroutine = StartCoroutine(StartPowerUpBar(obj.objectName, obj.powerUpEffects.lifeTimeDuration));
    }

    private void GameOverPrompt() {
        gameOverText.text = $"Game Over! \n Score {scoreSystem.GetScore()}";
        GameOverUI.SetActive(true);
        scoreText.gameObject.SetActive(false);
    }

    public void PauseGame() {
        FindObjectOfType<GameManager>().TriggerPause();
        pausePrompt.SetActive(!pausePrompt.activeSelf);
    }

    public void PlayAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
}
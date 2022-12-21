using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private ScoreTrackerSO scoreTracker;

    private float scoreMultiplier = 1f;
    private float defaultMultiplier = 1f;
    private float highScore = 0f;
    private bool powerUpMode = false;

    private GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        defaultMultiplier = PlayerPrefs.GetInt(OptionKey.SCORE_MULTIPLIER, 1);
        scoreMultiplier = defaultMultiplier;
        GetInitialHighscore();
    }

    private void OnEnable() {
        GameManager.OnGameOver += UpdateLeaderboard;
        GameManager.OnTimerThreshold += UpdateDefaultMultiplier;
    }

    private void OnDisable() {
        GameManager.OnGameOver -= UpdateLeaderboard;
        GameManager.OnTimerThreshold -= UpdateDefaultMultiplier;
    }

    void Start()
    {
        scoreTracker.score = 0f;
        scoreTracker.timer = 0f;
        UpdateScoreText();
    }

    void LateUpdate()
    {
        if (gameManager.isPaused) return;
        if (gameManager.isGameOver) return;

        UpdateTimer();
    }

    private void UpdateLeaderboard() {
        if (highScore >= scoreTracker.score) return;

        switch (scoreTracker.name) {
            case "ClassicScoreTracker":
                PlayerPrefs.SetFloat(ScoreType.CLASSIC_SCORE, scoreTracker.score);
                PlayerPrefs.SetFloat(ScoreType.CLASSIC_TIME, scoreTracker.timer);
                break;
            case "AllNewScoreTracker":
                PlayerPrefs.SetFloat(ScoreType.ALL_NEW_SCORE, scoreTracker.score);
                PlayerPrefs.SetFloat(ScoreType.ALL_NEW_TIME, scoreTracker.timer);
                break;
            case "TimeTrialScoreTracker":
                PlayerPrefs.SetFloat(ScoreType.TIME_TRIAL_SCORE, scoreTracker.score);
                PlayerPrefs.SetFloat(ScoreType.TIME_TRIAL_TIME, scoreTracker.timer);
                break;
            default:
                Debug.LogError("Please check your score tracker filename!");
                break;
        }
    }

    private void GetInitialHighscore() {
        switch (scoreTracker.name) {
            case "ClassicScoreTracker":
                highScore = PlayerPrefs.GetFloat(ScoreType.CLASSIC_SCORE, 0f);
                break;
            case "AllNewScoreTracker":
                highScore = PlayerPrefs.GetFloat(ScoreType.ALL_NEW_SCORE, 0f);
                break;
            case "TimeTrialScoreTracker":
                highScore = PlayerPrefs.GetFloat(ScoreType.TIME_TRIAL_SCORE, 0f);
                break;
            default:
                Debug.LogError("Please check your score tracker filename!");
                break;
        }
    }

    private void UpdateTimer() => scoreTracker.timer += Time.deltaTime;

    private void UpdateDefaultMultiplier() {
        defaultMultiplier++;

        if (!powerUpMode)
            scoreMultiplier = defaultMultiplier;
    }

    public void PowerUpScoreMultiplier(float multiplier=2f) {
        powerUpMode = !powerUpMode;

        if (powerUpMode)
            scoreMultiplier *= multiplier;
        else
            scoreMultiplier = defaultMultiplier;

    }

    public void AddScore(float additionalMultiplier=1f) {
        scoreTracker.score += scoreMultiplier * additionalMultiplier;
        UpdateScoreText();
    }

    private void UpdateScoreText() => scoreText.text = $"{Mathf.FloorToInt(scoreTracker.score):00000}";

    public float GetScore() => scoreTracker.score;
}

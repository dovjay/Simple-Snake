using UnityEngine;
using TMPro;
using System;

public class LeaderboardUI : MonoBehaviour {
    [Header("Classic Mode")]
    [SerializeField] TMP_Text classicScoreText;
    [SerializeField] TMP_Text classicTimeText;

    [Header("All-New Mode")]
    [SerializeField] TMP_Text allNewScoreText;
    [SerializeField] TMP_Text allNewTimeText;

    [Header("Time Trial Mode")]
    [SerializeField] TMP_Text timeTrialScoreText;
    [SerializeField] TMP_Text timeTrialTimeText;

    private void OnEnable() {
        UpdateHighScore(ScoreType.CLASSIC_SCORE, ScoreType.CLASSIC_TIME, classicScoreText, classicTimeText);
        UpdateHighScore(ScoreType.ALL_NEW_SCORE, ScoreType.ALL_NEW_TIME, allNewScoreText, allNewTimeText);
        UpdateHighScore(ScoreType.TIME_TRIAL_SCORE, ScoreType.TIME_TRIAL_TIME, timeTrialScoreText, timeTrialTimeText);
    }

    private void UpdateHighScore(string scoreType, string timeType, TMP_Text scoreText, TMP_Text timeText) {
        float score = PlayerPrefs.GetFloat(scoreType, 0f);
        float totalSeconds = PlayerPrefs.GetFloat(timeType, 0f);
        TimeSpan time = TimeSpan.FromSeconds(totalSeconds);

        scoreText.text = $"{Mathf.FloorToInt(score):00000}";
        timeText.text = time.ToString("hh':'mm':'ss");
    }
}
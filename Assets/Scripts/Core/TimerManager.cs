using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float timerDefault;
    [SerializeField] private float timeIncrease;
    [SerializeField] private float bonusTimeIncrease;

    private float remainingTime;

    private GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable() {
        Objective.OnFoodEaten += AddTime;
        Objective.OnPowerUpEaten += AddBonusTime;
    }

    private void OnDisable() {
        Objective.OnFoodEaten -= AddTime;
        Objective.OnPowerUpEaten -= AddBonusTime;
    }

    void Start()
    {
        remainingTime = timerDefault;
        UpdateTimer();
    }

    void Update()
    {
        if (gameManager.isPaused) return;
        if (gameManager.isGameOver) return;
        
        if (remainingTime > 0) {
            UpdateTimer();
            remainingTime -= Time.deltaTime;
        } else {
            gameManager.TriggerGameOver();
        }
    }

    private void UpdateTimer() => timerText.text = $"{Mathf.RoundToInt(remainingTime):00}s";

    private void AddTime() => remainingTime = Mathf.Min(remainingTime + timeIncrease, 99f);

    private void AddBonusTime() => remainingTime = Mathf.Min(remainingTime + bonusTimeIncrease, 99f);
}

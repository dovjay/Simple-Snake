using System;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {
    public static event Action OnPowerUpEnd;

    public float lifeTimeDuration = 10f;
    private PowerUpSO powerEffect;
    private GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void LateUpdate() {
        if (lifeTimeDuration <= 0) Destroy(gameObject);
        lifeTimeDuration -= Time.deltaTime;
    }

    public void Initialise(PowerUpSO powerUp) {
        powerEffect = powerUp;
        lifeTimeDuration = powerEffect.lifeTimeDuration;
        
        FindObjectOfType<ScoreSystem>().PowerUpScoreMultiplier(powerEffect.scoreMultiplier);
        FindObjectOfType<PlayerController>().InverseControlPowerUp(powerEffect.inverseControl);

        if (powerEffect.speedBoost)
            FindObjectOfType<PlayerController>().BoostPowerUp(powerEffect.speedBoost);
    }
    
    private void OnDestroy() {
        if (!gameManager.isGameOver) OnPowerUpEnd?.Invoke();

        FindObjectOfType<ScoreSystem>().PowerUpScoreMultiplier();
        FindObjectOfType<PlayerController>().InverseControlPowerUp(false);

        if (powerEffect.speedBoost)
            FindObjectOfType<PlayerController>().BoostPowerUp(false);
    }
}
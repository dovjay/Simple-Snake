using System;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public static event Action OnFoodEaten;
    public static event Action<Objective> OnPowerUpStart;
    public static event Action OnPowerUpEaten;

    public string objectName;
    public float timeLimit;
    public bool growth;
    public float additionalMultiplier = 1f;
    public PowerUpSO powerUpEffects;
    public bool shouldSpawn;

    [SerializeField]
    private GameObject powerUpManager;

    private void Start() {
        if (timeLimit > 0) Destroy(gameObject, timeLimit);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (shouldSpawn) {
            FindObjectOfType<ObjectiveSpawner>().Spawn(objectName);
            OnFoodEaten?.Invoke();
        }
        else {
            OnPowerUpEaten?.Invoke();
        }

        if (growth) other.transform.parent.GetChild(0).GetComponent<PlayerBody>().StartFoodProcess();

        if (powerUpEffects != null) {
            OnPowerUpStart?.Invoke(this);
            GameObject instance = Instantiate(powerUpManager);
            instance.GetComponent<PowerUpManager>().Initialise(powerUpEffects);
        }

        FindObjectOfType<ScoreSystem>().AddScore(additionalMultiplier);
        Destroy(gameObject);
    }
}

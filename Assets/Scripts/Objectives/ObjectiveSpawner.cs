using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSpawner : MonoBehaviour {
    public static event Action<GameObject> OnPowerUpSpawn;

    public List<GameObject> objectives;

    [SerializeField]
    private int defaultBonusCD = 3;

    private int bonusCD = 0;
    private Playground playground;

    private void Awake() {
        playground = FindObjectOfType<Playground>();
    }

    private void Start() {
        bonusCD = defaultBonusCD;
        Spawn("Food");
    }

    public void Spawn(string name) {
        Instantiate(objectives[FindObjective(name)], GeneratePosition(), Quaternion.identity);
        
        if (bonusCD == 0) {
            GameObject powerUp = Instantiate(objectives[GenerateRandomIndex()], GeneratePosition(), Quaternion.identity);
            OnPowerUpSpawn?.Invoke(powerUp);
            bonusCD = defaultBonusCD;
        }

        bonusCD--;
    }

    public Vector2 GeneratePosition() {
        Vector2 position = new Vector2(UnityEngine.Random.Range(-playground.xMargin, playground.xMargin) * playground.marginStep, 
                                       UnityEngine.Random.Range(-playground.yMargin, playground.yMargin) * playground.marginStep);

        while (Physics2D.OverlapCircle(position, playground.marginStep))
        {
            position = new Vector2(UnityEngine.Random.Range(-playground.xMargin, playground.xMargin) * playground.marginStep, 
                                   UnityEngine.Random.Range(-playground.yMargin, playground.yMargin) * playground.marginStep);
        } 

        return position;
    }

    private int GenerateRandomIndex() {
        return UnityEngine.Random.Range(1, objectives.Count);
    }

    private int FindObjective(string name) {
        int i = 0;

        foreach (GameObject objective in objectives)
        {
            if (objective.name == name) return i;
            i++;
        }

        return -1;
    }
}
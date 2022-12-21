using UnityEngine;

[CreateAssetMenu(fileName = "ScoreTracker", menuName = "Score/ScoreTrackerSO", order = 0)]
public class ScoreTrackerSO : ScriptableObject {
    public float score;
    public float timer;
}
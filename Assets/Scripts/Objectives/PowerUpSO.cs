using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp Name", menuName = "Power Up/PowerUpSO", order = 0)]
public class PowerUpSO : ScriptableObject
{
    public float lifeTimeDuration;
    public float scoreMultiplier;
    public bool speedBoost;
    public bool inverseControl;
}

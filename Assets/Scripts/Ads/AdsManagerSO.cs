using UnityEngine;

[CreateAssetMenu(fileName = "AdsManager", menuName = "Ads/AdsManagerSO", order = 0)]
public class AdsManagerSO : ScriptableObject {
    public string placementId;
    public int adsCounter;
    public int maxAdsCounter = 5;
}
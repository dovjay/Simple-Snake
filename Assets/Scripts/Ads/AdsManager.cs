using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : SingletonPersistent<AdsManager>, IUnityAdsListener
{
    [Header("Change Before Deploy")]
    [SerializeField] bool _testMode = true;

    [Header("Basic Info")]
#if UNITY_ANDROID
    [SerializeField] string _gameId = "4862421";
#elif UNITY_IOS
    [SerializeField] string _gameId = "4862420";
#endif
    [SerializeField] AdsManagerSO _interstitialAd;

    private void Start() {
        Advertisement.AddListener(this);
        Advertisement.Initialize(_gameId, _testMode);
    }

    private void OnEnable() {
        GameManager.OnGameOver += InterstitialCounter;
    }

    private void OnDisable() {
        GameManager.OnGameOver -= InterstitialCounter;
    }

    private void InterstitialCounter() {
        if (_interstitialAd.adsCounter == _interstitialAd.maxAdsCounter) {
            _interstitialAd.adsCounter = 0;
            ShowAd(_interstitialAd.placementId);
            return;
        }

        _interstitialAd.adsCounter++;
    }

    public void ShowAd(string placementId) {
        Advertisement.Show(placementId);
        print("Ads should be played");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError($"Unity Ads Error: {message}");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Unity Ads Started");
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("Unity Ads Ready");
    }
}

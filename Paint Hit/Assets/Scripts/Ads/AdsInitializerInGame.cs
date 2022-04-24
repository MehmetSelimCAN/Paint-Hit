using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializerInGame : MonoBehaviour, IUnityAdsInitializationListener {
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;

    [SerializeField] ContinueRewardButton rewardedAdsButton;

    void Awake() {
        InitializeAds();
    }

    private void Start() {
        if (Advertisement.isInitialized) {
            rewardedAdsButton.LoadAd();
        }
    }

    public void InitializeAds() {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;
        Advertisement.Initialize(_gameId, _testMode, this);
    }

    public void OnInitializationComplete() {
        rewardedAdsButton.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour , IUnityAdsLoadListener , IUnityAdsShowListener
{
    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdUnitId;

    private string adUnitId;

    private void Awake()
    {
        #if UNITY_IOS
                adUnitId = isoAdUnitId;
        #elif UNITY_ANDROID
                adUnitId = androidAdUnitId;
        #endif
    }

    public void LoadInterstatialAd()
    {
        Advertisement.Load(adUnitId, this);
    }

    public void ShowInterstitialAd()
    {
        Advertisement.Show(adUnitId, this);
        LoadInterstatialAd();
    }

    #region LoadCallBacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Interstitial Ad Loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {    }
    #endregion
    #region ShowCallBacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {    }

    public void OnUnityAdsShowStart(string placementId) {    }

    public void OnUnityAdsShowClick(string placementId) {    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        AdsManager.instance.bannerAds.ShowBannerAd();
        Debug.Log("Interstatial Ad Compleated");
    }
    #endregion
}

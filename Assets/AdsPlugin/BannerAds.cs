using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour
{
    [SerializeField] private string androidAdUnitGameId;
    [SerializeField] private string iosAdUnitGameId;

    private string adUnitId;

    private void Awake()
    {
        #if UNITY_IOS
                adUnitId = isoAdUnitId;
        #elif UNITY_ANDROID
                adUnitId = androidAdUnitGameId;
        #endif

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    }

    public void LoadBannerAd()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = BannerLoaded,
            errorCallback = BannerLoadedError
        };

        Advertisement.Banner.Load(adUnitId, options);
    }

    public void ShowBannerAd()
    {
        BannerOptions options = new BannerOptions
        {
            showCallback = BannerShown,
            clickCallback = BannerClicked,
            hideCallback = BannerHiden
        };

        Advertisement.Banner.Show(adUnitId, options);
    }

    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    #region ShowCallBacks
    private void BannerHiden() {    }

    private void BannerClicked() {    }

    private void BannerShown() {    }
    #endregion
    #region LoadCallBacks
    private void BannerLoadedError(string message)
    {
        throw new NotImplementedException();
    }

    private void BannerLoaded()
    {
        Debug.Log("Banner Ad Loaded");
    }
    #endregion
}

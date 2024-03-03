using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public InitializeAds initializeAds;
    public BannerAds bannerAds;
    public RewardedAds rewardedAds;
    public InterstitialAds interstitialAds;

    public static AdsManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        bannerAds.LoadBannerAd();
        interstitialAds.LoadInterstatialAd();
        rewardedAds.LoadRewardedAd();
    }
}

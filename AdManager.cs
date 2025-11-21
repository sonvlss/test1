using System;
using System.IO;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{

    // Singleton instence
    public static AdManager Instance;
    public event System.Action OnRewardEarned;

    // Ad Unit IDs (use test IDs for development)
#if UNITY_ANDROID
    private string bannerAdUnitId = "ca-app-pub-7044286027808355/4452473566";
    private string interestitialAdUnitId = "ca-app-pub-7044286027808355/4355468318";
    private string rewardedAdUnitId = "ca-app-pub-7044286027808355/7885286269";

#elif UNITY_IPHONE
    private string bannerAdUnitId = "ca-app-pub-7044286027808355/4480393720";
    private string interestitialAdUnitId = "ca-app-pub-7044286027808355/8103141638";
    private string rewardedAdUnitId = "ca-app-pub-7044286027808355/4707339023";
    
#else
    private string bannerAdUnitId = "unused";
    private string interestitialAdUnitId = "unused";
    private string rewardedAdUnitId = "unused";
    
    
#endif


    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Instance = this;
            Destroy(gameObject);
        }


    }


    void Start()
    {
        // Initialize the google mobile ads SDK
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Admob SDK Initilized");
            //Load ads after initialization
            LoadBannerAd();
            LoadInterstitialAd();
            LoadRewardedAd();

        });
    }

    private void LoadBannerAd()
    {
        // Create a banner view at the bottom of the screen
        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request
        AdRequest request = new AdRequest();

        // Register event handlers for the banner ad
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner ad loaded.");
        };

        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.Log("Banner ad failed to load: " + error.GetMessage());
        };

        // Load the banner ad
        bannerView.LoadAd(request);


    }

    private void LoadInterstitialAd()
    {

        // Clean up any existing interstitial ad
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        // Load a new interstitial ad
        InterstitialAd.Load(interestitialAdUnitId, new AdRequest(), (InterstitialAd ad, LoadAdError error) => {

            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load: " + error?.GetMessage());
                return;
            }
            ;

            interstitialAd = ad;
            Debug.Log("Interstitial ad loaded");

            // Register ad events
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial ad closed");
                LoadInterstitialAd(); // Preload the next ad
            };

            interstitialAd.OnAdFullScreenContentFailed += (error) =>
            {
                Debug.Log("Interstitial ad failed to show: " + error.GetMessage());
            };
        });

    }


    public void ShowIntersitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready");
        }
    }

    private void LoadRewardedAd()
    {

        // Clean up any existing rewarded ad
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;

        }

        // Load a new rewarded ad
        RewardedAd.Load(rewardedAdUnitId, new AdRequest(), (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load:" + error?.GetMessage());
                return;
            }

            rewardedAd = ad;
            Debug.Log("Rewarded ad loaded.");

            // Register ad events
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.LogError("Rewarded ad closed.");
                LoadRewardedAd();
            };
            rewardedAd.OnAdFullScreenContentFailed += (error) =>
            {
                Debug.LogError("Rewarded ad failed to show:" + error.GetMessage());
            };
        });


    }


    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"User earned reward: {reward.Amount} {reward.Type}");
                OnRewardEarned?.Invoke();
            });
        }
        else
        {
            Debug.Log("Rewarded ad not ready");
        }
    }





    // Update is called once per frame
    void Update()
    {

    }
}
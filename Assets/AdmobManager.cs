using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class AdmobManager : MonoBehaviour
{
    private static AdmobManager _instance = null;
    public bool isTestMode;

    public static AdmobManager instance
    {
        get
        {
            if (_instance == null)
                return null;
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        LoadBannerAd();
        LoadFrontAd();
        LoadRewardAd();
    }

    AdRequest GetAdRequest()
    {
        Debug.Log("AdRequest");
        return new AdRequest.Builder().AddTestDevice("17F8A70AFCFE114").AddTestDevice("C9D12CC89E48B898").AddTestDevice("A4EF468A1BC5516D").AddTestDevice("4760F000CD4CA4C5").Build();
    }

    #region 배너 광고
    const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    const string bannerID = "ca-app-pub-5708876822263347/3087649413";
    BannerView bannerAd;


    void LoadBannerAd()
    {
        AdSize adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        bannerAd = new BannerView(isTestMode ? bannerTestID : bannerID, adSize, AdPosition.Top);
        bannerAd.LoadAd(GetAdRequest());
    }

    public void ToggleBannerAd(bool b)
    {
        Debug.Log(b);
        if (b) bannerAd.Show();
        else bannerAd.Hide();
    }
    #endregion

    #region 전면 광고
    const string frontTestID = "ca-app-pub-3940256099942544/8691691433";
    const string frontID = "ca-app-pub-5708876822263347/8006151793";
    InterstitialAd frontAd;


    void LoadFrontAd()
    {
        frontAd = new InterstitialAd(isTestMode ? frontTestID : frontID);
        Debug.Log(frontAd);
        frontAd.LoadAd(GetAdRequest());
        frontAd.OnAdClosed += (sender, e) =>
        {
            Debug.Log("Ad End");
        };
    }

    public void ShowFrontAd()
    {
        Debug.Log("FrontAd");
        frontAd.Show();
        LoadFrontAd();
    }
    #endregion

    #region 리워드 광고
    const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    const string rewardID = "ca-app-pub-5708876822263347/4215080592";
    RewardedAd rewardAd;


    void LoadRewardAd()
    {
        rewardAd = new RewardedAd(isTestMode ? rewardTestID : rewardID);
        rewardAd.LoadAd(GetAdRequest());
        rewardAd.OnUserEarnedReward += (sender, e) =>
        {
        };
    }

    public void ShowRewardAd()
    {
        rewardAd.Show();
        LoadRewardAd();
    }
    #endregion
}

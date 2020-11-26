using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class Ads : MonoBehaviour
{
    BannerView bannerView;
    string adUnitId = "ca-app-pub-3344685387550142/7674245817";
    string adTestId = "ca-app-pub-3940256099942544/6300978111";
    bool testAd = false;
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
    }

    public void RequestBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        bannerView = new BannerView(testAd? adTestId : adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
}

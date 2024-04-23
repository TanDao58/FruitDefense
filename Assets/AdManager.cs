using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class AdManager : MonoBehaviour
{
    public List<UnityEvent> adEvents;

    public static AdManager Instance;

    int watchAdsId;

    private void Awake()
    {
        Instance = this;
    }

    public void HandleEarnReward()
    {
        if (watchAdsId < adEvents.Count)
        {
            UnityEvent e = adEvents[watchAdsId];
            e?.Invoke();
        }
        GoogleAdMobController.Instance.RequestAndLoadRewardedAd();
    }

    public void WatchAds(int id)
    {
        watchAdsId = id;
        GoogleAdMobController.Instance.ShowRewardedAd();
    }

    public void ShowIntertistialAds()
    {
        if (GameSystem.userdata.boughtItems == null) GameSystem.userdata.boughtItems = new List<string>();
        if (GameSystem.userdata.boughtItems.Contains(IAP_ID.no_ads.ToString())) return;
        if (GameSystem.userdata.isVipMember) return;
        GoogleAdMobController.Instance.ShowInterstitialAd();
    }
}

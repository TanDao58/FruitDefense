using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class MineGemController : MonoBehaviour
{
    public List<Image> diamonds;
    public TextMeshProUGUI txtDiamond;
    public TextMeshProUGUI timer;
    public float durationToEarnGem;
    public Button btnGet;
    public Button btnWatchAds;
    public CollectEffect effect;
    bool isWaiting;
    float nextUpdate;

    void Update()
    {
        if (Time.time < nextUpdate) return;
        nextUpdate = Time.time + 1f;

        long ticks = DateTime.Now.Ticks - GameSystem.userdata.lastEarnGemTime;
        double seconds = new TimeSpan(ticks).TotalSeconds;
        double maxSeconds = TimeSpan.FromMinutes(durationToEarnGem * 60).Ticks;
        if (seconds > maxSeconds) seconds = maxSeconds;

        isWaiting = seconds < durationToEarnGem * 60;
        if (isWaiting) {
            timer.text = TimeSpan.FromSeconds(durationToEarnGem * 60 - seconds).ToString(@"mm\:ss");
        } else {
            timer.text = "";
        }
        btnGet.gameObject.SetActive(!isWaiting);
        btnWatchAds.gameObject.SetActive(isWaiting);
    }

    public void OnWatchAdsFinished() {
        GameSystem.userdata.lastEarnGemTime = 0;
        GameSystem.SaveUserDataToLocal();
    }

    public void OnGetButton()
    {
        GameSystem.userdata.diamond += Constants.WATCH_ADS_GEM_RECEIVE_AMOUNT;
        GameSystem.userdata.lastEarnGemTime = DateTime.Now.Ticks;
        GameSystem.SaveUserDataToLocal();
        Home.Instance.UpdateGemText();

        effect.DoEffect();
    }
}
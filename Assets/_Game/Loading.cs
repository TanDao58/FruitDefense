using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Loading : MonoBehaviour
{
    public Image imgBackground;
    public GoogleAdMobController googleAd;
    float startLoadingTime;
    AsyncOperation loading;
    public bool changeScene = false;

    private void Start()
    {
        startLoadingTime = Time.time;
        string sceneName = Constants.SCENE_HOME;
        if (PlayerPrefs.GetInt("playIntro", 0) == 0)
        {
            sceneName = Constants.SCENE_INTRO;
            PlayerPrefs.SetInt("playIntro", 1);
        }
        loading = SceneManager.LoadSceneAsync(sceneName);
        loading.allowSceneActivation = false;
        imgBackground.color = new Color(1, 1, 1, 0);
        imgBackground.DOFade(1f, 1f);
    }

    private void Update()
    {
        CheckForAppOpenAd();
    }

    public void CheckForAppOpenAd()
    {
        if (loading == null) return;
        float time = Time.time - startLoadingTime;

        if (changeScene == false && time > Constants.APP_OPEN_LOADING_TIME_OUT && googleAd.isShowingAppOpenAd == false)
        {
            changeScene = true;
            loading.allowSceneActivation = true;
        }
    }

    public void ContinueToScene()
    {
        if (SceneManager.GetActiveScene().name != Constants.SCENE_LOADING) return;
        loading.allowSceneActivation = true;
    }
}
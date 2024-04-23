using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    public const float SCALE_TIME = 5f;

    public Transform mask;
    public SpriteRenderer spr1;
    public SpriteRenderer spr2;
    AsyncOperation load;

    int step = 0;

    void Start()
    {
        Time.timeScale = 1f;
        load = SceneManager.LoadSceneAsync(Constants.SCENE_HOME);
        load.allowSceneActivation = false;
        LeanTween.delayedCall(0.5f, () =>
        {
            NextStep();
        });
    }

    public void NextStep()
    {
        step++;

        if (step == 1)
        {
            spr1.gameObject.SetActive(true);
            spr2.gameObject.SetActive(false);
            mask.DOKill();
            mask.transform.localScale = Vector2.zero;
            mask.DOScale(17f, SCALE_TIME);
        } else if (step == 2)
        {
            spr1.gameObject.SetActive(false);
            spr2.gameObject.SetActive(true);
            mask.DOKill();
            mask.transform.localScale = Vector2.zero;
            mask.DOScale(17f, SCALE_TIME).OnComplete(() =>
            {
                load.allowSceneActivation = true;
            });
        } else
        {
            load.allowSceneActivation = true;
        }
    }
}
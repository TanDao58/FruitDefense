using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EasyEffect : MonoBehaviour
{
    public static EasyEffect Instance;
    private void Awake()
    {
        Instance = this;
    }
    public static void Appear(GameObject obj, float startScale, float endScale, float speed = 0.2f, float maxScale = 1.2f, Action doneAction = null)
    {
        obj.transform.localScale = new Vector3(startScale, startScale);
        LeanTween.scale(obj, new Vector3(maxScale, maxScale, maxScale) * endScale, speed).setOnComplete(() =>
        {
            LeanTween.scale(obj, new Vector3(1f, 1f, 1f) * endScale, speed).setOnComplete(() => doneAction?.Invoke());
        });
    }

    public static void Disappear(GameObject obj, float startScale, float endScale, float speed = 0.2f, float maxScale = 1.2f, Action doneAction = null)
    {
        obj.transform.localScale = new Vector3(startScale, startScale);
        LeanTween.scale(obj, new Vector3(maxScale, maxScale, maxScale) * startScale, speed).setOnComplete(() =>
        {
            LeanTween.scale(obj, new Vector3(1f, 1f, 1f) * endScale, speed).setOnComplete(() =>
            {
                doneAction?.Invoke();
                obj.SetActive(false);
            });
        });
    }

    const float DEFAULT_BOUNCE_STRENGTH = 0.5f;

    public static void Bounce(GameObject go, float time, float strength = DEFAULT_BOUNCE_STRENGTH, Action doneAction = null)
    {
        float baseScale = go.transform.localScale.x;

        LeanTween.scale(go, new Vector3(1 + strength, 1 - strength) * baseScale, time).setOnComplete(() =>
        {
            LeanTween.scale(go, new Vector3(1 - strength, 1 + strength) * baseScale, time).setOnComplete(() =>
            {
                LeanTween.scale(go, new Vector3(1f, 1f) * baseScale, time).setOnComplete(() => doneAction?.Invoke());
            });
        });
    }

    public static void UfoCatch(GameObject obj, float to, float time, Action doneAction = null)
    {
        LeanTween.scale(obj, Vector3.zero, time);
        LeanTween.moveY(obj, to, time).setOnComplete(() =>
        {
            obj.SetActive(false);
            doneAction?.Invoke();
        });
    }
    public static void Blinking(SpriteRenderer spriteRenderer, float duration, int blinkingTimes)
    {
        Instance.StartCoroutine(IEBlinking(spriteRenderer, duration, blinkingTimes));
    }

    static IEnumerator IEBlinking(SpriteRenderer spriteRenderer, float duration, int blinkingTimes)
    {
        float FADE_TIME = duration;
        Color color = spriteRenderer.color;

        for (int i = 0; i < blinkingTimes; i++)
        {
            LeanTween.value(1f, 0f, FADE_TIME).setOnUpdate((float f) =>
            {
                color.a = f;
                spriteRenderer.color = color;
            });
            yield return new WaitForSeconds(FADE_TIME);

            LeanTween.value(0f, 1f, FADE_TIME).setOnUpdate((float f) =>
            {
                color.a = f;
                spriteRenderer.color = color;
            });
            yield return new WaitForSeconds(FADE_TIME);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace DarkcupGames
{
    public class EasyEffect : MonoBehaviour
    {
        const float DEFAULT_BOUNCE_STRENGTH = 0.5f;
        public static EasyEffect Instance;

        private void Awake()
        {
            Instance = this;
        }

        public static void Appear(GameObject obj, float startScale, float endScale, float speed = 0.1f, float maxScale = 1.2f, Action doneAction = null)
        {
            obj.SetActive(true);
            obj.transform.localScale = new Vector3(startScale, startScale);
            LeanTween.scale(obj, new Vector3(maxScale, maxScale, maxScale) * endScale, speed).setOnComplete(() =>
            {
                LeanTween.scale(obj, new Vector3(1f, 1f, 1f) * endScale, speed)
                .setIgnoreTimeScale(true)
                .setOnComplete(() => 
                { 
                    doneAction?.Invoke(); 
                });
            }).setIgnoreTimeScale(true);
        }

        public static void Disappear(GameObject obj, float startScale, float endScale, float speed = 0.1f, float maxScale = 1.2f, Action doneAction = null)
        {
            obj.transform.localScale = new Vector3(startScale, startScale);
            LeanTween.scale(obj, new Vector3(maxScale, maxScale, maxScale) * startScale, speed).setOnComplete(() =>
            {
                LeanTween.scale(obj, new Vector3(1f, 1f, 1f) * endScale, speed).setOnComplete(() =>
                {
                    doneAction?.Invoke();
                    obj.SetActive(false);
                    obj.transform.localScale = Vector3.one;
                }).setIgnoreTimeScale(true);
            }).setIgnoreTimeScale(true);
        }

        public static void Bounce(GameObject go, float time, float strength = DEFAULT_BOUNCE_STRENGTH)
        {
            float baseScale = go.transform.localScale.x;

            LeanTween.scale(go, new Vector3(1 + strength, 1 - strength) * baseScale, time).setOnComplete(() =>
            {
                LeanTween.scale(go, new Vector3(1 - strength, 1 + strength) * baseScale, time).setOnComplete(() =>
                {
                    LeanTween.scale(go, new Vector3(1f, 1f) * baseScale, time).setIgnoreTimeScale(true);
                }).setIgnoreTimeScale(true);
            }).setIgnoreTimeScale(true);
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

        public static void RunTextNumber(TextMeshProUGUI txtNumber, long from, long to, float effectTime, string endText = "")
        {
            Instance.StartCoroutine(Instance.IEIncreaseNumber(txtNumber, from, to, effectTime, endText));
        }

        public IEnumerator IEIncreaseNumber(TextMeshProUGUI txtNumber, long startGold, long endGold, float effectTime, string endText = "")
        {
            long increase = (long)((endGold - startGold) / (effectTime / Time.deltaTime));
            if (increase == 0)
            {
                increase = endGold > startGold ? 1 : -1;
            }
            long gold = startGold;
            bool loop = true;
            while (loop)
            {
                gold += increase;
                if (startGold < endGold)
                {
                    loop = gold < endGold;
                }
                else
                {
                    loop = gold > endGold;
                }
                txtNumber.text = gold.ToString() + endText;
                yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            }
            txtNumber.text = endGold.ToString() + endText;
        }

        public static void Show(GameObject obj)
        {
            obj.SetActive(true);
            obj.GetComponent<UIEffect>()?.DoEffect();
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
}
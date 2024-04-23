using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class SkillEffect : MonoBehaviour
{
    public float flyTime = 0.5f;
    public float scaleTime = 0.5f;
    public float fadeTime = 1f;
    public float flyDistance = 2f;
    public float maxScale = 2f;

    SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        sprite.color = Color.white;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(transform.position + new Vector3(0f, flyDistance), flyTime));
        sequence.AppendCallback(() =>
        {
            transform.SetParent(null);
        });
        sequence.Append(transform.DOScale(Vector3.one * maxScale, scaleTime));
        sequence.Join(sprite.DOFade(0f, fadeTime));
        sequence.AppendCallback(() =>
        {
            gameObject.SetActive(false);
        });
    }
}

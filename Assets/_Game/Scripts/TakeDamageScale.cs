using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;
using Spine;

[RequireComponent(typeof(MonsterAI))]
public class TakeDamageScale : MonoBehaviour
{
    MonsterAI monsterAI;
    float nextEffect;
    Vector3 baseScale;

    private void OnEnable()
    {
        transform.localScale = baseScale;
        nextEffect = Time.time + 2f;
    }

    private void Awake()
    {
        monsterAI = GetComponent<MonsterAI>();
        baseScale = transform.localScale;
    }

    public void TakeDame()
    {
        if (Time.time < nextEffect) return;
        nextEffect = Time.time + 0.7f;
        EasyEffect.Bounce(monsterAI.gameObject, 0.1f, 0.15f);
    }
}
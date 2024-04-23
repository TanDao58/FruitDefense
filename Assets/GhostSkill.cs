using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GhostSkill : MonoBehaviour
{
    public const float INVISIBLE_SPEED_INCREASE = 4.5f;
    public const int HIT_TO_SKILL = 5;
    public const float INVISIBLE_TIME = 0.35f;
    public AudioClip sound;
    public SpriteRenderer mana;
    private MonsterEffect monsterEffect;
    private MonsterAI monsterAI;
    private int takeDamgeTime;
    float lastUseSkill = 0f;

    private void OnEnable() {
        monsterEffect = GetComponent<MonsterEffect>();
        monsterAI = GetComponent<MonsterAI>();
        takeDamgeTime = HIT_TO_SKILL - 1;
    }

    private void Update()
    {
        var value = mana.transform.localScale;
        value.x = (float)takeDamgeTime / HIT_TO_SKILL;
        if (value.x > 1f) value.x = 1f;
        mana.transform.localScale = value;
    }

    public void TurnInvisible()
    {
        takeDamgeTime++;

        if (takeDamgeTime < HIT_TO_SKILL) return;
        if (Time.time < lastUseSkill + 5f) return;
        lastUseSkill = Time.time;

        monsterEffect.Invisible(INVISIBLE_TIME);
        monsterAI.lastAttack = Time.time + INVISIBLE_TIME - monsterAI.battleStat.attackInterval;
        takeDamgeTime = 0;
        SFXSystem.Instance.Play(sound);
    }
}
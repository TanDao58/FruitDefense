using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceDebuff : MonoBehaviour
{
    public Sprite sprFreeze;
    private MonsterAI monsterAI;
    AnimationSetter animationSetter;
    private void Start()
    {
        animationSetter = GetComponentInChildren<AnimationSetter>();
        monsterAI = GetComponent<MonsterAI>();
    }

    public void SlowDebuff()
    {
        animationSetter.SetAnimation("Ulti");
        monsterAI.AttackTarget.TryGetComponent<MonsterEffect>(out var monster);
        monster.Slow(2f);
    }

    public void FreezeAOE()
    {
        var skillEffect = ObjectPool.Instance.GetGameObjectFromPool<SkillEffect>("Vfx/SkillFx", transform.position);
        skillEffect.GetComponent<SpriteRenderer>().sprite = sprFreeze;

        var enemies = Physics2D.OverlapCircleAll(transform.position, 3f, LayerMask.GetMask(monsterAI.TargetLayer));
        for (int i = 0; i < enemies.Length; i++)
        {
            var target = enemies[i].GetComponent<MonsterEffect>();
            if (target == null) return;
            if (target.froze || target.GetComponent<MonsterAI>().IsDead()) return;
            target.Freeze(2f);
        }
    }
}

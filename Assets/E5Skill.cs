using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class E5Skill : MonoBehaviour
{
    [SerializeField]private MonsterAI monsterAI;

    public void EarthWake()
    {
        monsterAI.AimSetter.SkeletonAnimation.AnimationState.SetAnimation(0, "Ulti", false);
        var duration = monsterAI.AimSetter.SkeletonAnimation.Skeleton.Data.FindAnimation("Ulti").Duration;
        LeanTween.delayedCall(duration, () =>
        {
            monsterAI.AimSetter.SkeletonAnimation.AnimationName = "Idle";
        });
        monsterAI.AimSetter.SkeletonAnimation.AnimationState.Event -= EventHandle;       
        monsterAI.AimSetter.SkeletonAnimation.AnimationState.Event += EventHandle;
        monsterAI.waitForSkillFinish = Time.time + duration;
    }

    private void EventHandle(TrackEntry trackEnty, Spine.Event e)
    {
        if (e.Data.Name != "OnUlti") return;
            var enemies = Physics2D.OverlapCircleAll(transform.position, 3f, LayerMask.GetMask(monsterAI.TargetLayer));
        ObjectPool.Instance.GetGameObjectFromPool("Explode VFX", transform.position);
        foreach (var enemy in enemies)
        {
            enemy.TryGetComponent<MonsterAI>(out var monster);            
            monster.TakeDame(monsterAI.HitParam);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;

public class GainSkill : MonoBehaviour
{
    [Header("AOE Skill")]
    [SerializeField] private float aoeRange;
    [SerializeField] private float pushForce;
    [SerializeField] private Transform skillPoint;
    AnimationSetter animationSetter;
    private MonsterAI monster;



    private void Start()
    {
        animationSetter = GetComponentInChildren<AnimationSetter>();
        monster = GetComponent<MonsterAI>();
    }
    public void AOEAttack()
    {
        
        monster.AimSetter.SkeletonAnimation.AnimationState.SetAnimation(0, "Ulti", false);
        var duration = monster.AimSetter.SkeletonAnimation.Skeleton.Data.FindAnimation("Ulti").Duration;
        LeanTween.delayedCall(duration, () =>
        {
            monster.AimSetter.SkeletonAnimation.AnimationName = "Idle";
        });
        monster.waitForSkillFinish = Time.time + duration;
        monster.AimSetter.SkeletonAnimation.AnimationState.Event -= EventHandle;
        monster.AimSetter.SkeletonAnimation.AnimationState.Event += EventHandle;
    }

    private void EventHandle(TrackEntry trackEnty, Spine.Event e)
    {
        if (e.Data.Name != "OnUlti") return;
        //if (skillPoint == null) Debug.LogError("null skill point at monster name: " + gameObject.name);
        var fxPath = GeneralUltility.BuildString("", "Vfx/", monster.monsterData.monsterName, "Skill");
        var obj = Resources.Load<GameObject>(fxPath);
        if (obj != null) ObjectPool.Instance.GetGameObjectFromPool(obj, skillPoint.transform.position);
        var enemies = Physics2D.OverlapCircleAll(transform.position, aoeRange, LayerMask.GetMask(monster.TargetLayer));
        foreach (var enemy in enemies)
        {
            enemy.TryGetComponent<MonsterAI>(out var enemyAI);
            enemyAI.TakeDame(monster.HitParam);
            var pushDirection = enemyAI.transform.position - transform.position;
            pushDirection.x = 0;
            enemy.attachedRigidbody.velocity = pushDirection.normalized * pushForce;
            LeanTween.delayedCall(0.3f, () => { enemy.attachedRigidbody.velocity = Vector2.zero; });
        }
    }
}

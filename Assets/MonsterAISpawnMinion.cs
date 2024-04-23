using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterAISpawnMinion : MonsterAI
{
    [SerializeField] private string minionName;
    [SerializeField] private float radius;
    [SerializeField] private PointFollower[] spawnPoints;
    [HideInInspector] public List<MonsterAI> spawnedMonsters;
    public UnityEvent onSpawnMinionFinished;

    public override void OnEnable()
    {
        base.OnEnable();
        if (spawnPoints == null || spawnPoints.Length == 0)
            spawnPoints = GetComponentsInChildren<PointFollower>();
    }

    protected override void AttackEventHandle(TrackEntry entry, Spine.Event e)
    {
        if (e.Data.Name.Equals("OnAttack"))
        {
            AttackEvent();
        }
        else if (/*e.Data.Name.Equals("OnUlti") ||*/ e.Data.Name.Equals("Spawn"))
        {
            UseSkill();
        }
    }

    public void UseSkill()
    {
        SpawnMinion();
    }

    /// <summary>
    /// Yêu cầu spine phải có PointFollower và skin theo số (1,2,3,4)
    /// </summary>
    /// <returns></returns>
    private void SpawnMinion()
    {
        spawnedMonsters = new List<MonsterAI>();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            var monster = ObjectPool.Instance.GetGameObjectFromPool<MonsterAI>(minionName, spawnPoints[i].transform.position);
            EasyEffect.Appear(monster.gameObject, 0f, 1f);
            monster.IsEnemy = IsEnemy;
            var par = ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>("Vfx/MinionAppear", spawnPoints[i].transform.position);
            par.ChangeColor(monster.allyColor);
            var data = monster.GetComponentInChildren<SkeletonAnimation>().Skeleton;
            if (data.Skin != null)
                data.SetSkin((i + 1).ToString());
            spawnedMonsters.Add(monster);
        }
        if (onSpawnMinionFinished != null)
        {
            onSpawnMinionFinished.Invoke();
        }
    }

    public void DoUltiAnimation()
    {
        var ultiAnim = AimSetter.SkeletonAnimation.Skeleton.Data.FindAnimation("Ulti");
        if (ultiAnim != null)
        {
            AimSetter.SkeletonAnimation.AnimationState.SetAnimation(0, "Ulti", false);
            waitForSkillFinish = Time.time + ultiAnim.Duration;
        }
    }

    public override void Die()
    {
        base.Die();
        animationSetter.SkeletonAnimation.AnimationState.Event -= AttackEventHandle;
        animationSetter.SkeletonAnimation.AnimationState.Event += AttackEventHandle;
    }

    public void SetMonsterAsBoss()
    {
        if (spawnedMonsters == null || spawnedMonsters.Count == 0) return;
        for (int i = 0; i < spawnedMonsters.Count; i++)
        {
            spawnedMonsters[i].isBoss = true;
        }
    }
}
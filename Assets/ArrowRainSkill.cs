using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using DG.Tweening;

public class ArrowRainSkill : MonoBehaviour
{
    private MonsterAI monster;
    private ParticalSystemController fx;
    private Camera mainCam;
    private RipplePostProcessor ripple;
    [SerializeField] private float raidus;
    private WaitForSeconds wait;
    private HitParam hitParam;

    [SerializeField] private AudioClip shootSfx;
    [SerializeField] private AudioClip hitSfx;
    [SerializeField] private AudioClip exploseSfx;
    [SerializeField] private AudioClip ultiChargeSfx;

    private void Start()
    {
        monster = GetComponent<MonsterAI>();
        mainCam = Camera.main;
        ripple = mainCam.GetComponent<RipplePostProcessor>();
        hitParam = new HitParam();
        hitParam.owner = monster.transform;
        hitParam.damage = monster.HitParam.damage/3;
        List<string> list = new List<string>()
        {
            "Vfx/ChargeFx","Vfx/E17ShootFx","Vfx/Spark","Vfx/ArrowField","Vfx/ArrowHit"
        };
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i]);
            var obj = ObjectPool.Instance.GetGameObjectFromPool(list[i], new Vector3(99f, 99f));
            obj.gameObject.SetActive(false);
        }
    }
    public void UseSkill()
    {       
        monster.AimSetter.SkeletonAnimation.AnimationState.SetAnimation(0, "Ulti", false);
        var duration = monster.AimSetter.SkeletonAnimation.Skeleton.Data.FindAnimation("Ulti").Duration;
        LeanTween.delayedCall(duration, () =>
        {
            //monster.AimSetter.SkeletonAnimation.AnimationName = monster.idleAnimationName;
        });
        monster.AimSetter.SkeletonAnimation.AnimationState.Event -= ShootArrowRain;
        monster.AimSetter.SkeletonAnimation.AnimationState.Event += ShootArrowRain;
        monster.waitForSkillFinish = Time.time + duration;
    }

    private void ShootArrowRain(TrackEntry track, Spine.Event e)
    {
        var monsterRange = (MonsterAIRange)monster;
        if (e.Data.Name.Equals("UltiCharge"))
        {
            if (fx == null) fx = ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>("Vfx/ChargeFx", monsterRange.firePoint.position);
            else
            {
                fx.gameObject.SetActive(true);
                fx.transform.position = monsterRange.firePoint.position;
            }
            fx.SetSortingOrderWithChildren(monster.MeshOrder.meshRenderer.sortingOrder);
            SFXSystem.Instance.Play(ultiChargeSfx);
        }
        if(e.Data.Name.Equals("OnUlti"))
        {
            ripple.CallRippleEffect(mainCam.WorldToScreenPoint(monsterRange.firePoint.position));
            monster.battleStat.attackInterval = monster.monsterData.attackInterval;
            if (fx != null)
            {
                fx.gameObject.SetActive(false);
                fx = null;
            }
            ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>("Vfx/E17ShootFx", monsterRange.firePoint.position);
            SFXSystem.Instance.Play(shootSfx);
            var spark = ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>("Vfx/Spark", monsterRange.firePoint.position);
            spark.SetSortingOrderWithChildren(monster.MeshOrder.meshRenderer.sortingOrder);
            if (monster.AttackTarget == null) monster.UpdateTarget();
            StartCoroutine(DoArrowRain(monster.AttackTarget));
        }
    }

    private IEnumerator DoArrowRain(Transform target)
    {
        Vector2 center = transform.position;
        if (target != null) center = target.position;
        if (wait == null) wait = new WaitForSeconds(0.02f);
        var field = ObjectPool.Instance.GetGameObjectFromPool("Vfx/ArrowField", center);
        SFXSystem.Instance.Play(hitSfx);
        for (int i = 0; i < 20; i++)
        {
            var destination = (Vector2)center + Random.insideUnitCircle.normalized * raidus;
            var spawnPosition = destination + new Vector2(0f, 30f);
            var arrow = ObjectPool.Instance.GetGameObjectFromPool("FireBall", spawnPosition);
            arrow.transform.DOMove(destination, 0.2f).OnComplete(() =>
            {
                arrow.gameObject.SetActive(false);
                ObjectPool.Instance.GetGameObjectFromPool("Vfx/ArrowHit", arrow.transform.position);
                //if(i % 20 == 0) SFXSystem.Instance.Play(hitSfx);
                foreach (var enemy in EnemySpawner.Instance.spawnedEnemies)
                {
                    var distanceToExplosion = Vector2.Distance(center, enemy.transform.position);
                    if (distanceToExplosion <= raidus)
                    {
                        enemy.TakeDame(hitParam);
                    }
                }
            });
            yield return wait;
        }
        yield return new WaitForSeconds(0.75f);
        field.SetActive(false);
        SFXSystem.Instance.Play(exploseSfx);
        ObjectPool.Instance.GetGameObjectFromPool("Vfx/Explosion", center);
        foreach (var enemy in EnemySpawner.Instance.spawnedEnemies)
        {
            var distanceToExplosion = Vector2.Distance(center, enemy.transform.position);
            if (distanceToExplosion <= raidus)
            {
                enemy.TakeDame(monster.HitParam);
            }
        }
    }

    public void DisableChargeFx()
    {
        if (fx != null) fx.gameObject.SetActive(false);
    }
}

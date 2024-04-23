using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class MonsterAIRange : MonsterAI
{
    [System.NonSerialized] public bool charge;
    public Transform firePoint;
    [SpineBone]
    [SerializeField] private string gunBone;
    [SerializeField] private Sprite bulletSprite;
    public Transform tempFirePoint;

    public override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void AttackEvent()
    {
        if (attackTarget == null || attackTarget.gameObject.activeInHierarchy == false) return;
        var bulletPath = GeneralUltility.BuildString("", monsterData.monsterName, "Bullet");      
        
        if (firePoint == null) {
            Debug.LogError($"firepoint is null on game object {gameObject.name}");
        }
        if (attackSound != null) {
            SFXSystem.Instance.Play(attackSound);
        }
        var direction = attackTarget.position - firePoint.position;
        var bulletObject = ObjectPool.Instance.GetGameObjectFromPool(bulletPath, firePoint.position);
        var bullet = bulletObject.GetComponent<Bullet>();
        bullet.direction = direction;
        bullet.target = attackTarget;
        bullet.owner = this;
        bullet.hitParam = hitParam;
        if(bullet.applyFlyFx)
        {
            var fx = ObjectPool.Instance.GetGameObjectFromPool("Vfx/BulletFyFx", transform.position);
            fx.transform.right = direction;
        }
        var fxPath = GeneralUltility.BuildString("", "Vfx/", LayerMask.LayerToName(gameObject.layer), "ShootFx");
        if(!vfxPrefab.Equals(string.Empty)) fxPath = GeneralUltility.BuildString("", "Vfx/", monsterData.monsterName, "ShootFx");
        var controller = ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>(fxPath, bullet.transform.position);
        //controller.FlipWithPivot(-animationSetter.SkeletonAnimation.transform.localScale.x);
        controller.transform.right = -direction;
        controller.SetSortingOrderWithChildren(meshOrder.meshRenderer.sortingOrder);
    }

    public override void UpdateTarget()
    {
        foreach (var ufo in EnemySpawner.Instance.ufos)
        {
            var distance = Vector2.Distance(transform.position, ufo.transform.position);
            if (ufo.gameObject.activeInHierarchy && distance <= battleStat.visionRange && ufo.gameObject.layer == LayerMask.NameToLayer(targetLayer))
            {
                attackTarget = ufo.transform;
                return;
            }
        }
        base.UpdateTarget();
    }

    //public void GetFirePoint()
    //{
    //    var bonePos = animationSetter.SkeletonAnimation.Skeleton.FindBone(gunBone).GetLocalPosition();
    //    firePoint = ((Vector2)transform.position - bonePos).normalized;
    //}
    public override void Die()
    {
        animationSetter.SkeletonAnimation.SkeletonDataAsset.Clear();
        base.Die();       
    }

    public override void StateAttack()
    {
        if (charge) return;
        base.StateAttack();
        //transform.position = Vector2.MoveTowards(transform.position, attackTarget.position + new Vector3(2f,2f), battleStat.speed * Time.deltaTime * Time.timeScale);
    }
    public override void ChangeSide()
    {
        base.ChangeSide();
    }
}

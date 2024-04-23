using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAIHealer : MonsterAI
{
    [SerializeField] private Transform firePoint;
    protected override void SetEnemyLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(LAYER_ENEMY);
        targetLayer = LAYER_ENEMY;
    }
    protected override void SetAllyLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(LAYER_ALLY);
        targetLayer = LAYER_ALLY;
    }
    public override void UpdateTarget()
    {
        Collider2D[] allTarget = Physics2D.OverlapCircleAll(transform.position, 100f, LayerMask.GetMask(targetLayer));
        Collider2D target = CheckAllyHealth(allTarget);
        if(target != null)
        {
            var targetDamable = target.GetComponent<IDamable>();
            if (!targetDamable.IsDead()) attackTarget = target.transform;
            else attackTarget = null;
        }
        else attackTarget = null;
    }

    //public override void DefaultAttack()
    //{
    //    attackId = Random.Range(0, 99999);
    //    int rand = attackId;
    //    //animationSetter.SetAnimation(attackAnimationName);

    //    animationSetter.SetAnimation(attackAnimationName, () =>
    //    {
    //        if (rand == attackId)
    //        {
    //            animationSetter.SetAnimation(idleAnimationName);
    //        }
    //    });
    //    animationSetter.SetFacing(attackTarget.transform.position);

    //}

    protected override void AttackEvent()
    {
        if (attackTarget == null) return;
        var bulletPath = GeneralUltility.BuildString("", monsterData.monsterName, "Bullet");
        var healBullet = ObjectPool.Instance.GetGameObjectFromPool<HealBullet>(bulletPath, firePoint.position);
        healBullet.direction = attackTarget.position - firePoint.position;
        healBullet.target = attackTarget;
        healBullet.owner = this;
        if (!isEnemy) ObjectPool.Instance.GetGameObjectFromPool("Vfx/AllyShootFx", healBullet.transform.position);
    }

    private Collider2D CheckAllyHealth(Collider2D[] colliders)
    {
        Collider2D result = null;
        float leastHp = float.MaxValue; //by percent

        foreach (var col in colliders)
        {
            var monster = col.GetComponent<MonsterAI>();
            float hp = monster.HealthBar.intansceHp.transform.localScale.x;

            if (monster != null && hp < 1)
            {
                if (result == null || hp < leastHp)
                {
                    result = col;
                    leastHp = hp;
                }
            }
        }
        return result;
    }
}
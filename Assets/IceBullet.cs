using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : Bullet
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (owner.AttackTarget == null || owner.AttackTarget.gameObject.activeInHierarchy == false) return;

        if(collision.gameObject.layer == owner.AttackTarget.gameObject.layer)
        {
            var enemies = Physics2D.OverlapCircleAll(transform.position, 3f, LayerMask.GetMask(LayerMask.LayerToName(owner.AttackTarget.gameObject.layer)));
            foreach (var enemy in enemies) 
            {
                enemy.GetComponent<MonsterEffect>().Freeze(2f);
                owner.HitParam.damage = damage;
            }
            collision.GetComponent<MonsterAI>().TakeDame(owner.HitParam);
            Disspear();
        }
    }
}

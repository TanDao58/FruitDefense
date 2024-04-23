using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenatrateBullet : Bullet
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(owner.TargetLayer))
        {
            collision.TryGetComponent<MonsterAI>(out var target);
            //owner.HitParam.crit = false;
            //owner.HitParam.damage = damage;
            ObjectPool.Instance.GetGameObjectFromPool("Vfx/SpecialHit", target.transform.position);
            target.TakeDame(owner.HitParam);           
            Disspear();
        }
    }
    protected override void Disspear()
    {
        if(owner.isEnemy) return;
        var explosePath = GeneralUltility.BuildString("", "Vfx/", LayerMask.LayerToName(owner.gameObject.layer), "BulletExplose");
        var explosion = ObjectPool.Instance.GetGameObjectFromPool(explosePath, transform.position);
        if (!owner.IsEnemy)
        {
            explosion.GetComponent<BulletExplose>().Explode(owner.HitParam);
            var controller = explosion.GetComponent<ParticalSystemController>();
            controller.ChangeColor(owner.allyColor);
        }
    }
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}

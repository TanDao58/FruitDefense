using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBullet : Bullet
{
    protected override void OnEnable()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            defaultImg = spriteRenderer.sprite;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == owner.AttackTarget)
        {
            collision.TryGetComponent<MonsterAI>(out var target);
            if (target != null) {
                float healValue = target.battleStat.maxhp * Constants.HEAL_PERCENT;
                target.Heal(healValue);
            }
            //owner.HitParam.damage = owner.battleStat.damage;
            //target.Heal(owner.HitParam.damage);
            Disspear();
        }
    }

    protected override void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime * Time.timeScale, Space.World);
        transform.Rotate(0, 0, 10f);
    }

    protected override void Disspear()
    {
        gameObject.SetActive(false);
    }
}

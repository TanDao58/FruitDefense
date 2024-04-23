using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAttack : MonoBehaviour
{
    private MonsterAI monster;
    private void Start()
    {
        monster = GetComponent<MonsterAI>();
    }

    public void Electrocute()
    {
        var electric = ObjectPool.Instance.GetGameObjectFromPool<ParticalSystemController>("Vfx/LightAtk",transform.position);
        electric.SetSortingOrder(monster.MeshOrder.meshRenderer.sortingOrder);
        var enemies = Physics2D.OverlapCircleAll(transform.position, 5.3f, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].TryGetComponent<MonsterAI>(out var enemy);
            var par = ObjectPool.Instance.GetGameObjectFromPool("Vfx/LightHit", enemy.transform.position);
            par.transform.localScale = enemy.transform.localScale;
            enemy.TakeDame(monster.HitParam);
            enemy.Effect.Stun(0.5f);
        }
    }
}

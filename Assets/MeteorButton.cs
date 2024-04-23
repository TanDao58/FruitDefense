using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeteorButton : BuffButton
{
    [SerializeField] private Transform spawnPoint;
    private List<MonsterAI> targetableEnemies = new List<MonsterAI>();
    private HitParam hitParam;
    private WaitForSeconds wait;

    protected override void Start()
    {
        base.Start();
        hitParam = new HitParam()
        {
            damage = 200
        };
        wait = new WaitForSeconds(0.2f);
    }
    public void DoMeteorFall()
    {
        targetableEnemies.Clear();
        var enemies = EnemySpawner.Instance.spawnedEnemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].gameObject.activeInHierarchy && enemies[i].targetable)
            {
                targetableEnemies.Add(enemies[i]);
            }
        }
        if (targetableEnemies.Count == 0) return;
        currentCoolDown = coolDown;
        var targetPosition = targetableEnemies[Random.Range(0, targetableEnemies.Count - 1)].transform.position;
        Amount--;
        StartCoroutine(CallMeteor(targetPosition));
    }

    private IEnumerator CallMeteor(Vector3 position)
    {
        for (int i = 0; i < 10; i++)
        {
            var fireball = ObjectPool.Instance.GetGameObjectFromPool("Fireball", spawnPoint.transform.position);
            var destination = (Vector2)position + Random.insideUnitCircle.normalized * 2.5f;

            fireball.transform.up = (Vector2)spawnPoint.position - destination;
            fireball.transform.DOMove(destination, 0.1f).OnComplete(() =>
            {
                var explose = ObjectPool.Instance.GetGameObjectFromPool<BulletExplose>("Vfx/MeteorExplose", fireball.transform.position);
                explose.Explode(hitParam);
                fireball.SetActive(false);
            });
            yield return wait;
        }
    }
}

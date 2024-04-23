using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinion : MonoBehaviour
{
    private MonsterAI monster;
    [SerializeField] private string minionName;
    [SerializeField] private int amount;
    [SerializeField] private float radius;

    private void Start()
    {
        monster = GetComponent<MonsterAI>();
    }
    public void Spawn()
    {
        for (int i = 0; i < amount; i++)
        {
            var pos = (Vector2)transform.position + (radius * Random.insideUnitCircle.normalized);
            var monster = ObjectPool.Instance.GetGameObjectFromPool<MonsterAI>(minionName, pos);
            EasyEffect.Appear(monster.gameObject, 0f, 1f);
            monster.IsEnemy = monster.isEnemy;
        }
    }
}

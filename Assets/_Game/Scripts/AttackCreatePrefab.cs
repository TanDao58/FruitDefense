using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCreatePrefab : MonoBehaviour
{
    public string prefabName;
    public float range = 1f;

    public void Attack()
    {
        var randomPos = new Vector3(Random.Range(-range, range), Random.Range(-range, range));
        GameObject obj = ObjectPool.Instance.GetGameObjectFromPool(prefabName, transform.position + randomPos);
        obj.GetComponent<MonsterAI>().IsEnemy = false;
    }
}

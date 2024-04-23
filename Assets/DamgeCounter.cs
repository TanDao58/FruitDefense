using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamgeCounter : MonoBehaviour
{
    private const float SHOW_INTERVAL = 0.3f;

    private MonsterAI monsterAI;
    public AnimationSetter animationSetter;
    float nextShowDamage;
    float currentTakeDame;

    private void Start()
    {
        monsterAI = GetComponent<MonsterAI>();
        animationSetter = GetComponentInChildren<AnimationSetter>();
    }

    public void TakeDamage(float damage,bool crit, bool showInstant = false)
    {
        if (monsterAI.IsEnemy == false || damage == 0) return;
        currentTakeDame += damage;

        if (Time.time > nextShowDamage || showInstant)
        {
            var number = ObjectPool.Instance.GetGameObjectFromPool("Number/DamgeNumber", transform.position);
            var numberMesh = number.GetComponent<DamgeNumber>();
            numberMesh.ShowNumber(currentTakeDame,crit);
        }
    }
}

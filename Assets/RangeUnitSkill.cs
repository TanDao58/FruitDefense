using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeUnitSkill : MonoBehaviour
{
    public Sprite sprFreeze;
    private MonsterAIRange monsterAI;
    private void Start()
    {
        monsterAI = GetComponent<MonsterAIRange>();
    }
    public void ShootPanatrateBullet()
    {
        var bulletObject = ObjectPool.Instance.GetGameObjectFromPool("PenatrateBullet", monsterAI.tempFirePoint.position);
        var bullet = bulletObject.GetComponent<Bullet>();
        if (monsterAI.AttackTarget == null) return;
        bullet.target = monsterAI.AttackTarget;
        bullet.direction = monsterAI.AttackTarget.position - monsterAI.transform.position;
        bullet.owner = monsterAI;
    }
    public void ShootFreezeBullet()
    {
        var skillEffect = ObjectPool.Instance.GetGameObjectFromPool<SkillEffect>("Vfx/SkillFx", transform.position);
        skillEffect.GetComponent<SpriteRenderer>().sprite = sprFreeze;

        StartCoroutine(IEShootFreezeBullet());
    }

    private IEnumerator IEShootFreezeBullet()
    {
        monsterAI.attackEvent.RemoveListener(monsterAI.DefaultAttack);
        monsterAI.charge = true;
        yield return new WaitForSeconds(1f);
        if (monsterAI.AttackTarget == null || monsterAI.AttackTarget.gameObject.activeInHierarchy == false)
        {
            monsterAI.charge = false;
            yield break;
        }
        var bulletObject = ObjectPool.Instance.GetGameObjectFromPool("IceBullet", monsterAI.tempFirePoint.position);
        var bullet = bulletObject.GetComponent<Bullet>();
        bullet.target = monsterAI.AttackTarget;
        bullet.direction = monsterAI.AttackTarget.position - monsterAI.transform.position;
        bullet.owner = monsterAI;
        
        yield return new WaitForSecondsRealtime(monsterAI.battleStat.attackInterval);
        monsterAI.attackEvent.AddListener(monsterAI.DefaultAttack);
        monsterAI.charge = false;
    }
}
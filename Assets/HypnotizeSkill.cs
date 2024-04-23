using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypnotizeSkill : MonoBehaviour
{
    const int ATTACK_TO_HYPNOTIZE = 3;

    [SerializeField] private float hypnotizedTime;
    [SerializeField] private Sprite sprHypnotoze;
    private MonsterAI monsterAi;

    private void Awake() {
        monsterAi = GetComponent<MonsterAI>();
    }

    public void HypnotizeTarget()
    {
        if(monsterAi.AttackTarget != null && monsterAi.AttackTarget.gameObject.activeInHierarchy)
        {
            monsterAi.AttackTarget.TryGetComponent<MonsterEffect>(out var targetEffect);
            targetEffect.Hypnotized(hypnotizedTime);
            ObjectPool.Instance.GetGameObjectFromPool("Vfx/N10Skill", targetEffect.transform.position);
            var skillEffect = ObjectPool.Instance.GetGameObjectFromPool<SkillEffect>("Vfx/SkillFx", transform.position);
            skillEffect.GetComponent<SpriteRenderer>().sprite = sprHypnotoze;
        }
    }
}

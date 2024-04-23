using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : MonoBehaviour
{
    [SerializeField] private float hpDecreaseFactor;
    private MonsterAI monsterAI;

    private void Start()
    {
        monsterAI = GetComponent<MonsterAI>();
    }
    public void DecreaseHpAttacker()
    {
        if (monsterAI.Attacker.TryGetComponent<MonsterAI>(out var attacker))
        {
            attacker.battleStat.hp -= monsterAI.battleStat.damage / hpDecreaseFactor;
        }
    }
}

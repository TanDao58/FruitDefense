using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E7Skill : MonoBehaviour
{
    private MonsterAI monsterAI;
    void Start()
    {
        monsterAI = GetComponent<MonsterAI>();
    }

    public void MultiHit()
    {
        for (int i = 0; i < 3; i++)
        {
            monsterAI.Attack();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer mp;
    private MonsterAI monster;
    // Start is called before the first frame update
    void Start()
    {
        mp = GetComponent<SpriteRenderer>();
        monster = GetComponentInParent<MonsterAI>();
    }

    // Update is called once per frame
    void Update()
    {
        var value = mp.transform.localScale;
        value.x = monster.ManaSKill;
        mp.transform.localScale = value;
    }
}

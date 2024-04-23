using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    C,R, SR, SSR
}
[System.Serializable]
public class MonsterData
{
    public Rarity rarity;
    public CurrencyType saleCurrency;
    public string monsterName;
    public string nickName;
    public string decripsion;
    public string demoSprite;
    public float level;
    public float maxLevel;

    public float maxhp = 200;
    public float damage = 20;
    public float baseDamge;
    public float baseHp;
    public Rarity baseRarity;

    public float attackRange = 5f;
    public float visionRange = 50f;
    public float attackInterval = 1f;
    public float speed = 2f;
    public float shopPrice = 400f;
    public float defend = 1f;

    public float displayHp;
    public float displayAttack;
    public float displayCooldown;
}

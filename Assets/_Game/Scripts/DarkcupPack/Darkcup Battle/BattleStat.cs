using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleStat 
{
    public string rank;
    public float speed = 4f;
    public float baseExp = 200;
    public float currentExp = 0;
    public float nextLvlExp = 0;
    public float monsterLevel;

    public float expGainWhenKill = 42f;
    public float goldGainWhenKill = 20f;
    public float foodGainWhenKill = 84f;
    public float foodConsumePerSec = 6f;
    //public float shopPrice = 400f;

    public float hp = 200;
    public float maxhp = 200;
    public float damage = 20;
    public float attackRange = 2f;
    public float visionRange = 20f;
    public float attackInterval = 1f;
    //public float defend = 1f;
    public int formationSortingOrder = 0;
    public float followRange =3;

    public BattleStat(MonsterData data, float level = 1f) {

            this.speed = data.speed;
            this.hp = data.maxhp * level;
            this.maxhp = data.maxhp * level;
            this.damage = data.damage * level;
            this.attackRange = data.attackRange;
            this.visionRange = data.visionRange;
            this.attackInterval = data.attackInterval;
       
        //this.defend = data.defend;
        //this.formationSortingOrder = data.formationSortingOrder;
        //this.goldGainWhenKill = data.goldGainWhenKill;
        //this.followRange = data.followRange;
    }

    public BattleStat(MonsterData data,string rank, float level)
    {
        int damageIncreaseAmount = 1;
        int hpIncreaseAmount = 1 ;
        if (rank == null) return;
        //switch (rank)
        //{
        //    case "C":
        //        damageIncreaseAmount = 10;
        //        hpIncreaseAmount = 10;
        //        break;
        //    case "R":
        //        damageIncreaseAmount = 20;
        //        hpIncreaseAmount = 10;
        //        break;
        //    case "S":
        //        damageIncreaseAmount = 30;
        //        hpIncreaseAmount = 10;
        //        break;
        //    case "SR":
        //        damageIncreaseAmount = 35;
        //        hpIncreaseAmount = 0;
        //        break;
        //    case "SSR":
        //        damageIncreaseAmount = 50;
        //        hpIncreaseAmount = 0;
        //        break;
        //}
        this.rank = rank;
        this.speed = data.speed;
        this.hp = data.maxhp + (level * hpIncreaseAmount);
        this.maxhp = data.maxhp + (level * hpIncreaseAmount);
        this.damage = data.damage + (level * damageIncreaseAmount);
        this.attackRange = data.attackRange;
        this.visionRange = data.visionRange;
        this.attackInterval = data.attackInterval;
    }
}

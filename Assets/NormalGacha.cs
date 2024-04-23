using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;

public class NormalGacha : Gacha
{
    private List<MonsterAI> monsterPrizes = new List<MonsterAI>();
    protected override void Start()
    {
        base.Start();
        var allMonster = DataManager.Instance.monsterAis;
        for (int i = 0; i < allMonster.Count; i++)
        {
            if (allMonster[i].name.StartsWith("E") && allMonster[i].monsterData.rarity == Rarity.SR)
            {
                monsterPrizes.Add(allMonster[i]);
            }
        }
    }
    public override void OpenGachaOne()
    {
        var prize = GetPrize();
        if(prize == PrizePool.Hero)
        {
            var random = Random.Range(0, monsterPrizes.Count);
            results.Add(new Result()
            {
                icon = monsterPrizes[random].portrait,
                amountText = "x 1",
                prizeId = "Hero" + monsterPrizes[random].monsterData.rarity.ToString()
            });
        }
        else
        {
            results.Add(new Result()
            {
                icon = Resources.Load<Sprite>("Item Icon/" + prize.ToString()),
                amountText = "x 1",
                prizeId = "Item"
            }) ;
        }
        Home.Instance.gachaProgress.ResgisterGacha(type);       
    }
    protected override PrizePool GetPrize()
    {
        var random = 10;
        if (random == 10)
        {
            return PrizePool.Hero;
        }
        return prizePool[Random.Range(0, prizePool.Count - 1)];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealButton : BuffButton
{
   public void HealAllAlly()
    {
        Amount--;
        CurrenCoolDown = coolDown;
        var allies = SelectManagerGameplay.Instance.spawnedHero;
        for (int i = 0; i < allies.Count; i++)
        {
            allies[i].Heal(allies[i].monsterData.maxhp);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WindButton : BuffButton
{
    public void PushEnemies()
    {
        Amount--;
        CurrenCoolDown = coolDown;
        var enemies = EnemySpawner.Instance.spawnedEnemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            var newPos = enemies[i].transform.position + new Vector3(0f, 5f);
            enemies[i].transform.DOMove(newPos, 1f);
        }
    }
}

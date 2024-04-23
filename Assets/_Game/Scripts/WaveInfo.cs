using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveInfoData
{
    public EnemyType enemyType;
    public int amount;

    public WaveInfoData()
    {

    }

    public WaveInfoData(EnemyType enemyType, int amount)
    {
        this.enemyType = enemyType;
        this.amount = amount;
    }
}

[System.Serializable]
public class WaveInfo
{
    public string[] enemyNames;
    public List<(int,int)> waveInfoDatas;
    public float restTime;
    public float minSpawnTime;
    public float maxSpawnTime;

    //public EnemyInWave enemyInWave;
    //public bool spawnUfo;
}

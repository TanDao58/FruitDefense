using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    [Header("Wave Info")]
    public List<WaveInfo> waveInfos;
    [Header("Boss Info")]
    public EnemyType boss;
    public MonsterData bossData;
    public float bossSizeFactor;
    [Header("Money Info")]
    public int startDP;
    public int increaseEachWaveDP;
    public int startMaxDP;
    public string levelName;
    public bool gotGem;
    public int levelIndex;

    public void CreateBoss(EnemyType enemyType, float bossSize, MonsterData bossData)
    {
        this.boss = enemyType;
        this.bossSizeFactor = bossSize;
        this.bossData = bossData;
    }
}
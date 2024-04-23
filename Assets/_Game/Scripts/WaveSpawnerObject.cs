using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Data/WaveData", order = 1)]
public class WaveSpawnerObject : ScriptableObject
{
    public List<LevelData> levelData;
}

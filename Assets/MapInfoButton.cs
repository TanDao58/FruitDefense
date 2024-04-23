using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;

[System.Serializable]
public class EnemyInfo
{
    public EnemyType enemyType;
    public bool isBoss;
}
public class MapInfoButton : MonoBehaviour
{
    [SerializeField]private GameObject InfoPopup;
    [SerializeField]private GameObject content;
    //private WaveSpawnerObject allData;
    public List<EnemyInfo> enemies = new List<EnemyInfo>();
    
    private void Start()
    {
        //allData = Resources.Load<WaveSpawnerObject>("WaveData");
        InfoPopup = transform.parent.gameObject.GetChildComponent<GameObject>("EnemiesInfoPopup");
        content = InfoPopup.GetChildComponent<GameObject>("Scroll View/Viewport/Content");
        InfoPopup.SetActive(false);
    }
    public void ShowInfo()
    {
        InfoPopup.SetActive(true);
        if (enemies.Count > 0) return;
        enemies = GetEnemiesInfo();
        for (int i = 0; i < enemies.Count; i++)
        {
            var enemyCard = DarkcupGames.ObjectPool.Instance.GetGameObjectFromPool<EnemyCard>("Button/EnemyCard", content.transform.position);
            enemyCard.transform.SetParent(content.transform);
            enemyCard.Dislay(enemies[i]);
            enemyCard.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }      
    }
    private List<EnemyInfo> GetEnemiesInfo()
    {
        var uiData = GetComponentInParent<UIUpdaterData>();
        var mapData = (MapInfoData)uiData.data;
        //var levelData = allData.levelData[mapData.level];
        var levelData = DataManager.Instance.levelDatas[mapData.level];
        var allEnemies = new List<EnemyInfo>();        
        var enemyList = new List<EnemyType>();
        for (int i = 0; i < levelData.waveInfos.Count; i++)
        {
            foreach (var enemy in levelData.waveInfos[i].waveInfoDatas)
            {
               if(!enemyList.Contains((EnemyType)enemy.Item1))
               {
                    enemyList.Add((EnemyType)enemy.Item1);
               }               
            }
        }
        foreach (var enemyType in enemyList)
        {
            allEnemies.Add(new EnemyInfo()
            {
                enemyType = enemyType,
                isBoss = false
            });
        }
        allEnemies.Add(new EnemyInfo()
        {
            enemyType = levelData.boss,
            isBoss = true
        });
        return allEnemies;
    }

    public void TurnOffPanel()
    {
        InfoPopup.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public List<LevelData> levelDatas = new List<LevelData>();
    public List<MonsterAI> monsterAis;
    public Dictionary<string, Sprite> portraits;
    public Dictionary<string, MonsterAI> dicMonsterAIs;

    public List<Sprite> testPortraits;
    public List<MonsterData> testMonsterDatas;
    public List<MapInfoData> mapDatas;
    public List<string> allyNames;
    public List<string> enemyNames;
    public List<string> saleHero;
    public List<string> unlockHero;
    public List<int> unlockLevels;
    private void Awake()
    {
        Instance = this;

        InitData();
        InitLevelData();
        GenerateDataByCode();
        if (Constants.TEST_MODE)
        {
            GameSystem.userdata.unlockedHeros = allyNames;
            GameSystem.userdata.maxLevel = 1000;
            GameSystem.SaveUserDataToLocal();
        }
        InitMapData();       
    }

    private void Start()
    {
        var newLevelReward = new Dictionary<string, bool>();
        var userLevelReward = GameSystem.userdata.levelReward;
        Debug.Log("Level Reward Count : " + newLevelReward.Count);
        if (userLevelReward == null || userLevelReward.Count == 0)
        {
            Debug.Log("Create");
            for (int i = 0; i < levelDatas.Count; i++)
            {
                newLevelReward.Add(levelDatas[i].levelName, false);
            }
            GameSystem.userdata.levelReward = newLevelReward;
            GameSystem.SaveUserDataToLocal();
            Debug.Log("Level Reward Count : " + newLevelReward.Count);
        }
        else
        {
            Debug.Log("Load");
            for (int i = 0; i < levelDatas.Count; i++)
            {
                if (userLevelReward.ContainsKey(levelDatas[i].levelName) == false)
                {
                    userLevelReward.Add(levelDatas[i].levelName, false);
                }
                levelDatas[i].gotGem = userLevelReward[levelDatas[i].levelName];
            }
        }
    }

    public void InitData()
    {
        portraits = new Dictionary<string, Sprite>();
        dicMonsterAIs = new Dictionary<string, MonsterAI>();

        testPortraits = new List<Sprite>();
        testMonsterDatas = new List<MonsterData>();
        allyNames = new List<string>() { "E0", "E2", "E3", "E4", "E5", "E8", "E9", "E10", "E12", "E15", "E17" , "E18" };
        enemyNames = new List<string>() { "N1", "N2", "N3", "N4", "N5", "N6", "N7", "N8", "N9", "N10", "N11", "N12" };
        unlockHero = new List<string>() { "E0", "E3", "E10", "E9", "E4" };
        unlockLevels = new List<int>() { 0, 1, 2, 3, 4 };
        saleHero = new List<string>();

        saleHero.AddRange(allyNames);

        for (int i = 0; i < unlockHero.Count; i++)
        {
            saleHero.Remove(unlockHero[i]);
        }

        if (saleHero.Contains("E18")) saleHero.Remove("E18");

        for (int i = 0; i < monsterAis.Count; i++)
        {
            string key = monsterAis[i].gameObject.name;
            var userData = GameSystem.userdata.unlockHeroDatas;
            if (userData.ContainsKey(key))
            {
                monsterAis[i].monsterData = userData[key];
            }
            dicMonsterAIs.Add(key, monsterAis[i]);
            portraits.Add(key, monsterAis[i].portrait);
            testMonsterDatas.Add(monsterAis[i].monsterData);
            testPortraits.Add(monsterAis[i].portrait);
        }
    }
    public void InitMapData()
    {
        mapDatas = new List<MapInfoData>();

        for (int i = 0; i < DataManager.Instance.levelDatas.Count; i++)
        {
            int backgroundId = (i / 5) % Constants.BACKGROUND_COUNT;
            mapDatas.Add(new MapInfoData()
            {
                mapName = "Level " + (i + 1),
                level = i + 1,
                backgroundId = backgroundId,
                demoSprite = Resources.Load<Sprite>("UpdateMap/" + "Map" + (backgroundId + 1)),
            });
        }
    }

    public void InitLevelData()
    {
        levelDatas = new List<LevelData>();
        LevelData level = null;


        // Level 1
        #region

        // Level 1
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 150;
        level.startMaxDP = 150;
        level.levelName = "Level 1";

        //level.waveInfos.Add(CreateWave(0.3f, 0.5f, new List<(int, int)>()
        //{
        //    ((int)EnemyType.N1, 30)
        //}));

        level.waveInfos.Add(CreateWave(1f, 5f, new List<(int, int)>()
        {
                   ((int)EnemyType.N1, 10),
                   //((int)EnemyType.N7, 3)
               }));

        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 1,
            speed = 0.5f
        });

        levelDatas.Add(level);

        #endregion

        // Level 2
        #region

        // Level 2
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 150;
        level.startMaxDP = 150;
        level.levelName = "Level 2";

        //level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
        //{
        //    ((int)EnemyType.N1, 3)
        //}));

        level.waveInfos.Add(CreateWave(1f, 5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 12),
        }));

        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 1,
            speed = 0.5f
        });

        levelDatas.Add(level);

        #endregion

        // Level 3
        #region

        // Level 3
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 150;
        level.startMaxDP = 150;
        level.levelName = "Level 3";

        //level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
        //{
        //    ((int)EnemyType.N1, 3)
        //}));

        level.waveInfos.Add(CreateWave(1f, 5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 12),
            ((int)EnemyType.N3, 1),
        }));

        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 1,
            speed = 0.5f
        });

        levelDatas.Add(level);

        #endregion

        // Level 4
        #region

        // Level 4
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 150;
        level.startMaxDP = 150;
        level.levelName = "Level 4";

        //level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
        //{
        //    ((int)EnemyType.N1, 3)
        //}));

        level.waveInfos.Add(CreateWave(1f, 5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 13),
            ((int)EnemyType.N3, 2),
        }));

        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 1,
            speed = 0.5f
        });

        levelDatas.Add(level);

        #endregion

        // Level 5
        #region

        // Level 5
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 150;
        level.startMaxDP = 150;
        level.levelName = "Level 5";

        //level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
        //{
        //    ((int)EnemyType.N1, 3)
        //}));

        level.waveInfos.Add(CreateWave(1f, 5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 3),
            ((int)EnemyType.N3, 2),
        }));

        level.waveInfos.Add(CreateWave(1f, 5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 15),
            ((int)EnemyType.N3, 5),
        }));

        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 1,
            speed = 0.5f
        });

        levelDatas.Add(level);

        #endregion

        // Level 6
        #region

        // Level 6
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 500;
        level.startMaxDP = 500;
        level.levelName = "Level 6";

        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N7, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 20),
            ((int)EnemyType.N7, 7)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 20),
            ((int)EnemyType.N7, 3)
        }));




        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 5000,
            damage = 100,
            attackInterval = 1,
            attackRange = 2,
            speed = 0.5f
        });

        levelDatas.Add(level);


        #endregion

        // Level 7
        #region

        // Level 7
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 1000;
        level.startMaxDP = 1000;
        level.levelName = "Level 7";

        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N7, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 25),
            ((int)EnemyType.N7, 10)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 20),
            ((int)EnemyType.N7, 10)
        }));

        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 10,
            speed = 0.5f
        });

        levelDatas.Add(level);

        #endregion

        // Level 8
        #region

        // Level 8
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 1000;
        level.startMaxDP = 1000;
        level.levelName = "Level 8";

        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N3, 1),
            ((int)EnemyType.N7, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 20),
            ((int)EnemyType.N3, 7),
            ((int)EnemyType.N7, 7)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 20),
            ((int)EnemyType.N3, 3),
            ((int)EnemyType.N7, 3)
        }));


        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 10,
            speed = 0.5f
        });

        levelDatas.Add(level);

        #endregion

        // Level 9
        #region

        // Level 9
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 1000;
        level.startMaxDP = 1000;
        level.levelName = "Level 9";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N3, 5),
            ((int)EnemyType.N7, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 1.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N7, 7),
            ((int)EnemyType.N3, 5)
        }));

        level.waveInfos.Add(CreateWave(0f, 0.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N7, 7),
            ((int)EnemyType.N3, 5),
        }));

        level.CreateBoss(EnemyType.N1, 2f, new MonsterData()
        {
            maxhp = 30000,
            damage = 700,
            attackInterval = 3,
            attackRange = 2,
            speed = 1f
        });
        levelDatas.Add(level);

        #endregion

        // Level 10
        #region

        // Level 10
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 5000;
        level.startMaxDP = 5000;
        level.levelName = "Level 10";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N5, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 1.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N5, 3)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N5, 10)
        }));


        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 10,
            speed = 0.5f
        });

        levelDatas.Add(level);


        #endregion

        // Level 11
        #region

        // Level 11
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 5000;
        level.startMaxDP = 5000;
        level.levelName = "Level 11";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N7, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 1.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N7, 5),
            ((int)EnemyType.N5, 3)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N5, 10),
            ((int)EnemyType.N7, 10)
        }));

        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 10,
            speed = 0.5f
        });

        levelDatas.Add(level);


        #endregion

        // Level 12
        #region

        // Level 12
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 5000;
        level.startMaxDP = 5000;
        level.levelName = "Level 12";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N3, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 1.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N3, 5),
            ((int)EnemyType.N5, 3)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N5, 10),
            ((int)EnemyType.N3, 10)
        }));

        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 10,
            speed = 0.5f
        });

        levelDatas.Add(level);


        #endregion

        // Level 13
        #region

        // Level 13
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 5000;
        level.startMaxDP = 5000;
        level.levelName = "Level 13";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N5, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 1.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N3, 5),
            ((int)EnemyType.N7, 5),
            ((int)EnemyType.N5, 3)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N5, 10),
            ((int)EnemyType.N3, 5),
            ((int)EnemyType.N7, 10)
        }));


        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 10,
            speed = 0.5f
        });

        levelDatas.Add(level);



        #endregion

        // Level 14
        #region

        // Level 14
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 5000;
        level.startMaxDP = 5000;
        level.levelName = "Level 14";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N5, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 1.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N3, 5),
            ((int)EnemyType.N7, 5),
            ((int)EnemyType.N5, 13)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N5, 10),
            ((int)EnemyType.N3, 5),
            ((int)EnemyType.N7, 10)
        }));

        level.CreateBoss(EnemyType.N4, 2f, new MonsterData()
        {
            maxhp = 30000,
            damage = 300,
            attackInterval = 1,
            attackRange = 1,
            speed = 0.25f
        });

        levelDatas.Add(level);

        #endregion

        // Level 15
        #region

        // Level 15
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 15";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N10, 1),
            ((int)EnemyType.N3, 2),
            ((int)EnemyType.N1, 10)
        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N1, 15),
            ((int)EnemyType.N10, 3),
            ((int)EnemyType.N8, 3)
        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N10, 5),
            ((int)EnemyType.N8, 5),
            ((int)EnemyType.N1, 25)
        }));

        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 10,
            speed = 0.5f
        });

        levelDatas.Add(level);


        #endregion

        // Level 16
        #region

        // Level 16
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 16";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N1, 15),
            ((int)EnemyType.N5, 5),
            ((int)EnemyType.N3, 5)
        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N10, 3),
            ((int)EnemyType.N5, 15),
            ((int)EnemyType.N8, 4)
        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N3, 10),
            ((int)EnemyType.N10, 1),
            ((int)EnemyType.N8, 10)
        }));

        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 10,
            speed = 0.5f
        });

        levelDatas.Add(level);


        #endregion

        // Level 17
        #region

        // Level 17
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 17";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N5, 10),
            ((int)EnemyType.N4, 2)
        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N8, 10),
            ((int)EnemyType.N4, 3)
        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N1, 20),
            ((int)EnemyType.N5, 15)
        }));

        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 10,
            speed = 0.5f
        });

        levelDatas.Add(level);


        #endregion

        // Level 18
        #region

        // Level 18
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 18";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N7, 15),
            ((int)EnemyType.N8, 3),
            ((int)EnemyType.N10, 3)
        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N10, 1),
            ((int)EnemyType.N7, 24),
            ((int)EnemyType.N8, 14)
        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N10, 10),
            ((int)EnemyType.N7, 3),
            ((int)EnemyType.N8, 3)
        }));

        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 10,
            speed = 0.5f
        });

        levelDatas.Add(level);

        #endregion

        // Level 19
        #region

        // Level 19
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 19";



        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N13, 3),
            ((int)EnemyType.N10, 1)
        }));

        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 15),
            ((int)EnemyType.N5, 10),
            ((int)EnemyType.N3, 5),
            ((int)EnemyType.N13, 5),
            ((int)EnemyType.N11, 3)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N5, 15),
            ((int)EnemyType.N6, 5),
            ((int)EnemyType.N13, 15),
            ((int)EnemyType.N10, 3),
            ((int)EnemyType.N11, 3)
        }));

        level.CreateBoss(EnemyType.N12, 2f, new MonsterData()
        {
            maxhp = 10000,
            damage = 500,
            attackInterval = 1,
            attackRange = 2,
            speed = 0.5f
        });

        levelDatas.Add(level);

        #endregion

        // Level 20
        #region
        // Level 20
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 20";

        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N8, 3),
            ((int)EnemyType.N7, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 20),
            ((int)EnemyType.N3, 7),
            ((int)EnemyType.N11, 1),
            ((int)EnemyType.N8, 7)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 20),
            ((int)EnemyType.N3, 10),
            ((int)EnemyType.N8, 7),
            ((int)EnemyType.N7, 3)
        }));


        levelDatas.Add(level);


        #endregion

        // Level 21
        #region

        // Level 21
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 21";

        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N3, 3),
            ((int)EnemyType.N8, 3),
            ((int)EnemyType.N7, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 20),
            ((int)EnemyType.N12, 1),
            ((int)EnemyType.N11, 5),
            ((int)EnemyType.N8, 7)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 30),
            ((int)EnemyType.N12, 2),
            ((int)EnemyType.N11, 5),
            ((int)EnemyType.N8, 7)
        }));

        levelDatas.Add(level);
        #endregion

        // Level 22
        #region

        // Level 22
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 22";

        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N13, 5),
            ((int)EnemyType.N8, 3),
            ((int)EnemyType.N7, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N13, 10),
            ((int)EnemyType.N8, 3),
            ((int)EnemyType.N5, 10),
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
        {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N13, 30),
            ((int)EnemyType.N8, 3),
            ((int)EnemyType.N5, 30)
        }));

        levelDatas.Add(level);
        #endregion

        // Level 23
        #region

        // Level 23
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 23";

        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N5, 10),
            ((int)EnemyType.N4, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N5, 20),
            ((int)EnemyType.N4, 5)
        }));

        level.waveInfos.Add(CreateWave(0f, 0.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 20),
            ((int)EnemyType.N4, 10),
            ((int)EnemyType.N5, 30)
        }));


        levelDatas.Add(level);
        #endregion

        // Level 24
        #region

        // Level 24
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 24";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N4, 3),
            ((int)EnemyType.N8, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 1.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N8, 7),
            ((int)EnemyType.N4, 5),
            ((int)EnemyType.N12, 1)
        }));

        level.waveInfos.Add(CreateWave(0f, 0.2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N8, 7),
            ((int)EnemyType.N13, 15),
            ((int)EnemyType.N12, 2)
        }));

        level.CreateBoss(EnemyType.N12, 2f, new MonsterData()
        {
            maxhp = 5000,
            damage = 500,
            attackInterval = 1,
            attackRange = 2,
            speed = 0.5f
        });

        levelDatas.Add(level);
        #endregion

        // Level 25
        #region

        // Level 25
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 25";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N8, 3),
            ((int)EnemyType.N13, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 1.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N8, 5),
            ((int)EnemyType.N13, 15)
        }));

        level.waveInfos.Add(CreateWave(0f, 0.2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 30),
            ((int)EnemyType.N8, 10),
            ((int)EnemyType.N4, 5),
            ((int)EnemyType.N13, 15)
        }));

        levelDatas.Add(level);
        #endregion

        // Level 26
        #region

        // Level 26
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 26";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N5,10),
            ((int)EnemyType.N13, 5)
        }));

        level.waveInfos.Add(CreateWave(1f, 1.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N5, 20),
            ((int)EnemyType.N13, 10),
            ((int)EnemyType.N11, 5)
        }));


        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N5, 30),
            ((int)EnemyType.N13, 30),
            ((int)EnemyType.N11, 10)
        }));

        levelDatas.Add(level);
        #endregion

        // Level 27
        #region

        // Level 27
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 27";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N13, 10),
            ((int)EnemyType.N5, 10),
            ((int)EnemyType.N8, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 1.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N13, 10),
            ((int)EnemyType.N5, 20),
            ((int)EnemyType.N8, 10),
            ((int)EnemyType.N11, 5)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N13, 30),
            ((int)EnemyType.N5, 20),
            ((int)EnemyType.N8, 10),
            ((int)EnemyType.N4, 15)
        }));

        levelDatas.Add(level);
        #endregion

        // Level 29
        #region

        // Level 29
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 29";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N13, 10),
            ((int)EnemyType.N10, 1),
            ((int)EnemyType.N8, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 1.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N13, 10),
            ((int)EnemyType.N10, 3),
            ((int)EnemyType.N8, 10),
            ((int)EnemyType.N11, 5)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N13, 30),
            ((int)EnemyType.N10, 5),
            ((int)EnemyType.N8, 10),
            ((int)EnemyType.N4, 15)
        }));

        levelDatas.Add(level);
        #endregion

        // Level 30
        #region

        // Level 30
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 30";

        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N5, 10),
            ((int)EnemyType.N13, 3),
            ((int)EnemyType.N10, 1)
        }));

        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 15),
            ((int)EnemyType.N8, 10),
            ((int)EnemyType.N4, 5),
            ((int)EnemyType.N13, 5),
            ((int)EnemyType.N11, 3)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 15),
            ((int)EnemyType.N8, 5),
            ((int)EnemyType.N13, 15),
            ((int)EnemyType.N12, 3),
            ((int)EnemyType.N11, 3)
        }));

        level.CreateBoss(EnemyType.N12, 1f, new MonsterData()
        {
            maxhp = 30000,
            damage = 500,
            attackInterval = 1,
            attackRange = 2,
            speed = 0.5f
        });

        levelDatas.Add(level);
        #endregion

        // Level 31
        #region

        // Level 31
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 31";

        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N5, 10),
            ((int)EnemyType.N6, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 20),
            ((int)EnemyType.N10, 7),
            ((int)EnemyType.N11, 5),
            ((int)EnemyType.N5, 10)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 30),
            ((int)EnemyType.N10, 10),
            ((int)EnemyType.N6, 5),
            ((int)EnemyType.N5, 10)
        }));


        levelDatas.Add(level);
        #endregion

        // Level 32
        #region

        // Level 32
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 32";

        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N6, 3),
            ((int)EnemyType.N4, 3),
            ((int)EnemyType.N5, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N5, 20),
            ((int)EnemyType.N6, 3),
            ((int)EnemyType.N10, 3),
            ((int)EnemyType.N11, 3),
            ((int)EnemyType.N4, 3)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
            ((int) EnemyType.N5, 20),
            ((int) EnemyType.N6, 3),
            ((int) EnemyType.N10, 3),
            ((int) EnemyType.N7, 10),
            ((int) EnemyType.N4, 10)
        }));

        levelDatas.Add(level);
        #endregion

        // Level 33
        #region

        // Level 33
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 33";

        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N5, 10),
            ((int)EnemyType.N6, 3),
            ((int)EnemyType.N4, 3),
            ((int)EnemyType.N8, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N5, 20),
            ((int)EnemyType.N6, 3),
            ((int)EnemyType.N8, 10),
            ((int)EnemyType.N11, 3),
            ((int)EnemyType.N4, 3)
        }));

        level.waveInfos.Add(CreateWave(0f, 1f, new List<(int, int)>()
 {
             ((int)EnemyType.N5, 20),
            ((int)EnemyType.N6, 10),
            ((int)EnemyType.N8, 10),
            ((int)EnemyType.N11, 10),
            ((int)EnemyType.N4, 10)
        }));

        levelDatas.Add(level);
        #endregion

        // Level 34
        #region

        // Level 34
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 34";

        level.waveInfos.Add(CreateWave(1f, 3f, new List<(int, int)>()
 {
            ((int)EnemyType.N5, 10),
            ((int)EnemyType.N10, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N5, 20),
            ((int)EnemyType.N10, 3),
            ((int)EnemyType.N8, 3)
        }));

        level.waveInfos.Add(CreateWave(0f, 0.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N5, 20),
            ((int)EnemyType.N4, 30),
            ((int)EnemyType.N10, 5)
        }));

        levelDatas.Add(level);
        #endregion

        // Level 35
        #region

        // Level 35
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 10000;
        level.startMaxDP = 10000;
        level.levelName = "Level 35";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
 {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N5, 3)
        }));

        level.waveInfos.Add(CreateWave(1f, 1.5f, new List<(int, int)>()
 {
            ((int)EnemyType.N5, 15),
            ((int)EnemyType.N4, 10)
        }));

        level.waveInfos.Add(CreateWave(0f, 0.2f, new List<(int, int)>()
 {
            ((int)EnemyType.N4, 30),
            ((int)EnemyType.N5, 25),
            ((int)EnemyType.N1, 30)
        }));

        level.CreateBoss(EnemyType.N10, 2f, new MonsterData()
        {
            maxhp = 35000,
            damage = 100,
            attackInterval = 0.5f,
            attackRange = 10,
            speed = 0.5f
        });

        levelDatas.Add(level);
        #endregion

        // Level 36
        #region

        // Level 36
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 15000;
        level.startMaxDP = 15000;
        level.levelName = "Level 36";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N1, 10),
            ((int)EnemyType.N2, 1)

        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N1, 25),
            ((int)EnemyType.N2, 5),
            ((int)EnemyType.N11, 3)
        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N11, 5),
            ((int)EnemyType.N2, 10),
            ((int)EnemyType.N4, 2)
        }));


        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 10,
            speed = 0.5f
        });

        levelDatas.Add(level);

        #endregion

        // Level 37
        #region

        // Level 37
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 15000;
        level.startMaxDP = 15000;
        level.levelName = "Level 37";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N9, 1),
            ((int)EnemyType.N5, 15),
            ((int)EnemyType.N3, 3)
        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N11, 3),
            ((int)EnemyType.N9, 5),
            ((int)EnemyType.N4, 2)
        }));
        level.waveInfos.Add(CreateWave(0f, 0.2f, new List<(int, int)>()
        {
            ((int)EnemyType.N9, 10),
            ((int)EnemyType.N7, 10),
            ((int)EnemyType.N4, 10)
        }));

        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 10,
            speed = 0.5f
        });

        levelDatas.Add(level);


        #endregion

        // Level 38
        #region

        // Level 38
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 15000;
        level.startMaxDP = 15000;
        level.levelName = "Level 38";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N2, 1),
            ((int)EnemyType.N10, 1),
            ((int)EnemyType.N9, 10)
        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N3, 10),
            ((int)EnemyType.N9, 10),
            ((int)EnemyType.N2, 5)
        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N2, 3),
            ((int)EnemyType.N10, 3),
            ((int)EnemyType.N9, 15)
        }));

        level.CreateBoss(EnemyType.None, 2f, new MonsterData()
        {
            maxhp = 1500,
            damage = 100,
            attackInterval = 1,
            attackRange = 10,
            speed = 0.5f
        });

        levelDatas.Add(level);

        #endregion

        // Level 39
        #region

        // Level 39
        level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        level.startDP = 15000;
        level.startMaxDP = 15000;
        level.levelName = "Level 39";

        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N5, 30),
            ((int)EnemyType.N9, 10),
            ((int)EnemyType.N4, 2)
        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N10, 3),
            ((int)EnemyType.N9, 30)
        }));
        level.waveInfos.Add(CreateWave(1f, 2f, new List<(int, int)>()
        {
            ((int)EnemyType.N10, 3),
            ((int)EnemyType.N9, 25)
        }));

        level.CreateBoss(EnemyType.N2, 2f, new MonsterData()
        {
            maxhp = 35500,
            damage = 150,
            attackInterval = 1,
            attackRange = 5,
            speed = 0.05f
        });

        levelDatas.Add(level);


        #endregion

        for (int i = 0; i < levelDatas.Count; i++)
        {
            levelDatas[i].levelName = "Level " + (i + 1);
            levelDatas[i].startDP = Constants.DP_START;
            levelDatas[i].startMaxDP = Constants.DP_MAX_INGAME;
        }
    }

    public WaveInfo CreateWave(float minSpawn, float maxSpawn, List<(int, int)> waveDatas)
    {
        WaveInfo wave = new WaveInfo();
        wave.waveInfoDatas = waveDatas;
        wave.minSpawnTime = minSpawn;
        wave.maxSpawnTime = maxSpawn;
        return wave;
    }

    List<EnemyType> enemys = new List<EnemyType>() { EnemyType.N1, EnemyType.N2, EnemyType.N3, EnemyType.N4, EnemyType.N5, EnemyType.N7, EnemyType.N8, EnemyType.N9, EnemyType.N10, EnemyType.N11 };
    List<EnemyType> bossEnemies = new List<EnemyType>() { EnemyType.N1, EnemyType.N2, EnemyType.N3, EnemyType.N4, EnemyType.N5, EnemyType.N7, EnemyType.N8, EnemyType.N9, EnemyType.N10 };

    public List<LevelData> GenerateDataByCode()
    {
        List<LevelData> levels = new List<LevelData>();

        for (int i = 0; i < 1000; i++)
        {
            LevelData generated = GenerateOneLevel();
            generated.levelName = "Level " + (i + 1);
            levels.Add(generated);
        }

        return levels;
    }

    LevelData GenerateOneLevel()
    {
        LevelData level = new LevelData();
        level.waveInfos = new List<WaveInfo>();
        for (int i = 0; i < 4; i++)
        {
            List<EnemyType> randomEnemies = enemys.RandomElement(Random.Range(2, 3));
            WaveInfo wave = new WaveInfo();
            wave.waveInfoDatas = new List<(int, int)>();

            int amount = (Random.Range(5, 10) + 15 * i) / randomEnemies.Count;
            Dictionary<EnemyType, int> dicMaxAmounts = new Dictionary<EnemyType, int>();
            dicMaxAmounts.Add(EnemyType.N10, 7);
            dicMaxAmounts.Add(EnemyType.N11, 2);

            for (int j = 0; j < randomEnemies.Count; j++)
            {
                int id = (int)randomEnemies[j];
                int number = (int)(amount * Random.Range(1f, 1.5f));
                if (dicMaxAmounts.ContainsKey(randomEnemies[j]))
                {
                    if (number > dicMaxAmounts[randomEnemies[j]])
                    {
                        number = dicMaxAmounts[randomEnemies[j]];
                    }
                }
                wave.waveInfoDatas.Add((id, number));
                float time = Random.Range(0.5f, 1f);
                wave.minSpawnTime = time / 2;
                wave.maxSpawnTime = time;
            }
            level.waveInfos.Add(wave);
        }

        level.CreateBoss(bossEnemies.RandomElement(), 2f, new MonsterData()
        {
            maxhp = 10000,
            damage = 100,
            attackInterval = 0.05f,
            attackRange = 2,
            speed = 0.5f
        });
        level.startDP = Constants.DP_START;
        level.startMaxDP = Constants.DP_MAX_INGAME;

        return level;
    }
}

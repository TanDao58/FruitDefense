using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DarkcupGames;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [System.NonSerialized] public List<UfoSkill> ufos = new List<UfoSkill>();
    public List<MonsterAI> spawnedEnemies = new List<MonsterAI>();
    protected TextMeshProUGUI waveNumber;
    public List<EnemyType> spawnDatas;
    public List<float> spawnDelays;
    public TextMeshProUGUI txtLevelName;
    public TextMeshProUGUI txtEnemyCount;
    public PopupUnlockHero popupNewEnemy;
    public Slider sliderEnemyAmount;
    public bool testMode;
    public int currentWave = 0;
    public int index;

    private GameObject currentUFO;
    [System.NonSerialized] public LevelData levelData;
    private float nextSpawnEnemy;
    private bool win = false;
    private bool spawnBoss = false;
    private int totalEnemyInWave;
    private Vector3 defaultPopUpPosition;
    [SerializeField] private Transform popupShowPos;
    [SerializeField] private RectTransform bossWarningImg;
    [SerializeField] private AudioClip warningSound;
    [SerializeField] private AudioClip bossBGM;

    private void Awake()
    {
        Instance = this;
        if (GameSystem.userdata == null)
        {
            GameSystem.LoadUserData();
        }
        waveNumber = Gameplay.Intansce.canvas.GetChildComponent<TextMeshProUGUI>("WaveNumer");
    }

    public virtual void Start()
    {
        int playerLevel = testMode ? Constants.TEMP_PLAYER_LEVEL : GameSystem.userdata.currentLevel;
        levelData = DataManager.Instance.levelDatas[playerLevel];
        txtLevelName.text = levelData.levelName;
        popupNewEnemy.gameObject.SetActive(false);
        currentWave = 0;
        defaultPopUpPosition = popupNewEnemy.transform.position;
        bossWarningImg.gameObject.SetActive(false);
        StartSpawnWave();
    }

    void StartSpawnWave()
    {
        index = 0;
        waveNumber.text = "Wave " + (currentWave + 1).ToString();
        spawnedEnemies = new List<MonsterAI>();
        spawnDatas = GenerateListEnemies(levelData.waveInfos[currentWave]);
        spawnDelays = new List<float>();
        for (int i = 0; i < spawnDatas.Count; i++)
        {
            spawnDelays.Add(Random.Range(levelData.waveInfos[currentWave].minSpawnTime, levelData.waveInfos[currentWave].maxSpawnTime));
        }
        totalEnemyInWave = spawnDatas.Count;
        if (levelData.boss != EnemyType.None)
        {
            totalEnemyInWave += 1;
        }
    }

    public MonsterAI SpawnUFO()
    {
        if (currentUFO != null && currentUFO.activeInHierarchy) return null;

        var monstersInScreence = FindObjectsOfType<MonsterAI>();
        List<MonsterAI> allies = new List<MonsterAI>();
        foreach (var monster in monstersInScreence)
        {
            if (!monster.IsEnemy)
            {
                allies.Add(monster);
            }
        }
        if (allies.Count == 0) return null;
        var random = Random.Range(0, allies.Count);
        var ufo = ObjectPool.Instance.GetGameObjectFromPool<MonsterAI>("N11", new Vector3(Random.Range(-4f, 4f), Random.Range(8f, 12f)));
        ufo.enabled = true;
        ufo.IsEnemy = true;

        var ufoSkill = ufo.GetComponent<UfoSkill>();
        ufoSkill.enabled = true;
        ufoSkill.ufoAI.AttackTarget = allies[random].transform;
        if (!ufos.Contains(ufoSkill))
        {
            ufos.Add(ufoSkill);
        }
        return ufo;
    }

    public List<EnemyType> GenerateListEnemies(WaveInfo waveInfo)
    {
        List<EnemyType> enemies = new List<EnemyType>();

        for (int i = 0; i < waveInfo.waveInfoDatas.Count; i++)
        {
            int type = waveInfo.waveInfoDatas[i].Item1;
            int amount = waveInfo.waveInfoDatas[i].Item2;

            for (int j = 0; j < amount; j++)
            {
                enemies.Add((EnemyType)type);
            }
        }
        enemies.Shuffle();
        return enemies;
    }

    public void SpawnEnemy(EnemyType type)
    {
        if(type != EnemyType.None) CheckSeenEnemy(type.ToString());
        if (type.ToString() == "N11")
        {
            MonsterAI ufo = SpawnUFO();
            if (ufo != null)
            {
                currentUFO = ufo.gameObject;
                ufo.battleStat = new BattleStat(ufo.monsterData, 1);
            }
        }
        else
        {
            Vector3 pos = new Vector3(Random.Range(-4f, 4f), Random.Range(8f, 12f));
            var monster = ObjectPool.Instance.GetGameObjectFromPool<MonsterAI>(type.ToString(), pos);
            monster.battleStat = new BattleStat(monster.monsterData, 1);
            monster.IsEnemy = true;
            monster.isBoss = false;
            monster.enabled = true;
            EasyEffect.Appear(monster.gameObject, 0f, 1f);
            spawnedEnemies.Add(monster);
        }
    }

    public void SpawnBoss(LevelData levelData)
    {
        DoBossWaring();
        Vector3 pos = new Vector3(Random.Range(-4f, 4f), Random.Range(8f, 12f));
        var boss = ObjectPool.Instance.GetGameObjectFromPool<MonsterAI>(levelData.boss.ToString(), pos);
        boss.battleStat = new BattleStat(levelData.bossData);
        boss.IsEnemy = true;
        boss.isBoss = true;
        boss.enabled = true;
        boss.battleStat.speed = levelData.bossData.speed;
        EasyEffect.Appear(boss.gameObject, 0f, levelData.bossSizeFactor);
        spawnedEnemies.Add(boss);
    }

    void CheckSeenEnemy(string enemyName)
    {
        if (GameSystem.userdata.seenEnemies == null)
        {
            GameSystem.userdata.seenEnemies = new List<string>();
        }
        if (GameSystem.userdata.seenEnemies.Contains(enemyName) == false)
        {
            GameSystem.userdata.seenEnemies.Add(enemyName);
            GameSystem.SaveUserDataToLocal();
            popupNewEnemy.gameObject.SetActive(true);
            popupNewEnemy.ShowUnlockHero(enemyName);
            defaultPopUpPosition.y = popupShowPos.position.y;
            popupNewEnemy.transform.position = defaultPopUpPosition;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(popupNewEnemy.transform.DOMove(popupShowPos.position, 1f));
            sequence.AppendInterval(3f);
            sequence.Append(popupNewEnemy.transform.DOMove(defaultPopUpPosition, 1f));
        }
    }

    protected bool CheckAllEnemyDie()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i].IsDead() && !spawnedEnemies[i].gameObject.activeInHierarchy)
            {
                spawnedEnemies.Remove(spawnedEnemies[i]);
            }
        }
        int spawnRemain = spawnDatas.Count - index;
        int enemyRemain = spawnRemain + spawnedEnemies.Count;
        txtEnemyCount.text = enemyRemain.ToString();
        float wavePercent = (float)(spawnDatas.Count - enemyRemain) / spawnDatas.Count;
        sliderEnemyAmount.value = ((float)currentWave) / levelData.waveInfos.Count + wavePercent / levelData.waveInfos.Count;
        if (spawnedEnemies.Count <= 0 && index >= spawnDatas.Count)
        {
            currentWave++;
            if (currentWave == levelData.waveInfos.Count)
            {
                StartCoroutine(Gameplay.Intansce.IEShowWinPopup());
                win = true;
            }
            else
            {
                nextSpawnEnemy = Time.time + levelData.waveInfos[currentWave].restTime;
                StartSpawnWave();
            }

            return true;
        }
        return false;
    }

    float nextCheckFinish;

    private void Update()
    {
        if (win) return;

        if (Time.time > nextSpawnEnemy && index < spawnDatas.Count && GameSystem.userdata.doneDragTutorial)
        {
            SpawnEnemy(spawnDatas[index]);
            WaveInfo wave = levelData.waveInfos[currentWave];
            nextSpawnEnemy = Time.time + spawnDelays[index];
            index++;

            if (spawnBoss == false && currentWave == levelData.waveInfos.Count - 1 && levelData.boss != EnemyType.None)
            {
                SpawnBoss(levelData);
                spawnBoss = true;
            }
        }

        if (Time.time > nextCheckFinish)
        {
            CheckAllEnemyDie();
            nextCheckFinish = Time.time + 1f;
        }
    }

    public void CloseNewEnemyPopup()
    {
        EasyEffect.Disappear(popupNewEnemy.gameObject, 1f, 0.8f);
        Time.timeScale = Constants.DEFAULT_TIME_SCALE;
    }

    private void DoBossWaring()
    {       
        var bgm = Gameplay.Intansce.GetComponent<AudioSource>();
        var warningImg = bossWarningImg.GetComponentInChildren<Image>();
        LeanTween.value(1f, 0f, 2f).setOnUpdate((volume) => bgm.volume = volume).setOnComplete(() =>
        {
            bossWarningImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            bossWarningImg.gameObject.SetActive(true);
            SFXSystem.Instance.Play(warningSound);
            LeanTween.value(0f, 1000f, 0.5f).setOnUpdate((value) => bossWarningImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value)).setOnComplete(() =>
            {               
                Sequence sequence = DOTween.Sequence();
                sequence.Append(warningImg.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    Gameplay.Intansce.DoHpLostEfect();
                    warningImg.DOFade(1f, 0.5f);
                }));
                sequence.AppendInterval(0.5f);
                sequence.Append(warningImg.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    Gameplay.Intansce.DoHpLostEfect();
                    warningImg.DOFade(1f, 0.5f);
                }));
                sequence.AppendInterval(0.5f);
                sequence.Append(warningImg.DOFade(0f, 0.5f).OnComplete(() =>
               warningImg.DOFade(1f, 0.5f)).OnComplete(() =>
               {
                   Gameplay.Intansce.DoHpLostEfect();
                   warningImg.DOFade(0f, 0.5f);
                   bgm.clip = bossBGM;
                   LeanTween.value(0f, 1f, 0.5f).setOnUpdate((volume) =>
                   {
                       bgm.volume = volume;
                       bgm.Play();
                   });
               })).OnComplete(()=>bossWarningImg.gameObject.SetActive(false));
            });
        });       
    }
}
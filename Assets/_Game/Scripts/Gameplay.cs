using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DarkcupGames;
using System.Linq;
using DG.Tweening;
using UnityEngine.Events;

//class SpawnData {
//    public float time;
//    public string code;
//}
public enum GameplayState
{
    Win, Lose, Pause, Playing
}

public class Gameplay : MonoBehaviour
{
    float FALL_TIME = 0.2f;
    float FALL_HEIGHT = 2f;
    public static Gameplay Intansce;
    public static bool isRestart;

    public GameObject canvas;

    [System.NonSerialized] public Vector2 vurnerableZone;
    [System.NonSerialized] public Vector2 dragLimitZone;
    [System.NonSerialized] public GameObject shield;
    [SerializeField] private GameObject reference;
    [SerializeField] private GameObject dragReference;
    [SerializeField] private MonsterAI defaultMonsterAI;
    [SerializeField] private Image imgSpeedUp;
    [SerializeField] PopupWinLose winPopup;
    [SerializeField] PopupWinLose losePopup;
    [SerializeField] GameObject pausePopup;
    [SerializeField] PopupUnlockHero unlockHeroPopup;
    [SerializeField] private TextMeshProUGUI countdownText;
    //[SerializeField] PopupUnlockHero tryHeroPopup;
    public List<GameObject> dangerZones;
    public GameObject blueZones;
    private TextMeshProUGUI levelText;
    private GameObject bottomGroup;
    private TextMeshProUGUI currentLifePointTxt;
    public TextMeshProUGUI heroAmountDisplay;
    public TextMeshProUGUI enemyAliveDisplay;
    private DeloyUpdate moneyUpdate;
    private Image loseHpEffect;
    private GameplayState gameState;
    private int maxLifePoint;
    private int lifePoint;
    private Camera mainCam;
    private bool speedUp = false;
    [SerializeField] private int enemyAmount;
    private float startLevelTime;
    public AudioClip takeDameSound;
    public GameObject fadeBlack;
    public FireworkManager firework;

    private int reviveTime = 1;
    private bool startCountDown = false;
    [SerializeField] private float countDownTime = 5f;
    [SerializeField] private float lastUpdateTime;

    public int EnemyAmount
    {
        get { return enemyAmount; }
        set
        {
            enemyAmount = value;
        }
    }

    public int EnemyTotalAmount
    {
        get { return CountEnemyAmount(GameSystem.userdata.currentLevel); }
    }

    public int LifePoint
    {
        get { return lifePoint; }
        set
        {
            if (value < 0) value = 0;
            lifePoint = value;
            UpdateDislayLifePoint(currentLifePointTxt, value.ToString());
            if (lifePoint == 0)
            {
                if (Constants.CHEAT_NO_DIE) return;
                gameState = GameplayState.Lose;
                if (reviveTime >= 1)
                {
                    reviveTime--;
                    startCountDown = true;
                    lastUpdateTime = (int)countDownTime;
                    countdownText.transform.parent.gameObject.SetActive(true);
                }
                else
                {
                    losePopup.ShowPopup(false);
                }
                //TryShowTrialPopup();
                //AdManager.Instance.ShowIntertistialAds();
                FirebaseManager.Instance.LogLevelFail(GameSystem.userdata.currentLevel, Time.time - startLevelTime);
            }
        }
    }
    public GameplayState GameState => gameState;
    public DeloyUpdate MoneyUpdate => moneyUpdate;

    private void Awake()
    {
        Intansce = this;
        fadeBlack.gameObject.SetActive(true);
        maxLifePoint = 5;
        gameState = GameplayState.Playing;
        mainCam = Camera.main;
        vurnerableZone = mainCam.ScreenToWorldPoint(reference.transform.position);
        dragLimitZone = mainCam.ScreenToWorldPoint(dragReference.transform.position);
        dangerZones[0].transform.position = vurnerableZone;
        dangerZones[0].SetActive(false);
        dangerZones[1].SetActive(false);
        blueZones.SetActive(false);
        Destroy(reference);
        Destroy(dragReference);
        if (GoogleAdMobController.Instance.bannerView != null)
        {
            GoogleAdMobController.Instance.bannerView.Hide();
        }
    }

    private void Start()
    {
        moneyUpdate = canvas.gameObject.GetChildComponent<DeloyUpdate>("DeployPoint");

        winPopup.gameObject.SetActive(false);
        losePopup.gameObject.SetActive(false);
        pausePopup.SetActive(false);
        firework.gameObject.SetActive(false);
        unlockHeroPopup.gameObject.SetActive(false);
        bottomGroup = canvas.GetChildComponent<GameObject>("BottomGroup");
        shield = canvas.GetChildComponent<GameObject>("LifePoint");
        currentLifePointTxt = shield.GetChildComponent<TextMeshProUGUI>("CurrentLifePoint");
        levelText = canvas.GetChildComponent<TextMeshProUGUI>("Level");
        heroAmountDisplay = canvas.GetChildComponent<TextMeshProUGUI>("HeroGroup/AmountTxt");
        enemyAliveDisplay = canvas.GetChildComponent<TextMeshProUGUI>("MonsterGroup/AmountTxt");
        loseHpEffect = canvas.GetChildComponent<Image>("LoseHp");
        loseHpEffect.gameObject.SetActive(false);
        UpdateDislayLifePoint(currentLifePointTxt, maxLifePoint.ToString());
        LifePoint = maxLifePoint;
        SetupChosenHero();
        ShowLevel();
        EnemyAmount = EnemyTotalAmount;
        Home.homeSceneAction = HomeSceneAction.None;
        FirebaseManager.Instance.LogLevelStart(GameSystem.userdata.currentLevel, isRestart);
        isRestart = false;
        startLevelTime = Time.time;
        if (!GameSystem.userdata.doneDragTutorial)
        {
            EnemySpawner.Instance.gameObject.SetActive(false);
            TutorialManager.Instance.DoDragTutorial();
        }
        fadeBlack.gameObject.SetActive(false);
        countdownText.transform.parent.gameObject.SetActive(false);
        speedUp = false;
        UpdateSpeed();
    }

    private void Update()
    {
        if (!startCountDown) return;
        countDownTime -= Time.unscaledDeltaTime;
        if (lastUpdateTime != (int)countDownTime)
        {
            lastUpdateTime = (int)countDownTime;
            countdownText.text = lastUpdateTime.ToString();
            if(countDownTime <= 0)
            {
                countdownText.transform.parent.gameObject.SetActive(false);
                losePopup.ShowPopup(false);
                startCountDown = false;
            }
        }
    }

    private void SetupChosenHero()
    {
        GameSystem.userdata.unlockedHeros.Distinct();
        if(!GameSystem.userdata.doneDragTutorial)
        {
            GameSystem.chosenMonsters.Clear();
            GameSystem.chosenMonsters.Add("E0");
        }
        else if (GameSystem.userdata.unlockedHeros.Count <= Constants.MAX_HERO && GameSystem.chosenMonsters.Count < GameSystem.userdata.unlockedHeros.Count)
        {
            GameSystem.chosenMonsters = GameSystem.userdata.unlockedHeros;
        }
        var names = GameSystem.chosenMonsters;
        for (int i = 0; i < names.Count; i++)
        {
            var dragButton = ObjectPool.Instance.GetGameObjectFromPool<DragButton>("Button/dragButton", transform.position);
            if (DataManager.Instance.dicMonsterAIs.ContainsKey(names[i]) == false)
            {
                Debug.LogError(GeneralUltility.BuildString(" ","Error: not found key ", names[i], " in datamanager!"));
                return;
            }
            var monsterAI = DataManager.Instance.dicMonsterAIs[names[i]];
            dragButton.DislayHero(monsterAI, names[i]);
            dragButton.transform.SetParent(bottomGroup.transform);
            dragButton.transform.localScale = new Vector3(1f, 1f);

            //reduce lagging for first spawn
            var preloaded = ObjectPool.Instance.GetGameObjectFromPool<MonsterAI>(names[i], Vector3.zero * 100f);
            preloaded.gameObject.SetActive(false);
        }
        SelectManagerGameplay.Instance.UpdateDragButtons();
    }

    //private void SetupDefaultHero()
    //{
    //    for (int i = 0; i < 5; i++)
    //    {
    //        var dragButton = ObjectPool.Instance.GetGameObjectFromPool<DragButton>("Button/dragButton", transform.position);
    //        dragButton.DislayHero(defaultMonsterAI, "E2");
    //        dragButton.transform.SetParent(bottomGroup.transform);
    //        dragButton.transform.localScale = new Vector3(1f, 1f);
    //    }
    //    SelectManagerGameplay.Instance.UpdateDragButtons();
    //}

    public IEnumerator IESpawnManyEnemy()
    {
        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                string prefabName = "E1";
                Vector3 pos = new Vector3(Random.Range(-2f, 2f), 6f);
                GameObject obj = ObjectPool.Instance.GetGameObjectFromPool(prefabName, pos);
                obj.GetComponent<MonsterAI>().IsEnemy = true;
                obj.GetChildComponent<SpriteRenderer>("hpBox/hpBar").color = Color.red;
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(5f);
        }
    }

    public IEnumerator IEShowWinPopup()
    {
        if (firework.gameObject.activeInHierarchy == true) yield break;
        firework.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(Constants.FIREWORK_TIME_BEFORE_ADS);

        //AdManager.Instance.ShowIntertistialAds();
        FirebaseManager.Instance.LogLevelPassed(GameSystem.userdata.currentLevel, Time.time - startLevelTime);
        GameSystem.userdata.currentLevel++;
        if (GameSystem.userdata.currentLevel > GameSystem.userdata.maxLevel)
        {
            GameSystem.userdata.maxLevel = GameSystem.userdata.currentLevel;
        }
        GameSystem.SaveUserDataToLocal();

        string unlockName = PopupWinLose.TryUnlockHero();
        if (unlockName != "")
        {
            unlockHeroPopup.ShowUnlockHero(unlockName);
        } else
        {
            winPopup.ShowPopup();
        }
    }

    public void CloseUnlockPopup(GameObject nextPopup)
    {
        unlockHeroPopup.gameObject.SetActive(false);
        nextPopup.gameObject.SetActive(true);
        if( LifePoint > 0 && nextPopup.TryGetComponent<PopupWinLose>(out var winPopup))
        {
            winPopup.ShowPopup();
        }
    }

    //public void TryShowTrialPopup()
    //{
    //    if (Constants.CHEAT_NO_DIE) return;
    //    if (reviveTime > 0)
    //    {
    //        reviveTime--;
    //        ShowTrial();
    //    }
    //    else
    //    {
    //        losePopup.SetActive(true);
    //    }
    //    //if (tryHeroPopup.gameObject.activeInHierarchy)
    //    //{
    //    //    tryHeroPopup.gameObject.SetActive(false);
    //    //}
    //    //losePopup.GetComponent<UIEffect>().DoEffect();
    //}

    public void RePlay()
    {
        isRestart = true;
        Home.homeSceneAction = HomeSceneAction.OpenHeroSelect;
        SceneManager.LoadScene(Constants.SCENE_HOME);
    }

    private void UpdateDislayLifePoint(TextMeshProUGUI lifePointText, string value)
    {
        lifePointText.text = value;
    }
    public void GainDP(float amount)
    {
        float addAmount = 0;
        if (MoneyUpdate.DP + amount <= MoneyUpdate.maxDeloyPoint)
        {
            addAmount = amount;
        }
        if (MoneyUpdate.DP + amount > MoneyUpdate.maxDeloyPoint)
        {
            addAmount = MoneyUpdate.maxDeloyPoint - MoneyUpdate.DP;
        }
        MoneyUpdate.DP += addAmount;
    }

    private void ShowLevel()
    {
        //levelText.text = "Level " + (GameSystem.userdata.currentLevel).ToString();
    }
    public void ResetData()
    {
        GameSystem.userdata.currentLevel = 0;
        GameSystem.SaveUserDataToLocal();
        RePlay();
    }

    public void SpeedUp()
    {
        speedUp = !speedUp;
        UpdateSpeed();   
    }
    public void UpdateSpeed()
    {
        imgSpeedUp.color = speedUp ? new Color(0.5f, 0, 0, 1f) : Color.white;
        Time.timeScale = speedUp ? Constants.SPEED_UP_TIME_SCALE : Constants.DEFAULT_TIME_SCALE;
    }
    public void UpdateHeroAmount()
    {
        heroAmountDisplay.text = SelectManagerGameplay.Instance.spawnedHero.Count.ToString() + "/" + "10";
    }

    //public void UpdateEnemyAliveDisplay()
    //{
    //    enemyAliveDisplay.text = enemyAmount.ToString();
    //}

    private int CountEnemyAmount(int level)
    {
        var enemyCount = 0;
        var levelDatas = DataManager.Instance.levelDatas[level];
        foreach (var wave in levelDatas.waveInfos)
        {
            foreach (var info in wave.waveInfoDatas)
            {
                enemyCount += info.Item2;
            }
        }
        if (levelDatas.boss != EnemyType.None) enemyCount++;
        return enemyCount;
    }

    public void DoHpLostEfect()
    {
        loseHpEffect.gameObject.SetActive(true);
        LeanTween.value(0f, 1f, 0.2f).setOnUpdate((y) =>
        {
            var newColor = loseHpEffect.color;
            newColor.a = y;
            loseHpEffect.color = newColor;
        }).setOnComplete(() =>
        {
            LeanTween.value(1f, 0f, 0.2f).setOnUpdate((a) =>
              {
                  var newColor = loseHpEffect.color;
                  newColor.a = a;
                  loseHpEffect.color = newColor;
              }).setOnComplete(() => loseHpEffect.gameObject.SetActive(false));
        });
    }

    public void PauseGame()
    {
        pausePopup.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pausePopup.gameObject.SetActive(false);
        Time.timeScale = Constants.DEFAULT_TIME_SCALE;
    }

    public void GoToNextLevel()
    {
        if (GameSystem.userdata.unlockedHeros.Count <= Constants.MAX_HERO)
        {
            GameSystem.userdata.selectedTeam = GameSystem.userdata.unlockedHeros;
            SceneManager.LoadScene(Constants.SCENE_GAMEPLAY);
        } else
        {
            Home.homeSceneAction = HomeSceneAction.OpenHeroSelect;
            SceneManager.LoadScene(Constants.SCENE_HOME);
        }
    }

    public void Revive()
    {
        startCountDown = false;
        gameState = GameplayState.Playing;
        losePopup.gameObject.SetActive(false);
        var enemies = EnemySpawner.Instance.spawnedEnemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            var newPos = enemies[i].transform.position + new Vector3(0f, 5f);
            enemies[i].transform.DOMove(newPos, 1f);
        }
        LifePoint = 3;
        //var adButton = losePopup.gameObject.GetChildComponent<UIEffectAppear>("Image/ReviveButton");
        //Destroy(adButton.gameObject);
        countdownText.transform.parent.gameObject.SetActive(false);
    }
    public void DoEffectFall(GameObject obj)
    {
        Vector3 end = obj.transform.position;
        Vector3 start = obj.transform.position + new Vector3(0f, FALL_HEIGHT);
        obj.transform.position = start;
        LeanTween.move(obj, end, FALL_TIME).setEaseInCubic().setOnComplete(() => {
            EasyEffect.Bounce(obj, 0.15f);
        });
    }

    public void UpdateTimeScale()
    {
        Time.timeScale = speedUp ? Constants.SPEED_UP_TIME_SCALE : Constants.DEFAULT_TIME_SCALE;
    }
}
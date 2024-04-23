using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;

[System.Serializable]
public class MapInfoData {
    public string mapName;
    public Sprite demoSprite;
    public string description;
    public int level;
    public Sprite enemySprite1;
    public Sprite enemySprite2;
    public int backgroundId;
}

public enum HomeSceneAction
{
    None, OpenShopHero, OpenShopEnergy, OpenHeroSelect
}

public class Home : MonoBehaviour
{
    public static Home Instance;
    public static HomeSceneAction homeSceneAction;
    public static bool showDailyReward = false;

    [System.NonSerialized] public GameObject gachaResult;
    [System.NonSerialized] public GachaProgress gachaProgress;

    [SerializeField] private UIUpdater mapUpdater;
    [SerializeField] private SelectHeroManager teamSelect;
    [SerializeField] private GameObject mapSelect;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private ShopCanvas shop;
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private PopupUnlockHero tryHero;
    [SerializeField] private LuckySpin luckySpin;
    [SerializeField] private GameObject selectHeroGift;
    public TextMeshProUGUI warningMessge;

    [Header("Top Group")]
    public TextMeshProUGUI energyTimer;
    public TextMeshProUGUI energy;
    public TextMeshProUGUI Gem;
    public TextMeshProUGUI Gold;
    public BottomGroup bottomGroup;
    public DailyReward dailyReward;
    public bool isFull;

    public Button nextBTN;
    public Button previousBTN;
    public Button playBTN;
    public EmptySlotManager playGameSlotGroup;
    public List<Transform> allMaps = new List<Transform>();

    float nextUpdateTimer;
    [SerializeField] private Sprite[] iconSprites;
    public Dictionary<string, Sprite> icons = new Dictionary<string, Sprite>();
    public ShopCanvas Shop => shop;
    public SelectHeroManager Team => teamSelect;
    bool isTryingHero;

    public int CurrentLevel {
        get { return GameSystem.userdata.currentLevel; }
        set {
            if (value < 0) value = 0;
            if (value > DataManager.Instance.levelDatas.Count - 1) value = DataManager.Instance.levelDatas.Count - 1;
            GameSystem.userdata.currentLevel = value;
        }
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UpdateEnergyText();
        UpdateGoldText();
        UpdateGemText();
        UpdateDisplayData(CurrentLevel);

        Time.timeScale = 1f;
        teamSelect.CreateCards();
        teamSelect.gameObject.SetActive(false);
        gachaResult = mainUI.GetChildGameObject("GachaResult");
        gachaResult.SetActive(false);
        gachaProgress = mainUI.GetChildComponent<GachaProgress>("GachaProgress");
        if (GoogleAdMobController.Instance.bannerView != null) {
            GoogleAdMobController.Instance.bannerView.Show();
        }
        tryHero.gameObject.SetActive(false);
        warningMessge.gameObject.SetActive(false);
        SetupTrialCard();
        for (int i = 0; i < iconSprites.Length; i++)
        {
            icons.Add(iconSprites[i].name, iconSprites[i]);
        }
        int playGameFirstTime = PlayerPrefs.GetInt("playGameFirstTime", 0);
        if (playGameFirstTime == 0)
        {
            PlayerPrefs.SetInt("playGameFirstTime", 1);
            OnLevelSelect();
            return;
        }
        if (!showDailyReward)
        {
            showDailyReward = true;
            bool showReward = dailyReward.CheckForDailyReward();
            if (!showReward) CheckHomeSceneAction();
        } else
        {
            CheckHomeSceneAction();
        }
    }

    public void Update()
    {
        if (Time.time > nextUpdateTimer)
        {
            UpdateEnergyTimer();
            nextUpdateTimer = Time.time + 1f;
        }
    }

    void CheckHomeSceneAction()
    {
        bottomGroup.SetUnselectAll();

        if (/*GameSystem.userdata.doneDragTutorial &&*/ !GameSystem.userdata.receivedStartHero && GameSystem.userdata.currentLevel >= 5)
        {
            selectHeroGift.SetActive(true);
        }
        else selectHeroGift.SetActive(false);


        switch (homeSceneAction)
        {
            case HomeSceneAction.OpenHeroSelect:
                OnLevelSelect();
                break;
            case HomeSceneAction.OpenShopHero:
                bottomGroup.tabButtons[0].Select();
                shop.OpenHeroGroup();
                break;
            case HomeSceneAction.OpenShopEnergy:
                bottomGroup.tabButtons[0].Select();
                shop.OpenGiftGroup();
                break;
            default:
                bottomGroup.tabButtons[2].Select();
                int rand = UnityEngine.Random.Range(0, 2);
                if (rand == 0)
                {
                    if (!GameSystem.userdata.doneDragTutorial) return;
                    if (DataManager.Instance.dicMonsterAIs.ContainsKey(GameSystem.userdata.trialHeroName) == false) return;
                    ShowTryCardAdsPopup(GameSystem.userdata.trialHeroName);
                }
                //else if (rand == 1)
                //{
                //    luckySpin.gameObject.SetActive(true);
                //    EasyEffect.Appear(luckySpin.gameObject, 0.8f, 1f);
                //}
                break;
        }
    }

    public void UpdateEnergyTimer()
    {
        long ticks = DateTime.Now.Ticks - GameSystem.userdata.lastIncreaseEnergyTimeTicks;
        double seconds = new TimeSpan(ticks).TotalSeconds;
        double maxSeconds = TimeSpan.FromMinutes(Constants.MAX_ENERGY * Constants.MINUTE_INCREASE_1_ENERGY).Ticks;
        if (seconds > maxSeconds) seconds = maxSeconds;
        bool increased = false;
        while (seconds > Constants.MINUTE_INCREASE_1_ENERGY * 60)
        {
            seconds -= Constants.MINUTE_INCREASE_1_ENERGY * 60;
            GameSystem.userdata.energy += 1;
            GameSystem.userdata.lastIncreaseEnergyTimeTicks = DateTime.Now.Ticks;
            increased = true;
        }
        if (GameSystem.userdata.energy > Constants.MAX_ENERGY)
        {
            GameSystem.userdata.energy = Constants.MAX_ENERGY;
        }
        if (increased)
        {
            GameSystem.SaveUserDataToLocal();
        }
        energyTimer.gameObject.SetActive(GameSystem.userdata.energy < Constants.MAX_ENERGY);
        energyTimer.text = TimeSpan.FromSeconds(Constants.MINUTE_INCREASE_1_ENERGY * 60 - seconds).ToString(@"mm\:ss");
    }

    public void UpdateEnergyText()
    {
        energy.text = GameSystem.userdata.energy.ToString() + "/" + Constants.MAX_ENERGY.ToString();
    }

    public void UpdateGoldText()
    {
        Gold.text = GameSystem.userdata.gold.ToString();
    }

    public void UpdateGemText()
    {
        Gem.text = GameSystem.userdata.diamond.ToString();
    }

    public void NextMapSelect() {
        CurrentLevel += 1;
        UpdateDisplayData(CurrentLevel);
    }

    public void PreviousMapSelect()
    {
        CurrentLevel -= 1;
        UpdateDisplayData(CurrentLevel);
    }

    public void UpdateDisplayData(int currentLevel)
    {
        bool unlocked = currentLevel <= GameSystem.userdata.maxLevel;
        Color unlockColor = unlocked ? Color.white : Color.gray;

        previousBTN.gameObject.SetActive(currentLevel != 0);
        nextBTN.gameObject.SetActive(currentLevel < DataManager.Instance.mapDatas.Count - 1);
        playBTN.gameObject.SetActive(unlocked);

        if (currentLevel < 0) currentLevel = 0;
        if (currentLevel >= DataManager.Instance.mapDatas.Count) currentLevel = DataManager.Instance.mapDatas.Count - 1;

        var currentMap = mapUpdater.transform.GetChild(0);
        var demoSprite = currentMap.gameObject.GetChildComponent<Image>("demoSprite");
        demoSprite.color = unlockColor;
        demoSprite.sprite = DataManager.Instance.mapDatas[currentLevel].demoSprite;
        Image enemy1 = currentMap.transform.Find("Enemy1").gameObject.GetComponent<Image>();
        Image enemy2 = currentMap.transform.Find("Enemy2").gameObject.GetComponent<Image>();
        enemy1.color = unlockColor;
        enemy2.color = unlockColor;

        var waveCounts = DataManager.Instance.levelDatas[currentLevel].waveInfos[0].waveInfoDatas.Count;
        if (waveCounts > 1) {
            enemy1.sprite = Resources.Load<Sprite>("EnemyImage/N" + DataManager.Instance.levelDatas[currentLevel].waveInfos[0].waveInfoDatas[0].Item1);
            enemy2.sprite = Resources.Load<Sprite>("EnemyImage/N" + DataManager.Instance.levelDatas[currentLevel].waveInfos[0].waveInfoDatas[1].Item1);
        } else {
            enemy1.sprite = Resources.Load<Sprite>("EnemyImage/N" + DataManager.Instance.levelDatas[currentLevel].waveInfos[0].waveInfoDatas[0].Item1);
            enemy2.sprite = Resources.Load<Sprite>("EnemyImage/N" + DataManager.Instance.levelDatas[currentLevel].waveInfos[0].waveInfoDatas[0].Item1);
        }
        enemy1.SetNativeSize();
        enemy2.SetNativeSize();

        TextMeshProUGUI mapName = currentMap.transform.Find("mapName").gameObject.GetComponent<TextMeshProUGUI>();
        mapName.text = unlocked ? DataManager.Instance.mapDatas[currentLevel].mapName : "???";

        TextMeshProUGUI description = currentMap.transform.Find("description").gameObject.GetComponent<TextMeshProUGUI>();
        description.text = unlocked ? DataManager.Instance.mapDatas[currentLevel].description : "";
    }
    public void OnLevelSelect() {
        if (GameSystem.userdata.currentLevel > GameSystem.userdata.maxLevel) return;

        List<string> unlockedHeroes = new List<string>();
        for (int i = 0; i < GameSystem.userdata.unlockedHeros.Count; i++)
        {
            if (!unlockedHeroes.Contains(GameSystem.userdata.unlockedHeros[i])) unlockedHeroes.Add(GameSystem.userdata.unlockedHeros[i]);
        }
        GameSystem.userdata.unlockedHeros = unlockedHeroes;
        List<string> availableHeroes = new List<string>();
        availableHeroes.AddRange(GameSystem.userdata.unlockedHeros);

        if (isTryingHero) availableHeroes.Add(GameSystem.userdata.trialHeroName);
        if (availableHeroes.Count <= Constants.MAX_HERO)
        {
            GameSystem.chosenMonsters = availableHeroes;
            GameSystem.userdata.selectedTeam = availableHeroes;
            GameSystem.SaveUserDataToLocal();
            //SceneManager.LoadScene(Constants.SCENE_GAMEPLAY);
            SelectHeroManager.Instance.FadeBlackToGameplay();
        } else
        {
            teamSelect.gameObject.SetActive(true);
            mapSelect.SetActive(false);
        }
    }

    public void OnShowMainScene() {
        teamSelect.gameObject.SetActive(false);
        mapSelect.SetActive(true);
    }

    public void OpenFridge()
    {
        LeanTween.cancel(gachaProgress.fridgeButton.gameObject);
        gachaProgress.fridgeButton.transform.localScale = gachaProgress.defaultButtonSize ;
        EasyEffect.Bounce(gachaProgress.fridgeButton.gameObject, 0.1f,0.5f,() => 
        { 
            gachaProgress.ChangeImage();
            var resultItem = DarkcupGames.ObjectPool.Instance.GetGameObjectFromPool("Result Item", gachaProgress.fridgeButton.transform.position);
            resultItem.transform.SetParent(gachaProgress.result.transform);
            resultItem.GetChildComponent<GameObject>("amountText").SetActive(false);          
            EasyEffect.Appear(resultItem, 0f, 4f, 0.2f, 1.2f);
            var gacha = shop.gameObject.GetChildComponent<Gacha>("Scroll View/Viewport/Content/Gacha Content/" + gachaProgress.resgisteredType.ToString());
            gachaProgress.result.UpdateChildUI(gacha.results);
            for (int i = 0; i < gacha.results.Count; i++)
            {
                var vfx = DarkcupGames.ObjectPool.Instance.GetGameObjectFromPool(gacha.results[i].prizeId + "_Vfx", gachaProgress.result.transform);
                vfx.transform.SetParent(gachaProgress.transform);
                vfx.transform.SetSiblingIndex(gachaProgress.result.transform.GetSiblingIndex()-1);
            }
        });        
    }

    public void Waring(TextMeshProUGUI textPopup , string message)
    {
        if (textPopup.gameObject.activeInHierarchy) return;
        textPopup.gameObject.SetActive(true);
        var sq = DOTween.Sequence();
        sq.Append(textPopup.transform.DOMoveY(1f, 1f));
        sq.AppendInterval(1f);
        sq.Append(textPopup.DOFade(0f, 2f).OnComplete(() =>
        {
            textPopup.color = new Color(1, 1, 1, 1);
            textPopup.transform.DOMoveY(0f, 0.1f);
            textPopup.gameObject.SetActive(false);
        }));
        textPopup.text = message;
    }

    public void ClosePopup(GameObject popup)
    {
        popup.transform.DOScale(0, 0.5f).OnComplete(() =>
         {
             popup.SetActive(false);
             popup.transform.localScale = Vector3.one;
         });
    }
    public void OpenPopup(GameObject popup)
    {
        popup.transform.localScale = Vector3.one;
        popup.transform.DOScale(1f, 0.5f);
    }

    [ContextMenu("Reset")]
    public void ResetData()
    { 
        GameSystem.userdata = new UserData();
        GameSystem.SaveUserDataToLocal();
        GameSystem.LoadUserData();
        SceneManager.LoadScene("HeroSelect");
        UpdateEnergyText();
    }

    public void TestGame()
    {
        GameSystem.userdata.energy = 40;
        GameSystem.SaveUserDataToLocal();
    }
    private List<string> FindLockedHero()
    {
        var lockedHero = new List<string>();
        var userHero = GameSystem.userdata.unlockedHeros;
        var allHero = DataManager.Instance.saleHero;
        for (int i = 0; i < allHero.Count; i++)
        {
            if (!userHero.Contains(allHero[i]))
            {
                lockedHero.Add(allHero[i]);
            }
        }
        return lockedHero;
    }

    public void SetupTrialCard()
    {
        var lockedHeros = FindLockedHero();
        if (lockedHeros.Count == 0) return;
        GameSystem.userdata.trialHeroName = lockedHeros.RandomElement();
        GameSystem.SaveUserDataToLocal();
        var heroCards = SelectHeroManager.Instance.heroCards;
        for (int i = 0; i < heroCards.Count; i++)
        {
            if (heroCards[i].heroName.Equals(GameSystem.userdata.trialHeroName))
            {
                heroCards[i].DisplayHero();
                heroCards[i].IsLocked = false;
                heroCards[i].Trial = true;
            }
        }
    }

    public void ShowTryCardAdsPopup(string heroName)
    {
        tryHero.ShowUnlockHero(heroName);
    }
    
    public void GetFreeHero(string heroName)
    {
        shop.popupUnlock.ShowUnlockHero(heroName);
        selectHeroGift.SetActive(false);
        var unlockHero = GameSystem.userdata.unlockedHeros;
        if (!unlockHero.Contains(heroName)) unlockHero.Add(heroName);
        GameSystem.userdata.receivedStartHero = true;
        GameSystem.SaveUserDataToLocal();
    }

    public void UnlockTrialCard()
    {
        isTryingHero = true;
        ClosePopup(tryHero.gameObject);
        var heroCards = SelectHeroManager.Instance.heroCards;
        for (int i = 0; i < heroCards.Count; i++)
        {
            if (heroCards[i].heroName.Equals(GameSystem.userdata.trialHeroName))
            {
                heroCards[i].OnWatchAdsTrialCompleted();
                return;
            }
        }
    }
}
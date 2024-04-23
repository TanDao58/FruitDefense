using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using TMPro;
using DarkcupGames;

public class UpgradePanelUpdater : MonoBehaviour
{
    public static UpgradePanelUpdater Instance;
    private Transform heroCardParent;
    public bool isEnemyList;
    public UpgradePopUp upgradePopUp;
    private TextMeshProUGUI txtHeroName;
    private TextMeshProUGUI txtDescription;
    private TextMeshProUGUI txtGoldPrice;
    private Image displayHp;
    private Image displayAttack;
    private Image displayCooldown;
    private Image imgRank;
    private Transform heroAnimParent;
    private SkeletonGraphic curentHero;
    private string currentHeroName;
    private float canClickAttackTime;

    private void Awake()
    {
        Instance = this;

        txtHeroName = gameObject.GetChildComponent<TextMeshProUGUI>("BG/TopBoard/HeroName/txtHeroName");
        txtDescription = gameObject.GetChildComponent<TextMeshProUGUI>("BG/TopBoard/infoSection/txtDescription");
        txtGoldPrice = gameObject.GetChildComponent<TextMeshProUGUI>("BG/TopBoard/txtGoldPrice");
        displayHp = gameObject.GetChildComponent<Image>("BG/TopBoard/HpBar/Fill");
        displayAttack = gameObject.GetChildComponent<Image>("BG/TopBoard/AttackBar/Fill");
        displayCooldown = gameObject.GetChildComponent<Image>("BG/TopBoard/CdrBar/Fill");
        imgRank = gameObject.GetChildComponent<Image>("BG/TopBoard/HeroImage/rankIcon");
        heroAnimParent = gameObject.GetChildComponent<Transform>("BG/TopBoard/HeroImage");
        heroCardParent = gameObject.GetChildComponent<Transform>("BG/HeroScroll/Viewport/Content");
        if (upgradePopUp == null)
        {
            upgradePopUp = FindObjectOfType<UpgradePopUp>(true);
        }
    }
    private void Start()
    {
        if (isEnemyList == false)
        {
            CreateAllCard(DataManager.Instance.allyNames);
        } else
        {
            CreateAllCard(DataManager.Instance.enemyNames);
        }
    }
    void CreateAllCard(List<string> names)
    {
        var src = heroCardParent.transform.GetChild(0).GetComponent<HeroCard>();
        for (int i = 0; i < names.Count; i++)
        {
            var card = Instantiate<HeroCard>(src, heroCardParent);
            card.SetData(names[i]);
            card.gameObject.SetActive(true);
        }
    }
    public void ShowRandom()
    {
        if (GameSystem.userdata.unlockedHeros.Count == 0)
        {
            GameSystem.userdata.unlockedHeros.Add("E0");
        }
        if (GameSystem.userdata.seenEnemies.Count == 0)
        {
            GameSystem.userdata.seenEnemies.Add("N1");
        }
        if (isEnemyList)
        {
            UpdateDisplay(GameSystem.userdata.seenEnemies.RandomElement());
        }
        else
        {
            UpdateDisplay(GameSystem.userdata.unlockedHeros.RandomElement());
        }
    }
    public void UpdateDisplay(string heroName)
    {
        if (GameSystem.userdata.heroUnlockedAmounts.ContainsKey(heroName) == false)
        {
            GameSystem.userdata.heroUnlockedAmounts.Add(heroName, 1);
            GameSystem.SaveUserDataToLocal();
        }
        if (GameSystem.userdata.unlockedHeroesLevel.ContainsKey(heroName) == false)
        {
            GameSystem.userdata.unlockedHeroesLevel.Add(heroName, 1);
            GameSystem.SaveUserDataToLocal();
        }
        ReplaceHeroAnim(heroName);

        MonsterAI monster = DataManager.Instance.dicMonsterAIs[heroName];
        string name = monster.monsterData.nickName;
        if (name == "") name = monster.monsterData.monsterName;
        txtHeroName.text = name;
        txtDescription.text = monster.monsterData.decripsion;
        if(txtGoldPrice != null) txtGoldPrice.text = monster.moneyCost.ToString();
        displayHp.fillAmount = monster.monsterData.displayHp;
        displayAttack.fillAmount = monster.monsterData.displayAttack;
        displayCooldown.fillAmount = monster.monsterData.displayCooldown;
        if (isEnemyList) imgRank.gameObject.SetActive(false);
        else imgRank.sprite = Resources.Load<Sprite>("Rank/" + monster.monsterData.rarity.ToString());
        SwitchToAttack();
    }

    public int GetBaseAmountToLevelUp(string rarity)
    {
        switch (rarity)
        {
            case "C":
                return 20;
            case "R":
                return 10;
            case "S":
                return 6;
            case "SR":
                return 2;
            case "SSR":
                return 1;
        }
        return 0;
    }
    public void ReplaceHeroAnim(string heroName)
    {
        if (this.currentHeroName == heroName) return;
        this.currentHeroName = heroName;
        if (curentHero != null)
        {
            Destroy(curentHero.gameObject);
        }
        string path = GeneralUltility.BuildString("SkeletonGraphic/", heroName);
        curentHero = Instantiate(Resources.Load<SkeletonGraphic>(path), heroAnimParent);
        curentHero.GetComponent<Button>().onClick.AddListener(SwitchToAttack);
        curentHero.GetComponent<Button>().transition = Selectable.Transition.None;
    }
    
    public void SwitchToAttack()
    {
        if (Time.time < canClickAttackTime) return;
        if (curentHero == null) return;

        Spine.Animation attack, idle;
        var animNameList = new List<string>() { "Ulti", "Attack", "Run" };
        if (Random.Range(0,2) == 0)
        {
            attack = FindExistAnimation(curentHero, animNameList);
        } else
        {
            attack = FindExistAnimation(curentHero, animNameList);
        }
        idle = FindExistAnimation(curentHero, new List<string>() { "Idle", "Run" });
        curentHero.AnimationState.SetAnimation(0, attack.Name, false);
        var tempHero = curentHero;
        LeanTween.delayedCall(attack.Duration, () =>
        {
            if (tempHero == null) return;
            tempHero.AnimationState.SetAnimation(0, idle.Name, true);
        });
        canClickAttackTime = Time.time + attack.Duration + 0.1f;
    }

    public static Spine.Animation FindExistAnimation(SkeletonGraphic skeletonGraphic, List<string> names)
    {
        for (int i = 0; i < names.Count; i++)
        {
            Spine.Animation temp = skeletonGraphic.SkeletonData.FindAnimation(names[i]);
            if (temp != null) return temp;
        }
        return skeletonGraphic.AnimationState.GetCurrent(0).Animation;
    }

    public bool IsAbleToLevelUp(int heroAmount, int inneedToLevelUp) {

        return heroAmount >= inneedToLevelUp;
    }

    public void HeroLevelUp()
    {
        string heroname = Instance.currentHeroName;
        if (heroname == null) return;
        var hero = Resources.Load<GameObject>(heroname);
        var heroselect = hero.GetComponent<MonsterAI>();
        if (GameSystem.userdata.gold >= heroselect.coinToUpgrade)
        {
            GameSystem.userdata.unlockedHeroesLevel[heroname]++;
            GameSystem.userdata.heroUnlockedAmounts[heroname] -= heroselect.amountToLevelUp;
            heroselect.amountToLevelUp = Mathf.FloorToInt(heroselect.baseAmountToLevelUp
                        + Mathf.Pow(GameSystem.userdata.unlockedHeroesLevel[heroname], 2));
            heroselect.coinToUpgrade = heroselect.baseMoneyToUpgrade + Mathf.Pow(GameSystem.userdata.unlockedHeroesLevel[heroname], 2);
            GameSystem.SaveUserDataToLocal();
        }
        else
        {
            Debug.Log("Bug");
        }

        UpdateDisplay(heroname);
    }

    public void OpenUpgradePopup()
    {
        upgradePopUp.gameObject.SetActive(true);
        upgradePopUp.DisplayHero(currentHeroName);
    }
}
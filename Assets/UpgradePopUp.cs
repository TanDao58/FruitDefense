using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;
using DG.Tweening;

public class UpgradePopUp : MonoBehaviour
{
    public MonsterAI displayMonser;
    [SerializeField] private Sprite goldButton;
    [SerializeField] private Sprite greyButton;
    [SerializeField] private Image bg;
    [SerializeField] private GameObject popup;
    [Header("Name")]
    [SerializeField] private TextMeshProUGUI heroName;

    [Header("Hero Image")]
    [SerializeField] private SkeletonGraphic currentHero;
    [SerializeField] private SkeletonGraphic nextHero;

    [Header("Rank")]
    [SerializeField] private Image rankImg;

    [Header("Hp Stat")]
    [SerializeField] private Image hpValue;
    [SerializeField] private TextMeshProUGUI currentHp;
    [SerializeField] private TextMeshProUGUI addHp;

    [Header("Atk Stat")]
    [SerializeField] private Image atkValue;
    [SerializeField] private TextMeshProUGUI currentAtk;
    [SerializeField] private TextMeshProUGUI addAtk;

    [Header("Info")]
    [SerializeField] private Image progressValue;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI currentLevel;
    [SerializeField] private TextMeshProUGUI maxLevel;
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private TextMeshProUGUI userHave;
    [SerializeField] private Button upgradeButton;

    private CurrencyType upgradeCurrency;

    private void Awake()
    {
        bg = GetComponent<Image>();
        popup = gameObject.GetChildComponent<GameObject>("Popup");
        heroName = gameObject.GetChildComponent<TextMeshProUGUI>("Popup/HeroName/Text (TMP)");
        currentHero = gameObject.GetChildComponent<SkeletonGraphic>("Popup/HeroDisplay/Hero");
        nextHero = gameObject.GetChildComponent<SkeletonGraphic>("Popup/HeroDisplay/Shadow");
        rankImg = gameObject.GetChildComponent<Image>("Popup/Rank");
        hpValue = gameObject.GetChildComponent<Image>("Popup/StatGroup/Hp/Bar/Value");
        currentHp = gameObject.GetChildComponent<TextMeshProUGUI>("Popup/StatGroup/Hp/Bar/TxtGroup/Value");
        addHp = gameObject.GetChildComponent<TextMeshProUGUI>("Popup/StatGroup/Hp/Bar/TxtGroup/AddValue");
        atkValue = gameObject.GetChildComponent<Image>("Popup/StatGroup/Atk/Bar/Value");
        currentAtk = gameObject.GetChildComponent<TextMeshProUGUI>("Popup/StatGroup/Atk/Bar/TxtGroup/Value");
        addAtk = gameObject.GetChildComponent<TextMeshProUGUI>("Popup/StatGroup/Atk/Bar/TxtGroup/AddValue");
        progressValue = gameObject.GetChildComponent<Image>("Popup/Info/ProgressGroup/ProgressBar/ProgessValue");
        currentLevel = gameObject.GetChildComponent<TextMeshProUGUI>("Popup/Info/ProgressGroup/Level");
        maxLevel = gameObject.GetChildComponent<TextMeshProUGUI>("Popup/Info/ProgressGroup/NextLevel");
        icon = gameObject.GetChildComponent<Image>("Popup/Info/UpgradeButton/PriceGroup/Icon");
        priceTxt = gameObject.GetChildComponent<TextMeshProUGUI>("Popup/Info/UpgradeButton/PriceGroup/PriceTxt");
        userHave = gameObject.GetChildComponent<TextMeshProUGUI>("Popup/Info/UpgradeButton/PriceGroup/UserHave");
        upgradeButton = gameObject.GetChildComponent<Button>("Popup/Info/UpgradeButton");

        //gameObject.SetActive(false);
    }

    public void DisplayHero(string id)
    {
        bg.enabled = true;
        gameObject.SetActive(true);
        popup.transform.localScale = Vector3.zero;
        popup.transform.DOScale(1f, 0.2f);
        displayMonser = DataManager.Instance.dicMonsterAIs[id];
        var monsterData = displayMonser.monsterData;
        currentHero.Clear();
        currentHero.skeletonDataAsset = displayMonser.GetComponentInChildren<SkeletonAnimation>().skeletonDataAsset;
        currentHero.Initialize(true);
        nextHero.Clear();
        nextHero.skeletonDataAsset = displayMonser.GetComponentInChildren<SkeletonAnimation>().skeletonDataAsset;
        nextHero.Initialize(true);
        heroName.text = monsterData.nickName;
        rankImg.sprite = Resources.Load<Sprite>("Rank/" + monsterData.rarity.ToString());
        LeanTween.value(0f, monsterData.displayHp, 0.5f).setOnUpdate((y) =>
        {
            if (hpValue != null)
                hpValue.fillAmount = y;
        });
        LeanTween.value(0f, monsterData.displayAttack, 0.5f).setOnUpdate((y) =>
        {
            if (atkValue != null)
                atkValue.fillAmount = y;
        });
        //hpValue.fillAmount = monsterData.displayHp;
        //atkValue.fillAmount = monsterData.displayAttack;
        UpdateDisplay(monsterData);
    }

    public float CalculateMoney(float level, Rarity rarity)
    {
        switch(rarity)
        {
            case Rarity.C:
                return Constants.BASE_UPGRADE_MONEY_C * Mathf.Pow(level + 1, 2f) * 0.5f;
            case Rarity.R:
                return Constants.BASE_UPGRADE_MONEY_R * Mathf.Pow(level + 1, 2f) * 0.5f;
            case Rarity.SR:
                return Constants.BASE_UPGRADE_MONEY_SR * Mathf.Pow(level + 1, 2f) * 0.5f;
            case Rarity.SSR:
                return Constants.BASE_UPGRADE_MONEY_SSR * Mathf.Pow(level + 1, 2f) * 0.5f;
            default:
                return 0;
        }
    }

    public int CheckMaxLevel(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.C:
                return 10;
            case Rarity.R:
                return 7;
            case Rarity.SR:
                return 5;
            case Rarity.SSR:
                return 3;
            default:
                return 0;
        }
    }

    public float CheckNextStat(float baseStat, float level)
    {
        var maxStat = baseStat * 4;
        return baseStat + ((maxStat - baseStat) / 25f) * (level + 1);
    }

    public void Close()
    {
        bg.enabled = false;
        popup.transform.DOScale(0f, 0.2f).OnComplete(() => gameObject.SetActive(false));
    }

    private void UpdateDisplay(MonsterData monsterData)
    {
        currentHp.text = ((int)monsterData.maxhp).ToString();
        currentAtk.text = ((int)monsterData.damage).ToString();
        LeanTween.value(0f, monsterData.level / CheckMaxLevel(monsterData.rarity), 0.5f).setOnUpdate((y) => 
        {
            if (progressValue != null)
                progressValue.fillAmount = y;
        });
        //progressValue.fillAmount = monsterData.level / CheckMaxLevel(monsterData.rarity);
        currentLevel.text = monsterData.level.ToString();
        maxLevel.text = "Lv: " + CheckMaxLevel(monsterData.rarity).ToString();

        if (monsterData.baseRarity < Rarity.SR && monsterData.rarity == Rarity.SR && monsterData.level == CheckMaxLevel(monsterData.rarity))
        {
            upgradeButton.gameObject.SetActive(false);
            addHp.text = " + 0";
            addAtk.text = " + 0";
            return;
        }
        addHp.text = GeneralUltility.BuildString("", "+ ", ((int)(CheckNextStat(monsterData.baseHp, monsterData.level) - monsterData.maxhp)).ToString());
        addAtk.text = GeneralUltility.BuildString("", "+ ", ((int)(CheckNextStat(monsterData.baseDamge, monsterData.level) - monsterData.damage)).ToString());
        upgradeButton.gameObject.SetActive(true);
        if (monsterData.level == CheckMaxLevel(monsterData.rarity))
        {
            if ((int)monsterData.rarity >= 0 && (int)monsterData.rarity < 2)
            {
                upgradeCurrency = CurrencyType.GreenLeaf;
                icon.sprite = Resources.Load<Sprite>("GreenLeaf");
                priceTxt.text = ((int)monsterData.rarity + 1).ToString();
                //priceTxt.alignment = TextAlignmentOptions.MidlineRight;
                userHave.gameObject.SetActive(true);
                userHave.text = GameSystem.userdata.greenLeaf.ToString() + " /";
            }
            else if ((int)monsterData.rarity == 2)
            {
                upgradeCurrency = CurrencyType.GoldLeaf;
                icon.sprite = Resources.Load<Sprite>("GoldLeaf");
                priceTxt.text = "1";
                //priceTxt.alignment = TextAlignmentOptions.MidlineRight;
                userHave.gameObject.SetActive(true);
                userHave.text = GameSystem.userdata.goldLeaf.ToString() + " /";
            }
            else
            {
                upgradeCurrency = CurrencyType.None;
                upgradeButton.gameObject.SetActive(false);
                addHp.text = " + 0";
                addAtk.text = " + 0";
                userHave.gameObject.SetActive(false);
            }
        }
        else
        {
            upgradeCurrency = CurrencyType.Coin;
            icon.sprite = Resources.Load<Sprite>("Coin");
            priceTxt.text = CalculateMoney(monsterData.level, monsterData.rarity).ToString();
            //priceTxt.alignment = TextAlignmentOptions.Center;
            userHave.gameObject.SetActive(false);
        }
        var enoughCurrency = CheckCurrency();
        if(!enoughCurrency)
        {
            upgradeButton.image.sprite = greyButton;
            upgradeButton.interactable = false;
            if (userHave.gameObject.activeInHierarchy) userHave.color = Color.red;
            else priceTxt.color = Color.red;
        }
        else
        {
            upgradeButton.image.sprite = goldButton;
            upgradeButton.interactable = true;
            if (userHave.gameObject.activeInHierarchy) userHave.color = Color.white;
            priceTxt.color = Color.white;
        }
    }

    private bool CheckCurrency()
    {
        var userData = GameSystem.userdata;
        var price = float.Parse(priceTxt.text);
        switch(upgradeCurrency)
        {
            case CurrencyType.Coin:
                return userData.gold > price;
            case CurrencyType.GreenLeaf:
                return userData.greenLeaf > price;
            case CurrencyType.GoldLeaf:
                return userData.goldLeaf > price;
            default: return false;
        }
    }
    public void UpgradeHero()
    {
        var price = float.Parse(priceTxt.text);
        var userData = GameSystem.userdata;
        switch(upgradeCurrency)
        {
            case CurrencyType.Coin:
                userData.gold -= price;
                break;
            case CurrencyType.GreenLeaf:
                userData.greenLeaf -= (int)price;
                break;
            case CurrencyType.GoldLeaf:
                userData.goldLeaf -= (int)price;
                break;
        }
        var monsterData = displayMonser.monsterData;
        if (monsterData.level < CheckMaxLevel(monsterData.rarity))
        {
            monsterData.damage = CheckNextStat(monsterData.baseDamge, monsterData.level);
            monsterData.maxhp = CheckNextStat(monsterData.baseHp, monsterData.level);
            monsterData.level++;
        }
        else
        {
            monsterData.rarity += 1;
            rankImg.sprite = Resources.Load<Sprite>("Rank/" + monsterData.rarity.ToString());
            monsterData.level = 1;
            UpgradePanelUpdater.Instance.UpdateDisplay(monsterData.monsterName);
        }

        GameSystem.userdata.unlockHeroDatas[monsterData.monsterName] = displayMonser.monsterData;
        GameSystem.SaveUserDataToLocal();
        UpdateDisplay(monsterData);
    }   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChooseHero : MonoBehaviour
{
    public static ChooseHero instance;
    [Header("Script Stats")]
    public GameObject currentSelect;
    public string heroName;

    [Header("Component")]
    public Button levelUpButton;
    public Sprite _upgradeable;
    public Sprite _unUpgradeable;
    public Image noticeSign;
    public GameObject upgradePopup;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        heroName = null;
        UpdateUpgradePanel();
        noticeSign.gameObject.SetActive(false);
        upgradePopup.gameObject.SetActive(false);
    }
    public void UpdateUpgradePanel()
    {

        SetLevelUpButtonEnable();
        if (heroName == null)
        {
            noticeSign.gameObject.SetActive(false);
            return;
        }
        GameObject hero = Resources.Load<GameObject>(heroName);
        MonsterAI monsterAI = hero.GetComponent<MonsterAI>();
        noticeSign.gameObject.SetActive(GameSystem.userdata.heroUnlockedAmounts[heroName] >= monsterAI.amountToLevelUp && currentSelect != null);
        monsterAI.amountToLevelUp = (int)monsterAI.baseAmountToLevelUp + Mathf.FloorToInt(Mathf.Pow(GameSystem.userdata.unlockedHeroesLevel[heroName], 2));
        //MonsterAI monsterAI = hero.GetComponent<MonsterAI>();
        var Bottom = upgradePopup.transform.Find("Bottom");
        Button upgradeButton = Bottom.transform.Find("UpgradeButton").GetComponent<Button>();
        Image progressFill = Bottom.transform.Find("ProgressBar").transform.Find("Fill").GetComponent<Image>();
        TextMeshProUGUI level = Bottom.transform.Find("Level").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI heroAmount = Bottom.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        progressFill.fillAmount = (float)GameSystem.userdata.heroUnlockedAmounts[heroName] / monsterAI.amountToLevelUp;
        heroAmount.text = GameSystem.userdata.heroUnlockedAmounts[heroName].ToString() + "/" + monsterAI.amountToLevelUp.ToString();
        level.text = "Lv " + GameSystem.userdata.unlockedHeroesLevel[heroName].ToString();
        upgradeButton.enabled = GameSystem.userdata.heroUnlockedAmounts[heroName] >= monsterAI.amountToLevelUp;
        Image upgradeButtonIMG = upgradeButton.GetComponent<Image>();
        upgradeButtonIMG.sprite = upgradeButton.enabled ? _upgradeable : _unUpgradeable;
        TextMeshProUGUI price = upgradeButton.transform.Find("Price").GetComponent<TextMeshProUGUI>();
        monsterAI.coinToUpgrade = monsterAI.baseMoneyToUpgrade + Mathf.Pow(GameSystem.userdata.unlockedHeroesLevel[heroName], 2);
        price.text = (monsterAI.coinToUpgrade / 100).ToString() + "K";
        upgradeButton.onClick.AddListener(() =>
        {
            CheckPlayerGold(monsterAI.coinToUpgrade, monsterAI.amountToLevelUp);
        });
    }

    public void CheckPlayerGold(float goldAmount,int heroAmount)
    {
        if(GameSystem.userdata.gold >= goldAmount)
        {
            GameSystem.userdata.gold -= goldAmount;
            GameSystem.userdata.unlockedHeroesLevel[heroName]++;
            GameSystem.userdata.heroUnlockedAmounts[heroName] -= heroAmount;
            GameSystem.SaveUserDataToLocal();
            UpdateUpgradePanel();
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }


    private void SetLevelUpButtonEnable()
    {
        if (heroName == null)
        {
            var buttonImage = levelUpButton.GetComponent<Image>();
            buttonImage.sprite = _unUpgradeable;
            levelUpButton.enabled = false;
        }

        else
        {
            var buttonImage = levelUpButton.GetComponent<Image>();
            buttonImage.sprite = _upgradeable;
            levelUpButton.enabled = true;
        }
    }
}

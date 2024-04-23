using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpgradeController : MonoBehaviour
{
    public static UpgradeController instance;
    public Image progressAmount;
    public TextMeshProUGUI heroesAmount;
    public TextMeshProUGUI level;
    public TextMeshProUGUI goldToUpgrade;
    public Image avatar;
    public Button upgradeButton;
    public Sprite upgradeableImage;
    public Sprite unanbleImage;
    public GameObject selectedHero;
    private void Awake()
    {
        instance = this;
    }
    public void UpdatePanel(GameObject hero)
    {
        selectedHero = hero;
        var monsterAI = hero.GetComponent<MonsterAI>();

        monsterAI.coinToUpgrade = monsterAI.baseMoneyToUpgrade + Mathf.Pow(GameSystem.userdata.unlockedHeroesLevel[selectedHero.name],2);
        goldToUpgrade.text = (monsterAI.coinToUpgrade / 100).ToString() + "K";
        heroesAmount.text = GameSystem.userdata.heroUnlockedAmounts[hero.name].ToString() + "/" + hero.GetComponent<MonsterAI>().amountToLevelUp;
        progressAmount.fillAmount = (float)GameSystem.userdata.heroUnlockedAmounts[hero.name] / hero.GetComponent<MonsterAI>().amountToLevelUp;
        if(progressAmount.fillAmount < 1)
        {
            upgradeButton.GetComponent<Image>().sprite = unanbleImage;
            upgradeButton.enabled = false;
        }
        else
        {
            upgradeButton.GetComponent<Image>().sprite = upgradeableImage;
            upgradeButton.enabled = true;
        }
        level.text = "Lv " + GameSystem.userdata.unlockedHeroesLevel[hero.name].ToString();
        //avatar.sprite = hero.GetComponent<MonsterAI>().portrait;
    }

    public void OnUpgradeClick()
    {

        var monsterAI = selectedHero.GetComponent<MonsterAI>();
        if(GameSystem.userdata.gold < monsterAI.coinToUpgrade)
        {
            return;
        }
        else
        {
            GameSystem.userdata.gold -= monsterAI.coinToUpgrade;
        }
        if (GameSystem.userdata.heroUnlockedAmounts["E2"] >= monsterAI.amountToLevelUp)
        {
            GameSystem.userdata.unlockedHeroesLevel[selectedHero.name] += 1;
            GameSystem.userdata.heroUnlockedAmounts[selectedHero.name] -= monsterAI.amountToLevelUp;
            GameSystem.SaveUserDataToLocal();
        }
        UpdatePanel(selectedHero);
    }

}

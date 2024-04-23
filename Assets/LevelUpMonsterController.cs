using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
public class LevelUpMonsterController : MonoBehaviour {
    public static LevelUpMonsterController instance;
    public List<string> heroName = new List<string>();
    public List<HeroCard> heroCards = new List<HeroCard>();

    public EmptySlotManager emptySlotManager;
    //public HeroCardsManager heroCardsManager;
    // Start is called before the first frame update
    private void Awake() {
        instance = this;
    }
    [ContextMenu("Test Add Hero")]
    public void TestAddHeroesAmount(string heroName) {
        if (GameSystem.userdata.unlockedHeros.Contains(heroName)) {
            GameSystem.userdata.heroUnlockedAmounts[heroName] += 100;
            GameSystem.SaveUserDataToLocal();
        } else {
            GameSystem.userdata.unlockedHeros.Add(heroName);
            GameSystem.SaveUserDataToLocal();
        }
    }

    [ContextMenu("Text level Up")]
    public void TestLevelUpHero() {
        GameObject obj = Resources.Load<GameObject>("E2");
        var monsterAI = obj.GetComponent<MonsterAI>();
        Debug.Log(obj.name);
        Debug.Log(GameSystem.userdata.unlockedHeroesLevel[obj.name]);
        if (GameSystem.userdata.heroUnlockedAmounts["E2"] >= monsterAI.amountToLevelUp) {
            Debug.Log("Before amount: " + GameSystem.userdata.heroUnlockedAmounts["E2"]);
            Debug.Log("Before level: " + GameSystem.userdata.unlockedHeroesLevel["E2"]);
            Debug.Log("Amount to level up: " + monsterAI.amountToLevelUp);
            GameSystem.userdata.unlockedHeroesLevel["E2"] += 1;
            GameSystem.userdata.heroUnlockedAmounts["E2"] -= monsterAI.amountToLevelUp;

            Debug.Log("After amount: " + GameSystem.userdata.heroUnlockedAmounts["E2"]);
            Debug.Log("After level: " + GameSystem.userdata.unlockedHeroesLevel["E2"]);
            GameSystem.SaveUserDataToLocal();
        }

    }
    [ContextMenu("AddGold")]
    public void GoldIncrease() {
        GameSystem.userdata.gold += 1000000;
        Home.Instance.UpdateGoldText();

    }
    [ContextMenu("ShowHero")]
    public void DebugHero() {
        foreach (KeyValuePair<string, int> item in GameSystem.userdata.unlockedHeroesLevel) {
            Debug.Log("Hero Name:" + item.Key + "Level:" + item.Value);
        }

        foreach (KeyValuePair<string, int> item in GameSystem.userdata.heroUnlockedAmounts) {
            Debug.Log("hero name:" + item.Key + "amount: " + item.Value);
        }
    }
    public void AddHero() {
        GameSystem.userdata.unlockedHeros.Add("E5");
        GameSystem.userdata.unlockedHeros.Add("E7");
        GameSystem.SaveUserDataToLocal();
    }
    [ContextMenu("Test Chosen")]
    public void UnlockAllHeroes() {
        for (int i = 0; i <= 12; i++) {
            if (!GameSystem.userdata.unlockedHeros.Contains("E" + (i).ToString())) {
                GameSystem.userdata.unlockedHeros.Add("E" + (i).ToString());
                GameSystem.userdata.unlockedHeroesLevel.Add("E" + (i).ToString(), 1);
                GameSystem.userdata.heroUnlockedAmounts.Add("E" + (i).ToString(), 1);
                GameSystem.SaveUserDataToLocal();
            }

        }

        GameSystem.SaveUserDataToLocal();
        SceneManager.LoadScene("HeroSelect");
    }
}

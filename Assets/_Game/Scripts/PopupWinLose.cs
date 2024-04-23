using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkcupGames;
using UnityEngine.SceneManagement;
using TMPro;

public class PopupWinLose : MonoBehaviour
{
    private const int DIAMOND_REWARD = 50;
    public PopupUnlockHero popupUnlockHero;
    [Header("Gold Reward")]
    public TextMeshProUGUI txtGold;
    public float goldReward;

    [Header("Diamond Reward")]
    public TextMeshProUGUI txtDiamond;
    public int diamondReward = 0;

    [Header("Star")]
    public Image[] stars;
    public Sprite onSprite;
    public Sprite offSprite;

    public SpriteRenderer sprVictory;

    public float expReward;
    float reward = 0;
    float exp;

    public void ShowPopup(bool win = true)
    {
        if (sprVictory != null) sprVictory.gameObject.SetActive(false);
        gameObject.SetActive(true);
        reward = goldReward * Random.Range(0.75f, 1.25f);
        exp = expReward * Random.Range(0.75f, 1.25f);
        txtGold.text = ((int)reward).ToString();
        diamondReward = 0;
        if (win)
        {
            txtDiamond.transform.parent.gameObject.SetActive(false);
            var hp = Gameplay.Intansce.LifePoint;
            if (hp > 5) hp = 5;
            var starAmount = CheckStar(hp);
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(true);
                stars[i].sprite = offSprite;
            }
            for (int i = 0; i < starAmount; i++)
            {
                stars[i].sprite = onSprite;
            }
            if (starAmount >= 3 && !GameSystem.userdata.levelReward[EnemySpawner.Instance.levelData.levelName])
            {
                Debug.Log("3 stars");
                txtDiamond.transform.parent.gameObject.SetActive(true);
                diamondReward = DIAMOND_REWARD;
                txtDiamond.text = diamondReward.ToString();
                GameSystem.userdata.levelReward[EnemySpawner.Instance.levelData.levelName] = true;

            }
        }
        Time.timeScale = 1f;
        Debug.Log($"calling onenable on {gameObject.name}");
        GetComponent<UIEffect>().DoEffect(false);
    }

    private int CheckStar(int hp)
    {
        switch (hp)
        {
            case 1: return 1;
            case 5: return 3;
            default: return 2;
        }
    }

    public void Next()
    {
        string heroName = TryUnlockHero();
        if (heroName != "")
        {
            popupUnlockHero.ShowUnlockHero(heroName);
        }
        else
        {
            Gameplay.Intansce.GoToNextLevel();
        }
    }

    public void GotoShop()
    {
        Home.homeSceneAction = HomeSceneAction.OpenShopHero;
        SceneManager.LoadScene(Constants.SCENE_HOME);
    }
    public void GotoMainMenu()
    {
        Home.homeSceneAction = HomeSceneAction.None;
        SceneManager.LoadScene(Constants.SCENE_HOME);
    }

    public static string TryUnlockHero()
    {
        List<string> unlockNames = DataManager.Instance.unlockHero;
        List<int> unlockLevels = DataManager.Instance.unlockLevels;

        List<string> teams = new List<string>();
        for (int i = 0; i < unlockNames.Count; i++)
        {
            teams.Add(unlockNames[i]);
            if (GameSystem.userdata.unlockedHeros.Contains(unlockNames[i]) == false && GameSystem.userdata.currentLevel >= unlockLevels[i])
            {
                GameSystem.userdata.unlockedHeros.Add(unlockNames[i]);
                var monsterData = DataManager.Instance.dicMonsterAIs[unlockNames[i]].monsterData;
                if (!GameSystem.userdata.unlockHeroDatas.ContainsKey(unlockNames[i]))
                {
                    monsterData.damage = monsterData.baseDamge;
                    monsterData.maxhp = monsterData.baseHp;
                    monsterData.level = 1;
                    monsterData.rarity = monsterData.baseRarity;
                    GameSystem.userdata.unlockHeroDatas.Add(unlockNames[i], monsterData);
                }
                GameSystem.userdata.unlockedHeroesLevel.Add(unlockNames[i], 1);
                if (teams.Count <= 5)
                {
                    GameSystem.chosenMonsters = teams;
                    GameSystem.userdata.selectedTeam = teams;
                }
                GameSystem.SaveUserDataToLocal();
                return unlockNames[i];
            }
        }
        return "";
    }

    public void OnDoubleReward()
    {
        reward *= 2;
        diamondReward *= 2;
        txtDiamond.text = diamondReward.ToString();
        txtGold.text = reward.ToString("F0");
        var goldTxtEffect = txtGold.GetComponent<UIEffectRunNumber>();
        goldTxtEffect.Prepare();
        goldTxtEffect.DoEffect();
        var diamondTxtEffect = txtDiamond.GetComponent<UIEffectRunNumber>();
        diamondTxtEffect.Prepare();
        diamondTxtEffect.DoEffect();
        gameObject.GetChildComponent<Button>("Image/DoubleReward").gameObject.SetActive(false);
    }

    public void GetReward()
    {
        GameSystem.userdata.gold += reward;
        GameSystem.userdata.diamond += diamondReward;
        GameSystem.userdata.exp += exp;
        GameSystem.SaveUserDataToLocal();
    }
}
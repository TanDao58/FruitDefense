using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class UserData {
    public string userid;
    public string username;
    public string usertoken;
    public string facebookid;
    public string facebooktoken;
    public string googleid;
    public string googletoken;
    public string appleid;
    public string appletoken;

    public float energy;
    public float diamond;
    public float gold;
    public float exp;
    public int goldLeaf;
    public int greenLeaf;
    public bool earnedEnergy;
    public long lastEarnGemTime;
    public long lastIncreaseEnergyTimeTicks;
    public float playtime;
    public float rankpoint;
    public bool isVipMember;
    public float currentXp;
    public float baseXp;
    public List<string> boughtItems;
    public int level;
    public int rewardCount = 0;
    public int nextReward = 0;
    public List<bool> receivedRewards;
    public int loginDay;
    public long lastLoginTime;
    public int userLevel = 1;
    public int currentLevel = 0;
    public int maxLevel = 0;
    public int currentWave = 0;
    public int langueIndex;
    public string trialHeroName;
    public List<string> unlockedHeros;
    public List<string> seenEnemies;
    public List<string> selectedTeam;
    public Dictionary<int, bool> dailyReward = new Dictionary<int, bool>();
    public Dictionary<BuffType, int> buff = new Dictionary<BuffType, int>();
    public Dictionary<string, string> unlockedHeroesRank = new Dictionary<string, string>();
    public Dictionary<string, int> unlockedHeroesLevel = new Dictionary<string, int>();
    public Dictionary<string, int> heroUnlockedAmounts = new Dictionary<string, int>();
    public Dictionary<string, MonsterData> unlockHeroDatas = new Dictionary<string, MonsterData>();
    public Dictionary<string, bool> levelReward = new Dictionary<string, bool>();
    public bool doneDragTutorial;
    public bool receivedStartHero;
    public int totalIntertistialAds;
    public int totalRewardeddAds;

    public UserData()
    {
        receivedStartHero = false;
        trialHeroName = "";
        energy = 40;
        earnedEnergy = false;
        lastIncreaseEnergyTimeTicks = DateTime.Now.Ticks;
        userLevel = 1;
        receivedRewards = new List<bool>() { false, false, false, false, false, false, false, false, false, false };
        unlockedHeros = new List<string>();
        boughtItems = new List<string>();
        seenEnemies = new List<string>();
        //if (!unlockedHeros.Contains("E0")) {
        //    unlockedHeros.Add("E0");
        //}
        loginDay = 1;
        dailyReward = new Dictionary<int, bool>();
        buff = new Dictionary<BuffType, int>();
        unlockedHeroesLevel = new Dictionary<string, int>();
        heroUnlockedAmounts = new Dictionary<string, int>();
        unlockedHeroesRank = new Dictionary<string, string>();
        unlockHeroDatas = new Dictionary<string, MonsterData>();
        levelReward = new Dictionary<string, bool>();

        var buffType = (BuffType[])Enum.GetValues(typeof(BuffType));
        for (int i = 1; i < 8; i++)
        {
            dailyReward.Add(i, false);
        }
        for (int i = 0; i < buffType.Length; i++)
        {
            buff.Add(buffType[i], 3);
        }
        foreach (string heroName in unlockedHeros)
        {
            unlockedHeroesLevel.Add(heroName, 1);
            heroUnlockedAmounts.Add(heroName, 1);
            unlockedHeroesRank.Add("C", heroName);
        }
        rewardCount = 0;
        doneDragTutorial = false;
    }
}

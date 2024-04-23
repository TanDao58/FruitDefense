using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DailyReward : MonoBehaviour
{
    [SerializeField] private Sprite disableImg;
    [SerializeField] private GameObject selectHeroGift;
    [SerializeField] private Image[] rewardContent;
    private List<ResourceData> resourceDatas = new List<ResourceData>();
    private WaitForSeconds wait = new WaitForSeconds(1f);

    public bool CheckForDailyReward()
    {
        if (GameSystem.userdata.loginDay < 7 && DateTime.Now.Ticks - GameSystem.userdata.lastLoginTime > TimeSpan.TicksPerDay)
        {
            gameObject.SetActive(true);
            StartCoroutine(Login());
            return true;
        }
        else if (GameSystem.userdata.loginDay > 7 || DateTime.Now.Ticks - GameSystem.userdata.lastLoginTime < TimeSpan.TicksPerDay)
        {
            gameObject.SetActive(false);
            rewardContent[GameSystem.userdata.loginDay - 1].gameObject.GetChildComponent<Image>("Hightlight").gameObject.SetActive(false);
            rewardContent[GameSystem.userdata.loginDay - 1].sprite = disableImg;
            for (int i = 0; i < rewardContent.Length; i++)
            {
                var highlightImg = rewardContent[i].gameObject.GetChildComponent<Image>("Hightlight");
                highlightImg.gameObject.SetActive(GameSystem.userdata.loginDay == i);
            }
        }
        return false;
    }

    private IEnumerator Login()
    {
        var userData = GameSystem.userdata;
        if (DateTime.Now.Ticks - userData.lastLoginTime > TimeSpan.TicksPerDay)
        {
            userData.lastLoginTime = DateTime.Now.Ticks;
            for (int i = 0; i < rewardContent.Length; i++)
            {
                var highlightImg = rewardContent[i].gameObject.GetChildComponent<Image>("Hightlight");
                highlightImg.gameObject.SetActive(userData.loginDay - 1 == i);
                if (i < userData.loginDay - 1) rewardContent[i].sprite = disableImg;
            }
            RecieveReward(userData.loginDay);          
            GameSystem.SaveUserDataToLocal();
            yield return wait;
            ResourcesGain.Instance.DisplayResources(resourceDatas);
            rewardContent[userData.loginDay - 1].gameObject.GetChildComponent<Image>("Hightlight").gameObject.SetActive(false);
            rewardContent[userData.loginDay - 1].sprite = disableImg;
            userData.loginDay++;
        }
    }

    private void RecieveReward(int day)
    {
        resourceDatas.Clear();
        var userData = GameSystem.userdata;
        switch (day)
        {
            case 1:
                userData.diamond += 50;
                resourceDatas.Add(new ResourceData()
                {
                    amount = 50,
                    sprite = Home.Instance.icons["Diamond"]
                });
                break;
            case 2:
                userData.greenLeaf += 5;
                resourceDatas.Add(new ResourceData()
                {
                    amount = 5,
                    sprite = Home.Instance.icons["GreenLeaf"]
                });
                break;
            case 3:
                userData.diamond += 100;
                resourceDatas.Add(new ResourceData()
                {
                    amount = 100,
                    sprite = Home.Instance.icons["Diamond"]
                });
                break;
            case 4:
                userData.buff[BuffType.Meteor] += 3;
                resourceDatas.Add(new ResourceData()
                {
                    amount = 500,
                    sprite = Home.Instance.icons["Meteor"]
                });
                break;
            case 5:
                userData.buff[BuffType.Heal] += 3;
                resourceDatas.Add(new ResourceData()
                {
                    amount = 500,
                    sprite = Home.Instance.icons["Heal"]
                });
                break;
            case 6:
                userData.buff[BuffType.Wind] += 3;
                resourceDatas.Add(new ResourceData()
                {
                    amount = 500,
                    sprite = Home.Instance.icons["Wind"]
                });
                break;
            case 7:
                var monsterData = DataManager.Instance.dicMonsterAIs["E18"].monsterData;
                monsterData.damage = monsterData.baseDamge;
                monsterData.maxhp = monsterData.baseHp;
                monsterData.level = 1;
                monsterData.rarity = monsterData.baseRarity;
                GameSystem.userdata.unlockHeroDatas.Add("E18", monsterData);
                Home.Instance.Shop.popupUnlock.ShowUnlockHero("E18");
                userData.gold += 3000;
                userData.energy += 5;
                userData.diamond += 100;
                resourceDatas.Add(new ResourceData()
                {
                    sprite = DataManager.Instance.dicMonsterAIs["E18"].portrait
                });
                resourceDatas.Add(new ResourceData()
                {
                    amount = 3000,
                    sprite = Home.Instance.icons["Coin"]
                });
                resourceDatas.Add(new ResourceData()
                {
                    amount = 3,
                    sprite = Home.Instance.icons["GoldLeaf"]
                });
                resourceDatas.Add(new ResourceData()
                {
                    amount = 100,
                    sprite = Home.Instance.icons["Diamond"]
                });
                break;
            default:
                return;
        }
    }

    public void CloseDailyPopup()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
        if (GameSystem.userdata.currentLevel < 5 || /*!GameSystem.userdata.doneDragTutorial ||*/ GameSystem.userdata.receivedStartHero) return;
        selectHeroGift.SetActive(true);
    }
}

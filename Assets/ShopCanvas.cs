using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class ShopCanvas : MonoBehaviour
{
    public GameObject heroGroup;
    public GameObject giftGroup;
    public PopupUnlockHero popupUnlock;
    public List<HeroSaleData> heroSaleDatas = new List<HeroSaleData>();
    public Image imgBoughtNoAds;
    public Image imgBoughVip;

    private void Start()
    {
        CreateSaleData();
        popupUnlock.gameObject.SetActive(false);
        OpenHeroGroup();
        UpdateDisplay();
    }

    private void CreateSaleData()
    {
        var listSaleHero = DataManager.Instance.saleHero;
        var monsterDic = DataManager.Instance.dicMonsterAIs;
        var userHeros = GameSystem.userdata.unlockedHeros;

        var newListSaleHero = new List<string>();
        newListSaleHero.AddRange(listSaleHero);
        
        for (int i = 0; i < userHeros.Count; i++)
        {
            if (newListSaleHero.Contains(userHeros[i]))
                newListSaleHero.Remove(userHeros[i]);
        }

        for (int i = 0; i < newListSaleHero.Count; i++)
        {
            var monsterData = monsterDic[newListSaleHero[i]].monsterData;
            heroSaleDatas.Add(new HeroSaleData()
            {
                heroName = newListSaleHero[i],
                currencyType = monsterData.saleCurrency,
                price = monsterData.shopPrice
            });
        }

        var content = heroGroup.GetChildComponent<Transform>("Content");
        var heroSaleObj = Resources.Load<HeroSale>("Button/HeroSale");

        for (int i = 0; i < heroSaleDatas.Count; i++)
        {
            var heroSale = Instantiate(heroSaleObj, content);
            heroSale.Init(heroSaleDatas[i]);
        }
    }

    public void GetEnergy()
    {
        GameSystem.userdata.energy += 5f;
        GameSystem.SaveUserDataToLocal();
        ResourcesGain.Instance.DisplayResources(new List<ResourceData>
        {
            new ResourceData()
            {
                sprite = Home.Instance.icons["Energy"],
                amount = 5
            }
        });
    }

    public void GetGem()
    {
        GameSystem.userdata.diamond += 10f;
        GameSystem.SaveUserDataToLocal();
        ResourcesGain.Instance.DisplayResources(new List<ResourceData>
        {
            new ResourceData()
            {
                sprite = Home.Instance.icons["Diamond"],
                amount = 10
            }
        });
    }

    public void BuyGreenLeaf()
    {
        var userData = GameSystem.userdata;
        if(userData.gold < 1000)
        {
            Home.Instance.Waring(Home.Instance.warningMessge, "Not enough Coin");
            return;
        }
        userData.gold -= 1000;
        userData.greenLeaf++;
        GameSystem.SaveUserDataToLocal();

        ResourcesGain.Instance.DisplayResources(new List<ResourceData>
        {
            new ResourceData()
            {
                sprite = Home.Instance.icons["GreenLeaf"],
                amount = 1
            }
        });
    }

    public void BuyGoldLeaf()
    {
        var userData = GameSystem.userdata;
        if(userData.diamond < 10)
        {
            Home.Instance.Waring(Home.Instance.warningMessge, "Not enough Diamond");
            return;
        }
        userData.diamond -= 10;
        userData.goldLeaf++;
        GameSystem.SaveUserDataToLocal();

        ResourcesGain.Instance.DisplayResources(new List<ResourceData>
        {
            new ResourceData()
            {
                sprite = Home.Instance.icons["GoldLeaf"],
                amount = 1
            }
        });
    }

    public void BuyBuff(int buffId)
    {       
        var userData = GameSystem.userdata;
        if (userData.gold < 1000)
        {
            Home.Instance.Waring(Home.Instance.warningMessge, "Not enough Coin");
            return;
        }
        userData.gold -= 1000;
        userData.buff[(BuffType)buffId]++;
        GameSystem.SaveUserDataToLocal();

        ResourcesGain.Instance.DisplayResources(new List<ResourceData>
        {
            new ResourceData()
            {
                sprite = Home.Instance.icons[((BuffType)buffId).ToString()],
                amount = 1
            }
        });
    }

    private void CloseAll()
    {
        heroGroup.SetActive(false);
        giftGroup.SetActive(false);
    }

    public void OpenHeroGroup()
    {
        //CloseAll();
        heroGroup.SetActive(true);
    }
    public void OpenGiftGroup()
    {
        //CloseAll();
        giftGroup.SetActive(true);
    }

    public void UpdateDisplay()
    {
        bool boughNoAds = GameSystem.userdata.boughtItems.Contains(IAP_ID.no_ads.ToString());
        imgBoughtNoAds.gameObject.SetActive(boughNoAds);

        bool boughAllHero = GameSystem.userdata.boughtItems.Contains(IAP_ID.unlock_all_hero.ToString());
        imgBoughVip.gameObject.SetActive(boughAllHero);
    }
}

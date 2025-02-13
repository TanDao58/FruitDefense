using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Purchasing;
using System;
using UnityEngine.SceneManagement;

public enum IAP_ID { no_ads, vip1, unlock_all_hero, beginer }

namespace DarkcupGames {
    public class ShopIAPManager : MonoBehaviour {
        public static ShopIAPManager Instance;
        public static MyIAPManager iap;

        public ShopCanvas shop;
        public List<Sprite> sprNoAds;
        public List<Sprite> sprVipPack;
        public List<TextPricingIAP> textPrices;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (iap == null)
            {
                Init();
            }
        }

        public void Init()
        {
            iap = new MyIAPManager();
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.AddProduct("vip1", ProductType.NonConsumable);
            builder.AddProduct("unlock_all_hero", ProductType.NonConsumable);
            builder.AddProduct("no_ads", ProductType.NonConsumable);
            UnityPurchasing.Initialize(iap, builder);
        }

        public bool IsInitDone()
        {
            if (iap == null) return false;
            if (iap.prices == null) return false;
            return true;
        }

        public void BuyProduct(string productId, Action onComplete) {
            MyIAPManager.currentBuySKU = productId;
            iap.OnPurchaseClicked(productId, onComplete);
        }

        public void OnBuyComlete(string sku)
        {
            if (GameSystem.userdata.boughtItems == null)
            {
                GameSystem.userdata.boughtItems.Add(sku);
                GameSystem.SaveUserDataToLocal();
            } 
        }

        public void BuyNoAdsPackage()
        {
            string id = IAP_ID.no_ads.ToString();

            if (GameSystem.userdata.boughtItems == null) GameSystem.userdata.boughtItems = new List<string>();
            bool boughNoAds = GameSystem.userdata.boughtItems.Contains(id);
            if (boughNoAds) return;

            iap.OnPurchaseClicked(id, () =>
            {
                int DIAMOND_AMOUNT = 100;
                int GOLD_AMOUNT = 10000;

                if (GameSystem.userdata.boughtItems == null) GameSystem.userdata.boughtItems = new List<string>();
                if (GameSystem.userdata.boughtItems.Contains(id) == false)
                {
                    GameSystem.userdata.boughtItems.Add(id);
                    GameSystem.userdata.isVipMember = true;
                    GameSystem.userdata.diamond += DIAMOND_AMOUNT;
                    GameSystem.userdata.gold += GOLD_AMOUNT;
                    GameSystem.SaveUserDataToLocal();
                }
                List<ResourceData> list = new List<ResourceData>();
                list.Add(new ResourceData()
                {
                    sprite = sprNoAds[0],
                    amount = 1
                });
                list.Add(new ResourceData()
                {
                    sprite = sprNoAds[1],
                    amount = DIAMOND_AMOUNT
                });
                list.Add(new ResourceData()
                {
                    sprite = sprNoAds[2],
                    amount = GOLD_AMOUNT
                });
                ResourcesGain.Instance.DisplayResources(list);
                shop.UpdateDisplay();
            });
        }

        public void BuyUnlockAllHeroPackage()
        {
            if (GameSystem.userdata.boughtItems == null) GameSystem.userdata.boughtItems = new List<string>();
            bool boughAllHero = GameSystem.userdata.boughtItems.Contains(IAP_ID.unlock_all_hero.ToString());
            if (boughAllHero) return;

            string id = IAP_ID.unlock_all_hero.ToString();
            iap.OnPurchaseClicked(id, () =>
            {
                int DIAMOND_AMOUNT = 500;
                int GOLD_AMOUNT = 20000;

                if (GameSystem.userdata.boughtItems == null) GameSystem.userdata.boughtItems = new List<string>();
                if (GameSystem.userdata.boughtItems.Contains(id) == false)
                {
                    GameSystem.userdata.boughtItems.Add(id);
                    GameSystem.userdata.isVipMember = true;
                    GameSystem.userdata.diamond += DIAMOND_AMOUNT;
                    GameSystem.userdata.gold += GOLD_AMOUNT;
                    GameSystem.SaveUserDataToLocal();
                }
                var allHeroes = new List<string>();
                allHeroes.AddRange(DataManager.Instance.allyNames);
                allHeroes.Remove("E18");
                for (int i = 0; i < allHeroes.Count; i++)
                {
                    if (GameSystem.userdata.unlockedHeros.Contains(allHeroes[i]) == false)
                    {
                        GameSystem.userdata.unlockedHeros.Add(allHeroes[i]);
                    }
                }
                GameSystem.SaveUserDataToLocal();
                List<ResourceData> list = new List<ResourceData>();
                list.Add(new ResourceData()
                {
                    sprite = sprVipPack[0],
                    amount = 1
                });
                list.Add(new ResourceData()
                {
                    sprite = sprVipPack[1],
                    amount = 1
                });
                list.Add(new ResourceData()
                {
                    sprite = sprVipPack[2],
                    amount = DIAMOND_AMOUNT
                });
                list.Add(new ResourceData()
                {
                    sprite = sprVipPack[3],
                    amount = GOLD_AMOUNT
                });
                ResourcesGain.Instance.DisplayResources(list);
                shop.UpdateDisplay();
                LeanTween.delayedCall(2f, () =>
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                });
            });
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DarkcupGames {
    public class ShopItemData {
        public string shopId;
        public string itemName;
        public string diamondPrice;
        public string currency;
        public Sprite demoSprite;
        public string description;
        public System.Action btnBuy;
        public System.Action btnUse;
        public bool unlocked;
    }

    public class ShopManager : MonoBehaviour {
        public UIUpdater uiUpdater;

        public List<Sprite> shurikenSprites;
        public SpriteRenderer razorRenderer;

        public ShopIAPManager shop;

        List<ShopItemData> datas;

        private void Start() {
            Init();
        }

        public void Init() {
            datas = new List<ShopItemData>();

            for (int i = 0; i < 3; i++) {   
                string key = "suriken" + i;
                string sku = "id_suriken_" + (i + 1);

                int index = i;

                datas.Add(new ShopItemData()
                {
                    itemName = "Shuriken " + i,
                    currency = "diamond",
                    demoSprite = shurikenSprites[i],
                    diamondPrice = ((i + 1) * 1000).ToString(),
                    description = "",
                    btnBuy = () =>
                    {
                        Debug.Log("You will buy " + key);
                        BuyItem(sku);
                    },
                    btnUse = () =>
                    {
                        Debug.Log("Use " + key);
                        razorRenderer.sprite = shurikenSprites[index];
                        gameObject.SetActive(false);
                    },
                    unlocked = GameSystem.userdata.boughtItems.Contains(sku)
                }); 
            }

            List<int> prices1 = new List<int> { 1, 1, 3, 5, 10 };

            for (int i = 0; i < prices1.Count; i++) {
                int amount = (prices1[i] + 1) * 60;

                string sku = "sub_"+(i+1)+"_week";

                datas.Add(new ShopItemData() {
                    itemName = amount + " diamonds every day",
                    currency = "dolar",
                    demoSprite = Resources.Load<Sprite>("diamond"),
                    diamondPrice = prices1[i] + "$",
                    description = "Renew weekly",
                    btnBuy = () =>
                    {
                        Debug.Log("You will buy " + sku);
                        BuyItem(sku);
                    },
                    btnUse = () =>
                    {
                        Debug.Log("Use " + sku);
                        //razorRenderer.sprite = shurikenSprites[index];
                        //gameObject.SetActive(false);
                    },
                    unlocked = GameSystem.userdata.boughtItems.Contains(sku)
                });
            }

            List<int> prices2 = new List<int> { 3, 5, 5, 10, 15 };

            for (int i = 0; i < prices2.Count; i++) {
                int amount = (prices2[i] + 1) * 60;

                string sku = "sub_" + (i + 1) + "_month";

                datas.Add(new ShopItemData() {
                    itemName = amount + " diamonds every day",
                    currency = "dolar",
                    demoSprite = Resources.Load<Sprite>("manydiamonds"),
                    diamondPrice = prices2[i] + "$",
                    description = "Renew monthly",
                    btnBuy = () =>
                    {
                        Debug.Log("You will buy " + sku);
                        BuyItem(sku);
                    },
                    btnUse = () =>
                    {
                        Debug.Log("Use " + sku);
                        //razorRenderer.sprite = shurikenSprites[index];
                        //gameObject.SetActive(false);
                    },
                    unlocked = GameSystem.userdata.boughtItems.Contains(sku)
                });
            }

            //for (int i = 0; i < shurikenSprites.Count; i++) {
            //    datas.Add(new ShopItemData() {
            //        itemName = "Shuriken " + i,
            //        currency = "diamond",
            //        demoSprite = shurikenSprites[i],
            //        price = ((i + 1) * 1000).ToString(),
            //        description = "This is some description"
            //    });
            //}

            //for (int i = 0; i < 4; i++) {
            //    int amount = (i + 1) * 1000;
            //    int price = (i + 1) * 10;

            //    datas.Add(new ShopItemData() {
            //        itemName = amount + " diamonds",
            //        currency = "dolar",
            //        demoSprite = Resources.Load<Sprite>("diamond"),
            //        price = price.ToString(),
            //        description = ""
            //    });
            //}

            uiUpdater.UpdateChildUI(datas);
        }

        public void BuyItem(string sku)
        {
            Debug.Log("You Buy item!");

            shop.BuyProduct(sku, ()=> {
                GameSystem.userdata.boughtItems.Add(sku);
                GameSystem.SaveUserDataToLocal();

                Init();
            });
        }

        public void BuyShuriken(string id, int price)
        {
            Debug.Log("You Buy item!");
            GameSystem.userdata.diamond -= price;
            GameSystem.userdata.boughtItems.Add(id);
            GameSystem.SaveUserDataToLocal();

            Init();
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public enum CurrencyType
{
    Coin, Diamond, GreenLeaf, GoldLeaf, None
}
[System.Serializable]
public class HeroSaleData
{
    public string heroName;
    public CurrencyType currencyType;
    public float price;
}

public class HeroSale : MonoBehaviour
{
    public string heroID;
    public LocalizeStringEvent localize;
    private SkeletonGraphic graphic;
    private TextMeshProUGUI heroName;
    private TextMeshProUGUI decripsion;
    private TextMeshProUGUI value;
    private Image rarity;
    private Image currencyImg;
    private HeroSaleData data;

    public void Init(HeroSaleData data)
    {
        this.data = data;
        graphic = gameObject.GetChildComponent<SkeletonGraphic>("HeroImg");
        heroName = gameObject.GetChildComponent<TextMeshProUGUI>("Name");
        decripsion = gameObject.GetChildComponent<TextMeshProUGUI>("Decripsion");
        rarity = gameObject.GetChildComponent<Image>("Rank");
        currencyImg = gameObject.GetChildComponent<Image>("Price/Icon");
        value = gameObject.GetChildComponent<TextMeshProUGUI>("Price/Value");

        heroID = data.heroName;
        var hero = DataManager.Instance.dicMonsterAIs[data.heroName];
        var heroSkeleton = hero.GetComponentInChildren<SkeletonAnimation>();

        graphic.Clear();
        graphic.skeletonDataAsset = heroSkeleton.skeletonDataAsset;
        graphic.Initialize(true);
        heroName.text = hero.monsterData.nickName;
        decripsion.text = hero.monsterData.decripsion;
        var imgPath = GeneralUltility.BuildString("Rank/", hero.monsterData.rarity.ToString());
        rarity.sprite = Resources.Load<Sprite>(imgPath);
        currencyImg.sprite = Resources.Load<Sprite>(data.currencyType.ToString());
        currencyImg.SetNativeSize();
        value.text = GeneralUltility.BuildString("","x",hero.monsterData.shopPrice.ToString());
        localize.SetTable("New Table");
        localize.SetEntry(decripsion.text);
    }

    public void BuyHero()
    {
        var userHeros = GameSystem.userdata.unlockedHeros;

        bool EnoughMoney(CurrencyType currency)
        {
            switch (currency)
            {
                case CurrencyType.Coin:
                    if (GameSystem.userdata.gold >= data.price) return true;
                    else return false;
                case CurrencyType.Diamond:
                    if (GameSystem.userdata.diamond >= data.price) return true;
                    else return false;
                default: return false;
            }
        }
        if(EnoughMoney(data.currencyType))
        {
            if (data.currencyType == CurrencyType.Coin) GameSystem.userdata.gold -= data.price;
            if (data.currencyType == CurrencyType.Diamond) GameSystem.userdata.diamond -= data.price;
            if (!userHeros.Contains(heroID))
            {
                userHeros.Add(heroID);
                if(!GameSystem.userdata.unlockHeroDatas.ContainsKey(heroID))
                {
                    var monsterData = DataManager.Instance.dicMonsterAIs[heroID].monsterData;
                    monsterData.damage = monsterData.baseDamge;
                    monsterData.maxhp = monsterData.baseHp;
                    monsterData.level = 1;
                    monsterData.rarity = monsterData.baseRarity;
                    GameSystem.userdata.unlockHeroDatas.Add(heroID, monsterData);
                }
            }
            var shop = Home.Instance.Shop;
            if (shop.heroSaleDatas.Contains(data)) shop.heroSaleDatas.Remove(data);
            GameSystem.SaveUserDataToLocal();
            shop.popupUnlock.ShowUnlockHero(data.heroName);
            shop.popupUnlock.GetComponent<DarkcupGames.UIEffect>().DoEffect();
            Home.Instance.Team.UpdateDisplay();
            Destroy(gameObject);
        }        
        else
        {
            Home.Instance.Waring(Home.Instance.warningMessge ,"You don't have enough " + data.currencyType.ToString());
        }
    }

    //public void UpdateLocalize()
    //{
    //    LeanTweenExt.LeanDelayedCall(gameObject, 1f, () =>
    //    {
    //        switch (decripsion.text)
    //        {
    //            case "A great fan of Afro-samurai. Attack with a combo of 3 hits.":
    //                decripsion.text = "Fan cuồng Samura. Tấn công bằng song kiếm với 3 đòn liên tục";
    //                break;
    //            case "Always come in pairs. The small one will get its revenge once the big one falls.":
    //                decripsion.text = "Luôn đi cùng đứa em nhỏ. Người em sẽ trả thù bất kì kẻ nào hại anh mình";
    //                break;
    //            case "Pop the enemy to death with a corn slingshot.":
    //                decripsion.text = "Dùng ná để bắn những hạt bắp vào kẻ thù";
    //                break;
    //            case "Shall heal the one in need with its mighty grapes.":
    //                decripsion.text = "Trị thương những đồng đội bằng những trái nho mini";
    //                break;
    //            case "This pomegranate goes Pow. Create an explosion with its mighty stomp.":
    //                decripsion.text = "Trái lựu đô vật. Có thể tạo ra động đất với 1 cú nhảy";
    //                break;
    //            case "Big fat Durian warlord. Summon its loyal army of 4 small Durian warriors. ":
    //                decripsion.text = "Đại tướng sầu riêng. Chỉ huy một bình đoàn những múi sầu riêng non";
    //                break;
    //            case "Take off part of its shell as a mighty shield. Might go well with The Grape One.":
    //                decripsion.text = "Dùng vỏ của mình để làm khiên che chắn cho đồng đội";
    //                break;
    //            case "This Avocado go Ka-Boom.":
    //                decripsion.text = "Đừng bơ - Nổ đấy";
    //                break;
    //            case "A mama tomato. When dead will explose and give birth to 3 baby tomato ":
    //                decripsion.text = "Một cà chua mẹ. Rút lui bằng cách tạo ra một vụ nổ lớn và 3 đứa con sẽ thay mẹ làm nốt công việc";
    //                break;
    //            case "A living lemon battery. Stun enemy with a lightning AOE attack.":
    //                decripsion.text = "Quả chanh tích điện. Giật tất cả kẻ thù ở gần";
    //                break;
    //            case "Pineapple whose arrow can penatrate enemy's thick metal armor.":
    //                decripsion.text = "Mũi tên của trái dứa này sẽ xuyên thủng những kẻ thủ có lớp giáp cứng nhất";
    //                break;
    //            case "A Mystical fruit that can turn into any fruit.":
    //                decripsion.text = "Một trái cây bí ẩn. Nó có thể là gì?";
    //                break;
    //        }
    //        localize.RefreshString();
    //    });
        
    //}
}

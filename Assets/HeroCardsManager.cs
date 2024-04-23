//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;
//public class HeroCardsManager : MonoBehaviour
//{
//    public static HeroCardsManager instance;
//    public bool inUpgrade;
//    public List<HeroCard> heroCards = new List<HeroCard>();
//    public EmptySlotManager emptySlotManager;
//    private Dictionary<string, HeroCard> dicCards;

//    private void Awake()
//    {
//        instance = this;
//    }
//    void Start()
//    {
//        Debug.Log("Herocard manager = " + gameObject.name);
//        heroCards = GetComponentsInChildren<HeroCard>().ToList();
//        dicCards = new Dictionary<string, HeroCard>();
//        for (int i = 0; i < heroCards.Count; i++) {
//            dicCards.Add(heroCards[i].heroName, heroCards[i]);
//        }
//        var savedTeam = GameSystem.userdata.selectedTeam;
//        if (savedTeam != null && savedTeam.Count > 0 && savedTeam.Count <= Constants.MAX_HERO) {
//            for (int i = 0; i < savedTeam.Count; i++) {
//                dicCards[savedTeam[i]].PutInEmptySlot();
//            }
//        }
//    }

//    public void UpdateCurrentSelectedCards()
//    {
//        foreach(HeroCard herocard in heroCards)
//        {
//            if(herocard.gameObject.name == ChooseHero.instance.currentSelect.name)
//            {
//                continue;
//            }
//            herocard.chosen = false;
//        }
//    }
//}


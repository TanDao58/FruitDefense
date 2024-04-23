using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;
using Spine.Unity;
using TMPro;
using DarkcupGames;

public class SelectHeroManager : MonoBehaviour
{
    public static SelectHeroManager Instance;
    [System.NonSerialized] public List<HeroCard> heroCards;
    public EmptySlotManager emptySlotManager;
    public GameObject popupWatchAdsEnergy;
    public Transform fadeBlack;
    public Transform heroCardParent;
    public SkeletonGraphic heroAnim;
    public TextMeshProUGUI txtHeroName;
    public TextMeshProUGUI txtHeroDiscription;
    public TextMeshProUGUI warningText;
    private Dictionary<string, HeroCard> dicCards;

    private void Awake()
    {
        Instance = this;
        fadeBlack.transform.localScale = Vector3.zero;
        fadeBlack.gameObject.SetActive(false);
    }
    private void Start()
    {
        emptySlotManager = GetComponentInChildren<EmptySlotManager>();
        var savedTeam = GameSystem.userdata.selectedTeam;
        if (savedTeam != null && savedTeam.Count > 0 && savedTeam.Count <= Constants.MAX_HERO)
        {
            for (int i = 0; i < savedTeam.Count; i++)
            {
                if (!dicCards[savedTeam[i]].IsLocked && GameSystem.userdata.unlockedHeros.Contains(savedTeam[i]))
                    dicCards[savedTeam[i]].PutInEmptySlot();
            }
            ShowInfo(savedTeam.RandomElement());
        }
        else
        {
            ShowInfo(GameSystem.userdata.unlockedHeros.RandomElement());
        }
    }

    public void StartGame()
    {
        GameSystem.userdata.unlockedHeros = GameSystem.userdata.unlockedHeros.Distinct().ToList();
        //if (GameSystem.userdata.unlockedHeros.Count >= Constants.MAX_HERO && emptySlotManager.chosenMonsters.Count < Constants.MAX_HERO)
        if (emptySlotManager.chosenMonsters.Count < 1)
        {
            if (warningText.gameObject.activeInHierarchy) return;
            warningText.gameObject.SetActive(true);
            var sq = DOTween.Sequence();
            sq.Append(warningText.transform.DOMoveY(1f, 1f));
            sq.AppendInterval(1f);
            sq.Append(warningText.DOFade(0f, 2f).OnComplete(() =>
            {
                warningText.color = new Color(1, 1, 1, 1);
                warningText.transform.DOMoveY(0f, 0.1f);
                warningText.gameObject.SetActive(false);
            }));         
            return;
        }
        //check for energy
        if (GameSystem.userdata.energy <= 0)
        {
            popupWatchAdsEnergy.gameObject.SetActive(true);
            EasyEffect.Appear(popupWatchAdsEnergy.gameObject, 0.8f, 1f, speed: 0.1f);
            if (FirebaseManager.Instance)
            {
                FirebaseManager.Instance.LogAction("not enough energy");
            }
            return;
        }
        else
        {
            GameSystem.userdata.energy -= Constants.ENERGY_COST_EACH_PLAY;
        }
        GameSystem.userdata.trialHeroName = "";
        StartGameImediately();
    }

    public void StartGameImediately()
    {
        GameSystem.chosenMonsters = new List<string>();
        for (int i = 0; i < emptySlotManager.chosenMonsters.Count; i++)
        {
            GameSystem.chosenMonsters.Add(emptySlotManager.chosenMonsters[i].heroName);
        }
        GameSystem.userdata.selectedTeam = GameSystem.chosenMonsters;
        FadeBlackToGameplay();
    }

    public void FadeBlackToGameplay()
    {
        fadeBlack.gameObject.SetActive(true);
        fadeBlack.transform.localScale = Vector3.zero;
        fadeBlack.transform.DOScale(Vector3.one * 30f, 1f).OnComplete(() =>
        {
            SceneManager.LoadScene(Constants.SCENE_GAMEPLAY);
        });
        GameSystem.SaveUserDataToLocal();
        if (GoogleAdMobController.Instance.bannerView != null)
        {
            GoogleAdMobController.Instance.bannerView.Hide();
        }
    }

    /// <summary>
    /// Luôn để child 0 của heroCardParent là active false để không bị destroy, transform này dùng để làm source khi instantiate
    /// </summary>
    public void CreateCards()
    {
        if (heroCards != null && heroCards.Count != 0) return;

        heroCards = new List<HeroCard>();
        dicCards = new Dictionary<string, HeroCard>();
        foreach (Transform item in heroCardParent)
        {
            if (item.gameObject.activeSelf == true)
            {
                Destroy(item.gameObject);
            }
        }
        HeroCard source = heroCardParent.GetChild(0).gameObject.GetComponent<HeroCard>();
        var names = DataManager.Instance.allyNames;
        for (int i = 0; i < names.Count; i++)
        {
            var heroCard = Instantiate(source, heroCardParent);
            heroCard.SetData(names[i]);
            heroCard.DisplayHero();
            heroCard.gameObject.SetActive(true);
            heroCard.siblingIndex = (i + 1);
            heroCards.Add(heroCard);
            dicCards.Add(names[i], heroCard);
            string heroName = names[i];
            heroCard.GetComponent<Button>().onClick.AddListener(() =>
            {
                ShowInfo(heroName);
            });
        }
    }

    public void UpdateDisplay()
    {
        if (heroCards == null || heroCards.Count == 0)
        {
            CreateCards();
        }
        for (int i = 0; i < heroCards.Count; i++)
        {
            heroCards[i].DisplayHero();
        }
    }

    public void ShowInfo(string heroName)
    {
        if (DataManager.Instance.dicMonsterAIs.ContainsKey(heroName) == false) return;
        var hero = DataManager.Instance.dicMonsterAIs[heroName];

        heroAnim.Clear();
        heroAnim.skeletonDataAsset = hero.GetComponentInChildren<SkeletonAnimation>().skeletonDataAsset;
        heroAnim.Initialize(true);
        txtHeroName.text = hero.monsterData.nickName;
        txtHeroDiscription.text = hero.monsterData.decripsion;
    }

    //public void UpdateTrialHero()
    //{
    //    if (GameSystem.userdata.trialEnabled == false)
    //    {
    //        GameSystem.userdata.trialHeroName = DataManager.Instance.
    //    }
    //}
}

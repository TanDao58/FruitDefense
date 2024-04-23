using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HeroCard : MonoBehaviour
{
    public const float SIZE_IN_SLOT = 0.8f;

    [SerializeField] private Sprite[] crystalSprites;    
    [SerializeField]public Canvas canvas;

    private Image heroImg;
    private Image selectImage;
    private TextMeshProUGUI priceText;
    private Image crystalImg;    
    private Image lockImg;
    private Button selectButton;
    private GameObject mask;
    private GameObject priceGroup;
    private MonsterAI monster;
    private Vector3 baseScale;
    public EmptySlot takenSlot;
    private Transform defaultParent;
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI trailTxt;
    public int siblingIndex;
    public bool choose;
    public bool chosen;
    public bool inUpgrade;
    public Button currentSelectButton;
    public Button equipButton;
    private bool isLocked;
    public EmptySlotManager emptySlotManager;
    public string heroName;
    public MonsterAI Monster => monster;
    public bool HasChoose => choose;
    public Transform  slotTransform;
    public bool isEnemyCard;
    public GameObject watchAdsGroup;
    private bool inited = false;
    private bool trial = false;

    public bool IsLocked
    {
        get
        {
            return isLocked;
        }
        set
        {
            if(value == true)
            {
                LockCard();                
            }
            if (value == false)
            {
                UnlockCard();
            }
            isLocked = value;
        }
    }

    public bool Trial
    {
        get { return trial; }
        set
        {
            trial = value;
            if (trailTxt) trailTxt.gameObject.SetActive(value);
            if (watchAdsGroup) watchAdsGroup.SetActive(value);
        }
    }

    void Awake()
    {
        if (!inited) Init();
    }

    public void Init()
    {
        inited = true;
        defaultParent = transform.parent;
        baseScale = transform.localScale;
        monster = Resources.Load<MonsterAI>(heroName);
        selectButton = GetComponent<Button>();
        mask = gameObject.GetChildComponent<GameObject>("Mask");
        priceGroup = gameObject.GetChildComponent<GameObject>("PriceGroup");
        trailTxt = gameObject.GetChildComponent<TextMeshProUGUI>("TrialTxt");
        heroImg = mask.GetChildComponent<Image>("HeroImg");
        lockImg = mask.GetChildComponent<Image>("LockImage");
        priceText = priceGroup.GetChildComponent<TextMeshProUGUI>("PriceTxt");
        crystalImg = priceGroup.GetChildComponent<Image>("Crystal");
        canvasGroup = GetComponent<CanvasGroup>();

        if (trailTxt) trailTxt.gameObject.SetActive(false);
        if (watchAdsGroup) watchAdsGroup.gameObject.SetActive(false);
    }

    public void Start() {
        DisplayHero();
    }

    public void SetData(string heroName)
    {
        this.heroName = heroName;
        if (DataManager.Instance.dicMonsterAIs.ContainsKey(heroName) == false)
        {
            Debug.LogError($"key not found in dic {heroName}");
            return;
        }
        monster = DataManager.Instance.dicMonsterAIs[heroName];
    }

    public void DisplayHero()
    {
        if (!inited) Init();
        heroImg.sprite = monster.portrait;
        heroImg.SetNativeSize();
        bool unlocked;
        if (GameSystem.userdata.seenEnemies == null) GameSystem.userdata.seenEnemies = new List<string>();
        if (isEnemyCard)
        {
            unlocked = GameSystem.userdata.seenEnemies.Contains(heroName);
        }
        else
        {
            unlocked = GameSystem.userdata.unlockedHeros.Contains(heroName) || GameSystem.userdata.trialHeroName.Equals(heroName);
        }
        IsLocked = !unlocked;
    }

    private void LockCard()
    {
        heroImg.color = Color.gray;
        lockImg.gameObject.SetActive(true);
        crystalImg.sprite = crystalSprites[1];
        priceText.text = "??";
        selectButton.enabled = false;
    }

    private void UnlockCard()
    {
        heroImg.color = Color.white;
        lockImg.gameObject.SetActive(false);
        crystalImg.sprite = crystalSprites[0];
        priceText.text = monster.moneyCost.ToString();
        selectButton.enabled = true;
    }

    public void Choose()
    {
        if (watchAdsGroup.gameObject.activeInHierarchy)
        {
            Home.Instance.ShowTryCardAdsPopup(heroName);
            return;
        }
        if(!choose)
        {
            foreach (var slot in emptySlotManager.emptySlots)
            {
                if (slot.ChosenMonster == null)
                {
                    transform.SetParent(canvas.transform);
                    slot.ChosenMonster = this;
                    transform.DOMove(slot.transform.position, 0.5f);
                    transform.DOScale(slot.transform.localScale, 0.5f).OnComplete(() =>
                    {
                        slotTransform = slot.transform;
                        transform.SetParent(slot.transform);
                        transform.DOScale(slot.transform.localScale  * SIZE_IN_SLOT, 0.2f);
                    });
                    choose = true;
                    takenSlot = slot;
                    break;
                }
            }
        }
        else
        {
            transform.SetParent(canvas.transform);
            transform.DOMove(defaultParent.parent.parent.position, 0.5f);
            LeanTween.value(1f, 0f, 0.5f).setOnUpdate((y) => canvasGroup.alpha = y).setOnComplete(() => canvasGroup.alpha = 1f) ;
            transform.DOScale(new Vector3(1f,1f,1f), 0.5f).OnComplete(()=> 
            {
                transform.SetParent(defaultParent);
                transform.SetSiblingIndex(siblingIndex);
                transform.localScale = baseScale;
            });
            choose = false;
            takenSlot.ChosenMonster = null;
            takenSlot = null;
        }
    }

    public void PutInEmptySlot() {
        foreach (var slot in emptySlotManager.emptySlots) {
            if (slot.ChosenMonster == null) {
                slot.ChosenMonster = this;
                slotTransform = slot.transform;
                transform.position = slot.transform.position;
                transform.SetParent(slot.transform);
                var rect = transform.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                transform.localPosition = Vector2.zero;
                transform.localScale = slot.transform.localScale * SIZE_IN_SLOT;
                choose = true;
                takenSlot = slot;
                break;
            }
        }
    }

    public void OnWatchAdsTrialCompleted()
    {
        watchAdsGroup.gameObject.SetActive(false);
    }
}
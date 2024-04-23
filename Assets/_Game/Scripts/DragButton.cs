using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;
public class DragButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public string prefabName;

    [SerializeField] AudioClip interactionSound;
    private TextMeshProUGUI priceText;
    private MonsterAI monster;
    private Image heroImg;
    private Image coolDownImage;
    private Image enableDragImage;
    private Image selectedImg;
    private Camera mainCamera;
    private Transform heroImgParent;
    private GameObject attackRange;
    private TextMeshProUGUI trialTxt;

    public Image SelectedImg => selectedImg;

    private void Awake() {
        heroImg = gameObject.GetChildComponent<GameObject>("Mask").GetChildComponent<Image>("heroImg");
        coolDownImage = gameObject.GetChildComponent<Image>("coolDownImage");
        enableDragImage = gameObject.GetChildComponent<Image>("enbleDragImage");
        selectedImg = gameObject.GetChildComponent<Image>("selectImg");
        priceText = gameObject.GetChildComponent<TextMeshProUGUI>("priceText");
        trialTxt = gameObject.GetChildComponent<TextMeshProUGUI>("TrialTxt");
        heroImgParent = heroImg.transform.parent;
        mainCamera = Camera.main;
        selectedImg.enabled = false;
        coolDownImage.fillAmount = 0;
        coolDownImage.gameObject.SetActive(false);
        attackRange = ObjectPool.Instance.GetGameObjectFromPool("AttackRange", transform.position);
        attackRange.SetActive(false);
    }

    private void Update()
    {
        enableDragImage.gameObject.SetActive(Gameplay.Intansce.MoneyUpdate.DP < monster.moneyCost);
        heroImg.color = coolDownImage.fillAmount == 0 ? Color.white : Color.gray;
        if (coolDownImage.fillAmount > 0)
        {
            coolDownImage.fillAmount -= Time.deltaTime / monster.cooldownTime;
        }
    }

    public void DislayHero(MonsterAI inputMonster, string monsterName, bool trial = false)
    {
        monster = inputMonster;
        prefabName = monsterName;
        heroImg.sprite = inputMonster.portrait;
        heroImg.SetNativeSize();
        priceText.text = inputMonster.moneyCost.ToString();
        trialTxt.gameObject.SetActive(trial);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Gameplay.Intansce.MoneyUpdate.DP < monster.moneyCost) return;
        heroImg.transform.SetParent(Gameplay.Intansce.canvas.transform);
        heroImg.transform.position = eventData.position;
        attackRange.SetActive(true);
        attackRange.transform.localScale = Vector3.one * monster.monsterData.visionRange;
        var pos = mainCamera.ScreenToWorldPoint(eventData.position);
        pos.z = 0;
        attackRange.transform.position = pos;
        Gameplay.Intansce.dangerZones[0].SetActive(true);
        Gameplay.Intansce.dangerZones[1].SetActive(true);
        Gameplay.Intansce.blueZones.SetActive(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SpawnMonster(eventData.position);
        attackRange.SetActive(false);
    }

    public void SpawnMonster(Vector3 screenPos)
    {
        Gameplay.Intansce.dangerZones[0].SetActive(false);
        Gameplay.Intansce.dangerZones[1].SetActive(false);
        Gameplay.Intansce.blueZones.SetActive(false);
        heroImg.transform.position = heroImgParent.position;
        heroImg.transform.SetParent(heroImgParent);
        SelectManagerGameplay.Instance.ResetSelect();
        SFXSystem.Instance.Play(interactionSound);
        RectTransform parenRect = transform.parent.GetComponent<RectTransform>();
        bool insideCardArea = RectTransformUtility.RectangleContainsScreenPoint(parenRect, screenPos);
        if (insideCardArea) return;

        Vector3 pos = mainCamera.ScreenToWorldPoint(screenPos);
        pos.z = 0;
        if (coolDownImage.fillAmount > 0 || pos.y > Gameplay.Intansce.dragLimitZone.y /*|| SelectManagerGameplay.Instance.spawnedHero.Count >= 10*/) return;

        Gameplay.Intansce.dangerZones[0].SetActive(false);
        Gameplay.Intansce.dangerZones[1].SetActive(false);
        Gameplay.Intansce.blueZones.SetActive(false);

        if (CheckMaxHero())
        {
            //var number = ObjectPool.Instance.GetGameObjectFromPool("Number/DamgeNumber", pos);
            //var text = number.GetComponentInChildren<TextMeshPro>();
            //text.text = "Max Amount";
            //text.color = Color.white;
            var number = ObjectPool.Instance.GetGameObjectFromPool("Number/DamgeNumber", pos);
            var numberMesh = number.GetComponent<DamgeNumber>();
            numberMesh.ShowText("Max Amount", Color.white);
            return;
        }
        if (coolDownImage.fillAmount > 0) return;
        if (Gameplay.Intansce.MoneyUpdate.DP - monster.moneyCost >= 0)
        {
            //var monsterAI = ObjectPool.Instance.GetGameObjectFromPool<MonsterAI>("E18", pos);
            var monsterAI = ObjectPool.Instance.GetGameObjectFromPool<MonsterAI>(prefabName, pos);
            monsterAI.enabled = true;
            monsterAI.IsEnemy = false;
            SelectManagerGameplay.Instance.spawnedHero.Add(monsterAI);
            Gameplay.Intansce.UpdateHeroAmount();
            EasyEffect.Appear(monsterAI.gameObject, 0f, 1f);
            Gameplay.Intansce.DoEffectFall(monsterAI.gameObject);
            Gameplay.Intansce.MoneyUpdate.DP -= monster.moneyCost;
            coolDownImage.gameObject.SetActive(true);
            coolDownImage.fillAmount = 1;
            ObjectPool.Instance.GetGameObjectFromPool("Vfx/HeroAppear", pos - new Vector3(0f, 0.5f));
        }
        if (!GameSystem.userdata.doneDragTutorial)
        {
            GameSystem.userdata.doneDragTutorial = true;
            FindObjectOfType<DragTutorial>().gameObject.SetActive(false);
            EnemySpawner.Instance.gameObject.SetActive(true);
            GameSystem.SaveUserDataToLocal();
        }
    }

    private bool CheckMaxHero()
    {
        //bool isMaxed = false;
        //List<GameObject> recheckList = new List<GameObject>();
        //recheckList = GameObject.FindGameObjectsWithTag("Ally").ToList();
        //if(recheckList.Count >= 10)
        //{
        //    isMaxed = true;
        //}
        //return isMaxed; 
        var currentHero = SelectManagerGameplay.Instance.spawnedHero;
        for (int i = 0; i < currentHero.Count; i++)
        {
            if(currentHero[i].IsDead() && currentHero.Contains(currentHero[i]))
            {
                currentHero.Remove(currentHero[i]);
            }
        }
        return currentHero.Count == Constants.MAX_HERO_INGAME;
    }


    public void Selected()
    {
        if (coolDownImage.fillAmount > 0) return;
        if (Gameplay.Intansce.MoneyUpdate.DP < monster.moneyCost) return;
        SelectManagerGameplay.Instance.ResetSelect();
        SFXSystem.Instance.Play(interactionSound);
        selectedImg.enabled = true;
        SelectManagerGameplay.Instance.SetButton(this);
        Gameplay.Intansce.dangerZones[0].SetActive(true);
        Gameplay.Intansce.dangerZones[1].SetActive(true);
        Gameplay.Intansce.blueZones.SetActive(true);
    }
}

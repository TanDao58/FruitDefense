using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkcupGames;

public class ResourceData
{
    public Sprite sprite;
    public int amount;
}

public class ResourcesGain : MonoBehaviour
{
    public static ResourcesGain Instance;
    [SerializeField] private ResourcesCard[] resourcesCards;
    [SerializeField] private UIEffect uIEffect;
    [SerializeField] private RectTransform popupBody;

    private void Awake()
    {
        Instance = this;
    }

    public void DisplayResources(List<ResourceData> datas)
    {
        //resourcesCards = GetComponentsInChildren<ResourcesCard>(true);
        //for (int i = 0; i < resourcesCards.Length; i++)
        //{
        //    resourcesCards[i].Init();
        //}
        if (datas.Count > resourcesCards.Length)
        {
            Debug.LogError("Max 9 ResourceData can be displayed at once");
            return;
        }
        popupBody.gameObject.SetActive(true);        
        for (int i = 0; i < resourcesCards.Length; i++)
        {
            resourcesCards[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < datas.Count; i++)
        {
            resourcesCards[i].Dislay(datas[i]);
        }
        uIEffect.DoEffect(false);
    }
}

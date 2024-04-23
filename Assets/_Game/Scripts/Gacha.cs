using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DarkcupGames;
using DG.Tweening;

public enum PrizePool
{
    Radar,GemCollector,Sniper,Hero
}
public enum GachaType
{
    NormalGacha,SuperGacha,SupremeGacha
}
public abstract class Gacha : MonoBehaviour
{
    public GachaType type;
    protected List<PrizePool> prizePool = new List<PrizePool>();
    public List<Result> results = new List<Result>();
    protected int odd;

    protected virtual void Start()
    {
        prizePool = Enum.GetValues(typeof(PrizePool)).Cast<PrizePool>().ToList();
    }

    public abstract void OpenGachaOne();
    
    protected abstract PrizePool GetPrize();  

    protected void ShowResultOne()
    {
        var gachaResultPopup = Home.Instance.gachaResult;
        gachaResultPopup.SetActive(true);
        EasyEffect.Appear(gachaResultPopup, 0f, 1f,0.2f,1f,() => 
        {
            var gachaResult = gachaResultPopup.GetComponentInChildren<UIUpdater>();
            EasyEffect.Appear(gachaResult.gameObject, 0f, 1f, 0.2f, 1f);
            gachaResult.UpdateChildUI(results);
        });       
    }
}

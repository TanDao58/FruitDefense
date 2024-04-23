using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;
using UnityEngine.UI;
using DG.Tweening;

public class LuckySpin : MonoBehaviour
{
    const int GOLD = 0;
    const int DIAMOND = 1;
    const int ENERGY = 2;

    [SerializeField] private RotateLuckySpin rotate;
    [SerializeField] private CollectEffect collectEffect;
    [SerializeField] private Button btnSpin;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private List<Transform> destinations;

    private int spinTime = 1;
    private bool isSpining = false;
    private void OnEnable()
    {
        spinTime = 1;
        btnSpin.gameObject.SetActive(true);
    }
    public void Spin()
    {
        if (spinTime == 0) return;
        spinTime--;
        rotate.spinSpeed = 500f;
        rotate.onReceiveReward = ReceiveReward;
        rotate.enabled = true;
        btnSpin.gameObject.SetActive(false);
        isSpining = true;
    }

    public void ReceiveReward(Transform nearest)
    {
        void DoEffectCollect(int type)
        {
            Sprite effect = sprites[type];
            for (int i = 0; i < collectEffect.particles.Count; i++)
            {
                collectEffect.particles[i].GetComponent<Image>().sprite = effect;
            }
            collectEffect.destination = destinations[type];
            collectEffect.startPosition = nearest;
            collectEffect.DoEffect();
        }
        var resourceData = new ResourceData();
        switch (nearest.name)
        {
            case "gold":
                DoEffectCollect(GOLD);
                GameSystem.userdata.gold += 5;
                GameSystem.SaveUserDataToLocal();
                resourceData.sprite = Home.Instance.icons["Coin"];
                resourceData.amount = 5;
                break;
            case "diamond":
                DoEffectCollect(DIAMOND);
                GameSystem.userdata.diamond += 10;
                GameSystem.SaveUserDataToLocal();
                resourceData.sprite = Home.Instance.icons["Diamond"];
                resourceData.amount = 10;
                break;
            case "energy":
                DoEffectCollect(ENERGY);
                GameSystem.userdata.energy += 5;
                GameSystem.SaveUserDataToLocal();
                resourceData.sprite = Home.Instance.icons["Energy"];
                resourceData.amount = 5;
                break;
        }

        DOTween.Sequence().AppendInterval(3f).AppendCallback(() =>
        {
            EasyEffect.Disappear(gameObject, 1f, 0.8f,0.2f,1.2f,( )=> ResourcesGain.Instance.DisplayResources(new List<ResourceData>() 
            {
                resourceData
            }));
        });
        isSpining = false;
    }

    public void ShowLuckSpin()
    {
        EasyEffect.Appear(gameObject, 0.8f, 1f);
    }

    public void TryClosePopup()
    {
        if (isSpining) return;
        EasyEffect.Disappear(gameObject, 1f, 0.8f);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DarkcupGames{
public class CollectCoin : MonoBehaviour, IPointerDownHandler
{
    public int amount = 1;
    bool collected;

    private void OnEnable() {
        collected = false;
        var obj = gameObject;
        LeanTween.delayedCall(2.5f, () => {
            if (obj != null)
                Collect();
        });
    }

    public void OnPointerDown(PointerEventData eventData) {
        Collect();
    }

    GameObject star;

    public void Collect() {
        if (collected) return;
        collected = true;

        int spawnNumber = amount / 200 + 3;
        float ratio = ((float)amount / 10f);
        float size = Mathf.Lerp(0.1f, 0.5f, ratio);

        //Vector2 pos = Camera.main.ScreenToWorldPoint(GameSystem.Instance.txtGold.transform.position);

        //for (int i = 0; i < spawnNumber; i++) {
        //    star = ObjectPool.Instance.GetGameObjectFromPool("collectCoin", transform.position);
        //    star.GetComponent<CollectCoinEffect>().SetDestination(pos);
        //}
        
        //gameObject.SetActive(false);
        //LeanTween.delayedCall(2f, () =>
        //{
        //    GameSystem.Instance.AddGold(amount);
        //});
    }
}
}
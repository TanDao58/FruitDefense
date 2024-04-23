using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnHeroOnClick : MonoBehaviour, IPointerDownHandler {
    public void OnPointerDown(PointerEventData eventData) {
        SelectManagerGameplay.Instance.OnSpawnHero(eventData.position);
        //Vector2 pos = Camera.main.ScreenToWorldPoint(eventData.position);
        //SelectManagerGameplay.Instance.OnSpawnHero(pos);
    }

    //private void OnMouseDown() {
    //    SelectManagerGameplay.Instance.OnSpawnHero(Input.mousePosition);
    //}
}

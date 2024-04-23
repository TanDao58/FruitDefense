using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
namespace DarkcupGames
{
    public class PopupData
    {
        public GameObject popup;
        public Action popupAction;
    }
    [RequireComponent(typeof(Canvas))]
    public class PopupManager : MonoBehaviour
    {
        public static PopupManager Instance;
        public List<PopupData> listPopup = new List<PopupData>();
        public UnityEvent showPopupEvent;

        Canvas canvas;
        private void Awake()
        {
            Instance = this;

            canvas = GetComponent<Canvas>();
        }

        public void ShowPopup(string popupName)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.name == popupName)
                {
                    transform.GetChild(i).SetAsLastSibling();
                    transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    transform.GetChild(i).gameObject.SetActive(true);

                    //Show(transform.GetChild(i).gameObject);
                    return;
                }
            }

            GameObject obj = ObjectPool.Instance.GetGameObjectFromPool(popupName, transform);
            obj.transform.SetAsLastSibling();
            obj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            obj.SetActive(true);
            //Show(obj);
        }
        public void ResgiserPopup(GameObject popup, Action popupAction = null)
        {
            listPopup.Add(new PopupData()
            {
                popup = popup,
                popupAction = popupAction
            }) ;
        }

        public void StartShowPopup()
        {
            StartCoroutine(IEShowPopup());
        }

        private IEnumerator IEShowPopup()
        {
            int popUpIndex = 0;
            while(popUpIndex < listPopup.Count)
            {
                listPopup[popUpIndex].popup.SetActive(true);
                listPopup[popUpIndex].popupAction?.Invoke();
                yield return new WaitUntil(() => !listPopup[popUpIndex].popup.activeInHierarchy);
                popUpIndex++;               
            }
            listPopup.Clear();
        }
    }    
}
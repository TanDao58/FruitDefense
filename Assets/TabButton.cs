using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TabButton : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private Sprite selectSprite;
    [SerializeField] private Sprite unselectSprite;
    [SerializeField] private BottomGroup bottomGroup;
    private Image buttonImg;
    private Image icon;
    public bool selected;
    private Vector2 iconDefaultPosition;

    public bool Selected
    {
        get { return selected; }
        set
        {           
            if(value == true)
            {
                buttonImg.sprite = selectSprite;
                icon.gameObject.transform.DOScale(1.3f, 0.2f);
                var iconPosition = icon.transform.localPosition;
                icon.transform.DOLocalMoveY(15f, 0.2f);
            }
            else
            {
                buttonImg.sprite = unselectSprite;
                icon.gameObject.transform.DOScale(1f, 0.2f);
                icon.transform.DOLocalMoveY(1f, 0.2f);
            }
            if (popup) {
                popup.SetActive(value);
            }
            selected = value;
        }
    }

    private void Awake()
    {
        buttonImg = GetComponent<Image>();
        icon = gameObject.GetChildComponent<Image>("icon");
        bottomGroup = transform.parent.GetComponentInParent<BottomGroup>();
        iconDefaultPosition = icon.transform.localPosition;
    }
    public void Select()
    {
        bottomGroup.SetUnselectAll();
        bottomGroup.popupSelectHero.SetActive(false);
        Selected = true;

        if (Home.Instance.CurrentLevel > GameSystem.userdata.maxLevel) {
            Home.Instance.CurrentLevel = GameSystem.userdata.maxLevel;
            Home.Instance.UpdateDisplayData(Home.Instance.CurrentLevel);
        }
    }
}

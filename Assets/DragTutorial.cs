using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Localization.Components;

public class DragTutorial : Tutorial
{
    private float lastTutorialTime;
    private float tutorialInterval = 20f;
    private DragButton dragButton;
    private RectTransform blueZone;
    [SerializeField] private GameObject hand;
    [SerializeField] private TextMeshProUGUI message;
    public LocalizeStringEvent localize;
    protected override void Start()
    {
        base.Start();
        blueZone = Gameplay.Intansce.blueZones.GetComponent<RectTransform>();
        message.gameObject.SetActive(false);
        hand.SetActive(false);
        var unmaskRect = unmask.GetComponent<RectTransform>();
        dragButton = SelectManagerGameplay.Instance.bottomGroup.GetComponentInChildren<DragButton>();
        var dragButtonRect = dragButton.GetComponent<RectTransform>();
        unmaskRect.position = SelectManagerGameplay.Instance.bottomGroup.transform.position;
        unmaskRect.DOSizeDelta(new Vector2(dragButtonRect.sizeDelta.x + 50f, dragButtonRect.sizeDelta.y + 50f), 1f).OnComplete(()=>
        {
            //unmask.fitTarget = dragButtonRect;
            tutorialInterval = 0f;
            message.gameObject.SetActive(true);
            message.text = "Click on the Hero Card";
            localize.SetTable("New Table");
            localize.SetEntry(message.text);
        });
    }

    void Update()
    {
        if (!dragButton.SelectedImg.enabled) return;
        if(Time.time - lastTutorialTime > tutorialInterval)
        {
            hand.SetActive(true);
            if (!message.text.Equals("Drag the card to or click on deploy zone to summon the hero"))
            {
                message.text = "Drag the card to or click on deploy zone to summon the hero";
                localize.SetTable("New Table");
                localize.SetEntry(message.text);
            }
            hand.transform.position = SelectManagerGameplay.Instance.bottomGroup.transform.position;
            hand.transform.DOMove(Gameplay.Intansce.canvas.transform.position, 1f);
            lastTutorialTime = Time.time;
            tutorialInterval = 2f;
            unmask.fitTarget = blueZone;
        }
    }
}

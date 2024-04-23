using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class SelectManagerGameplay : MonoBehaviour 
{
    public static SelectManagerGameplay Instance;
    public GameObject bottomGroup;
    public List<MonsterAI> spawnedHero = new List<MonsterAI>();
    private DragButton selectedButton;
    private Camera mainCamera;
    private DragButton[] dragButtons;
    
    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;       
    }

    private void Start()
    {
        Gameplay.Intansce.UpdateHeroAmount();
    }

    public void SetButton(DragButton dragButton)
    {
        selectedButton = null;
        selectedButton = dragButton;
    }

    public void OnSpawnHero(Vector2 screenPos) {
        //if (pos.y < HomeArea.Instance.transform.position.y || pos.y > Gameplay.Intansce.dragLimitZone.y) return;

        if (selectedButton != null) {
            selectedButton.SpawnMonster(screenPos);
            selectedButton = null;
        }
    }

    public void ResetSelect()
    {
        foreach (var button in dragButtons)
        {
            button.SelectedImg.enabled = false;
        }
    }

    public void UpdateDragButtons() {
        dragButtons = bottomGroup.GetComponentsInChildren<DragButton>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HeroCard))]
[RequireComponent(typeof(Button))]
public class HeroCardInfo : MonoBehaviour
{
    public UpgradePanelUpdater panel;

    private Button upgradeButton;
    private HeroCard herocard;

    void Start()
    {
        herocard = GetComponent<HeroCard>();
        upgradeButton = GetComponent<Button>();
        if (upgradeButton == null)
        {
            Debug.LogError($"not found upgrade button on {gameObject.name}");
        }
        upgradeButton.onClick.AddListener(() => {
            OnHeroCardClicked(herocard.heroName);
        });
    }

    public void OnHeroCardClicked(string heroName)
    {
        panel.UpdateDisplay(heroName);
    }
}
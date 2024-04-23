using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptySlot : MonoBehaviour
{
    [SerializeField] private HeroCard chosenMonster;
    public EmptySlotManager parent;
    public Canvas canvas;

    private void Awake()
    {
        parent = transform.parent.gameObject.GetComponent<EmptySlotManager>();
    }

    public HeroCard ChosenMonster

    {
        get
        {
            return chosenMonster;
        }
        set
        {
            if (value != null)
            {
                if(!SelectHeroManager.Instance.emptySlotManager.chosenMonsters.Contains(value))
                {
                    SelectHeroManager.Instance.emptySlotManager.chosenMonsters.Add(value);
                }                
            }
            if (value == null)
            {
                SelectHeroManager.Instance.emptySlotManager.chosenMonsters.Remove(chosenMonster);
            }
            chosenMonster = value;
        }
    }

    public void UpdateChosenMonster()
    {
        chosenMonster = GetComponentInChildren<HeroCard>();
    }

    public void updateParent(GameObject herocard)
    {
        var hero = herocard.GetComponent<HeroCard>();
        parent = transform.parent.gameObject.GetComponent<EmptySlotManager>();
        hero.emptySlotManager = parent;
        canvas = transform.parent.parent.parent.parent.parent.gameObject.GetComponent<Canvas>();
        hero.canvas = canvas;
    }
}

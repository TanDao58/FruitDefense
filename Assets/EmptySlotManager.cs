using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EmptySlotManager : MonoBehaviour
{
    public List<HeroCard> chosenMonsters = new List<HeroCard>();
    public EmptySlot[] emptySlots;
    public bool inUpgrade;

    void Awake()
    {
        emptySlots = GetComponentsInChildren<EmptySlot>();
    }

    [ContextMenu("Test")]
    public void UpdateChosenMonster()
    {
        chosenMonsters = GetComponentsInChildren<HeroCard>().ToList();
    }
}

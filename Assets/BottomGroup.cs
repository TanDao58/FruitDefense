using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomGroup : MonoBehaviour
{
    public GameObject popupSelectHero;
    public TabButton[] tabButtons;

    private void Awake()
    {
        tabButtons = transform.GetComponentsInChildren<TabButton>();
    }

    public void SetUnselectAll()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            tabButtons[i].Selected = false;
        }
    }

    public void ShowTab(int index)
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            if (tabButtons[i] != null)
            {
                tabButtons[i].Selected = index == i;
            }
        }
        popupSelectHero.SetActive(false);
    }
}
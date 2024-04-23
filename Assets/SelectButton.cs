using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SelectButton : MonoBehaviour
{
    public static SelectButton instance;
    public Button saveButton;
    Button selectButton;
    public GameObject currentSelect;
    public EmptySlotManager emptySlotManager;
    List<HeroCard> temp = new List<HeroCard>();
    // Start is called before the first frame update



    public void UpdateSelectButton()
    {
        var text = GetComponentInChildren<TextMeshProUGUI>();
        if (text.text == "Remove") text.text = "Equip";
        if (currentSelect != null)
        {
            selectButton.enabled = true;
            selectButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            selectButton.enabled = false;
            selectButton.GetComponent<Image>().color = new Color(.5f, .5f, .5f, 1f);
        }
    }

    public void UpdateSaveButton()
    {
        saveButton.enabled = emptySlotManager.chosenMonsters.Count > 0;
        temp = emptySlotManager.chosenMonsters;
        if (!saveButton.enabled)
        {
            if(temp != emptySlotManager.chosenMonsters)
            {
                saveButton.enabled = true;
            }
        }
        saveButton.GetComponent<Image>().color = saveButton.enabled ? new Color(1f, 1f, 1f, 1f) : new Color(.5f, .5f, .5f, 1f);
        saveButton.gameObject.SetActive(currentSelect == null);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesCard : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountTxt;

    public void Init()
    {
        icon = GetComponent<Image>();
        amountTxt = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Dislay(ResourceData data)
    {
        gameObject.SetActive(true);
        icon.sprite = data.sprite;
        //icon.SetNativeSize();
        icon.preserveAspect = true;
        amountTxt.text = data.amount.ToString();
        amountTxt.gameObject.SetActive(data.amount > 1);
    }
}

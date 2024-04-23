using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum MoneyType { Gold, Diamond, Energy }

[RequireComponent(typeof(TextMeshProUGUI))]
public class MoneyUpdate : MonoBehaviour
{
    public float Money {
        get {
            switch (moneyType) {
                case MoneyType.Gold:
                    return GameSystem.userdata.gold;
                case MoneyType.Diamond:
                    return GameSystem.userdata.diamond;
                case MoneyType.Energy:
                    return GameSystem.userdata.energy;
                default:
                    return GameSystem.userdata.gold;
            }
        }
    }

    public MoneyType moneyType = MoneyType.Gold;
    public bool useCustomFormat;
    public string customFormat;

    float lastValue;
    TextMeshProUGUI txtValue;

    void Start()
    {
        txtValue = GetComponent<TextMeshProUGUI>();
        UpdateValue();
    }

    void Update()
    {
        if (lastValue != Money) {
            UpdateValue();
        }
    }

    void UpdateValue() {
        lastValue = Money;
        if (useCustomFormat)
        {
            txtValue.text = string.Format(customFormat, ((int)Money).ToString());
        } else
        {
            txtValue.text = ((int)Money).ToString();
        }
    }
}
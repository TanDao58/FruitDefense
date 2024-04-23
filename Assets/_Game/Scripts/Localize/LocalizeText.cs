using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizeText : MonoBehaviour, ILanguageChangeListerner
{
    public LanguageLocalize language;
    [TextArea]
    public string content;
    private TextMeshProUGUI txt;

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void OnEnable()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (LocalizeManager.GetCurrentLanguage() == language)
        {
            txt.text = content;
        }
    }

    public void NotifyLanguageChange(int language)
    {
        UpdateDisplay();
    }
}
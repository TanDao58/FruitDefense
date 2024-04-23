using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ToggleButton : MonoBehaviour
{
    public string playerPrefKey;
    public Sprite sprOn;
    public Sprite sprOff;
    public int defaultValue = 1;

    Button button;
    Image img;

    private void Awake()
    {
        if (playerPrefKey == "")
        {
            GeneralUltility.LogError("missing player pref key on toggle ", gameObject.name);
            return;
        }
        button = GetComponent<Button>();
        button.onClick.AddListener(OnToggleClick);
        img = GetComponent<Image>();
        UpdateDisplay();
    }

    public void OnToggleClick()
    {
        int value = PlayerPrefs.GetInt(playerPrefKey, defaultValue);
        value = value == 1 ? 0 : 1;
        PlayerPrefs.SetInt(playerPrefKey, value);
        UpdateDisplay();

        var settings = FindObjectsOfType<SettingListener>();
        for (int i = 0; i < settings.Length; i++)
        {
            settings[i].UpdateSetting();
        }
    }

    public void UpdateDisplay()
    {
        int value = PlayerPrefs.GetInt(playerPrefKey, defaultValue);
        img.sprite = value == 0 ? sprOff : sprOn;
    }
}
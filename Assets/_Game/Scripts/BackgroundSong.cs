using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundSong : SettingListener
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateSetting();
    }

    public override void UpdateSetting()
    {
        audioSource.enabled = PlayerPrefs.GetInt(Constants.SETTING_MUSIC, Constants.DEFAULT_MUSIC) == 1;
    }
}
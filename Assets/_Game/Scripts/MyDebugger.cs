using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Spine.Unity;
using System.Linq;
using System;

public class MyDebugger : MonoBehaviour
{
    public MyIAPManager iap;

    [ContextMenu("Test")]
    public void AddHero()
    {
        GameSystem.userdata.unlockedHeros.Add("E8");
        GameSystem.userdata.unlockedHeros.Add("E3");
        GameSystem.userdata.unlockedHeros.Add("E4");
        GameSystem.userdata.unlockedHeros.Add("E10");
        GameSystem.userdata.unlockedHeros.Add("E9");
        GameSystem.userdata.unlockedHeros.Add("E7");
        GameSystem.SaveUserDataToLocal();
    }

    float current = -9999;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(iap.controller);
            Debug.Log(iap.extensions);
        }
    }
}

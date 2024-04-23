using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIEffectRunText : UIEffectBase
{
    public int textPerUpdate = 4;
    public float updateInterval = 0.02f;

    TextMeshProUGUI txtContent;
    string src;
    bool run = false;
    float nextUpdate;

    public override void DoEffect()
    {
        run = true;
    }

    public override void Prepare()
    {
        txtContent = GetComponent<TextMeshProUGUI>();
        src = txtContent.text;
        txtContent.text = "";
        run = false;
    }

    private void Update()
    {
        if (run == false) return;
        if (Time.unscaledTime < nextUpdate) return;
        nextUpdate = Time.unscaledTime + updateInterval;

        if (src.Length <= textPerUpdate)
        {
            txtContent.text = GeneralUltility.BuildString(txtContent.text, src);
            run = false;
        } else
        {
            txtContent.text = GeneralUltility.BuildString(txtContent.text, src.Substring(0, textPerUpdate));
            src = src.Substring(textPerUpdate, src.Length - textPerUpdate);
        }
    }
}
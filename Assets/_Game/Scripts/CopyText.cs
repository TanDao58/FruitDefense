using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CopyText : MonoBehaviour
{
    public TextMeshProUGUI copyTarget;
    public float copyRate = 1;
    float nextUpdate;

    TextMeshProUGUI txt;

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Time.time > nextUpdate)
        {
            nextUpdate = Time.time + copyRate;
            txt.text = copyTarget.text;
        }
    }
}

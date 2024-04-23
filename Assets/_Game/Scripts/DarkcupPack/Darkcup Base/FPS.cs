using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
    TextMeshProUGUI txtFPS;

    void Start()
    {
        txtFPS = GetComponent<TextMeshProUGUI>();
        InvokeRepeating("UpdateFPS", 1, 1);
    }

    void UpdateFPS() {
        float fps = 1 / Time.deltaTime;
        txtFPS.text = fps.ToString("F2");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonConfig : MonoBehaviour
{
    public int targetFPS = 60;

    private void Start() {
        Application.targetFrameRate = targetFPS;
    }
}
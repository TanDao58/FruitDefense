using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFitWidth : MonoBehaviour
{
    public bool enableInUpdate = false;
    public float sceneWidth = 1f;

    Camera cam;
    
    private void Start() {
        cam = GetComponent<Camera>();
        if (!enableInUpdate) {
            FitToWidth();
            this.enabled = false;
        }
    }

    public void FitToWidth() {
        float unitsPerPixel = sceneWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        cam.orthographicSize = desiredHalfHeight;
    }

    private void Update() {
        FitToWidth();
    }
}

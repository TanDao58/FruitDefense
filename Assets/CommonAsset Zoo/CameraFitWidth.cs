using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkcupGames{
public class CameraFitWidth : MonoBehaviour
{
    public float ratio = 1f;

    void Start()
    {
        Camera.main.orthographicSize = ratio * Screen.height / Screen.width;
    }

    void Update()
    {
#if UNITY_EDITOR
        Camera.main.orthographicSize = ratio * Screen.height / Screen.width;
#endif
    }
}
}
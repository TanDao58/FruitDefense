using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChanger : MonoBehaviour
{
    Canvas canvasBackground;
    GameObject bg;

    void Start()
    {
        canvasBackground = GetComponent<Canvas>();

        int backgroundId = DataManager.Instance.mapDatas[GameSystem.userdata.currentLevel].backgroundId;
        //bg = ObjectPool.Instance.GetGameObjectFromPool<Transform>("Background/BG " + (backgroundId + 1), canvasBackground.transform.position);
        var source = Resources.Load<GameObject>("Background/BG " + (backgroundId + 1));
        bg = Instantiate(source, canvasBackground.transform);
        //ObjectPool.Instance.GetGameObjectFromPool<Transform>(, canvasBackground.transform.position);
        //bg.SetParent(canvasBackground.transform);
    }
}

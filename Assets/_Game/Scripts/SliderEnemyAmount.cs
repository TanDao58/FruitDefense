using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderEnemyAmount : MonoBehaviour
{
    public List<Image> iconWaves;
    private float imageWidth;

    private void Start()
    {
        imageWidth = GetComponent<RectTransform>().rect.width;
        var levelDatas = DataManager.Instance.levelDatas[GameSystem.userdata.currentLevel];
        for (int i = 0; i < iconWaves.Count; i++)
        {
            iconWaves[i].gameObject.SetActive(false);
            if (i < levelDatas.waveInfos.Count - 1)
            {
                float x = imageWidth * ((float)(i + 1)) / levelDatas.waveInfos.Count;
                iconWaves[i].gameObject.SetActive(true);
                iconWaves[i].rectTransform.anchoredPosition = new Vector3(x, iconWaves[i].rectTransform.anchoredPosition.y);
            }
        }
    }
}
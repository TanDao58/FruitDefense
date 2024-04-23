using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkcupGames;

public class GachaProgress : MonoBehaviour
{
    [System.NonSerialized] public GachaType resgisteredType;
    [System.NonSerialized] public Button fridgeButton;
    [System.NonSerialized] public Vector2 defaultButtonSize;
    [System.NonSerialized] public UIUpdater result;

    private Image closeFridgeImg;
    private Image openFridgeImg;
    private GameObject light;
    // Start is called before the first frame update
    void Start()
    {
        fridgeButton = GetComponentInChildren<Button>();
        closeFridgeImg = fridgeButton.gameObject.GetChildComponent<Image>("closeFridgeImg");
        openFridgeImg = fridgeButton.gameObject.GetChildComponent<Image>("openFridgeImg");
        light = gameObject.GetChildComponent<GameObject>("light");
        result = gameObject.GetChildComponent<UIUpdater>("Result");
        defaultButtonSize = fridgeButton.transform.localScale;
        openFridgeImg.gameObject.SetActive(false);
        light.SetActive(false);
        gameObject.SetActive(false);
    }

   public void ChangeImage()
    {
        openFridgeImg.gameObject.SetActive(true);
        closeFridgeImg.gameObject.SetActive(false);
        light.SetActive(true);
        EasyEffect.Appear(light, 0f, 1.5f, 0.2f, 1f);
    }

    public void ResgisterGacha(GachaType gachaType)
    {
        resgisteredType = gachaType;
        gameObject.SetActive(true);
        fridgeButton.enabled = false;
        EasyEffect.Appear(fridgeButton.gameObject, 0f, defaultButtonSize.x, 0.2f, 1f, () => { fridgeButton.enabled = true; });
    }
}

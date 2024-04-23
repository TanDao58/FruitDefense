using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DamgeNumber : MonoBehaviour
{
    [SerializeField] private float showTime;
    [SerializeField] private float hight;
    [SerializeField] private char symbol;

    [SerializeField] private TextMeshPro txt;
    [SerializeField] private Color defalutColor;

    public void ShowNumber(float number, bool crit = false)
    {
        number = Mathf.FloorToInt(Mathf.Abs(number));
        ShowText(symbol + number.ToString(), crit ? Color.red : defalutColor);
    }

    public void ShowText(string text, Color color) {
        txt.text = text;
        //var color = txt.color;
        //if (crit) {
        //    color = Color.red;
        //} else {
        //    color = defalutColor;
        //}
        color.a = 1;
        txt.color = color;
        gameObject.transform.DOMove(transform.position + new Vector3(0, hight), showTime).OnComplete(() => {
            txt.DOFade(0, 0.2f).OnComplete(() => gameObject.SetActive(false));
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuffGroup : MonoBehaviour
{
    public float animSpeed = 0.5f;

    public List<BuffButton> buttons;
    public Transform arrow;
    public Transform closePosition;

    Vector3 arrowStartPos;
    
    bool isShow = false;

    private void Start()
    {
        arrowStartPos = arrow.transform.position;
        UpdateDisplay();
    }

    public void OnArrowClick()
    {
        isShow = !isShow;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (isShow)
        {
            arrow.DORotate(new Vector3(0, 0, 0), animSpeed);
            arrow.DOMove(arrowStartPos, animSpeed).OnComplete(() =>
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    buttons[i].gameObject.SetActive(isShow);
                }
            });
        } else
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(isShow);
            }
            arrow.DORotate(new Vector3(0, 0, 180), animSpeed);
            arrow.DOMove(closePosition.position, animSpeed);
        }
    }
}

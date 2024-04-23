using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    public SpriteRenderer intansceHp;
    public SpriteRenderer slowHp;
    private float waitTime;

    private void OnEnable()
    {
        waitTime = 2f;
    }
    void Update()
    {
        if(intansceHp.transform.localScale.x < slowHp.transform.localScale.x)
        {
            waitTime -= Time.deltaTime;
            if(waitTime < 0)
            {
                var fillAmount = slowHp.transform.localScale;
                fillAmount.x -= Time.deltaTime/3;
                slowHp.transform.localScale = fillAmount;
            }
        }
        if (intansceHp.transform.localScale.x > slowHp.transform.localScale.x)
        {
            waitTime = 2f;
            slowHp.transform.localScale = intansceHp.transform.localScale;
        }
    }
}

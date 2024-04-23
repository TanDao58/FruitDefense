using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BuffType
{
    Meteor,Heal,Wind
}

public class BuffButton : MonoBehaviour
{
    [SerializeField] protected BuffType buffType;
    protected Button button;
    protected Image lockImg;
    protected TextMeshProUGUI amountTxt;
    protected const float coolDown = 60f;
    protected float currentCoolDown;
    protected int amount;

    protected int Amount
    {
        get { return amount; }
        set
        {
            amount = value;
            amountTxt.text = amount.ToString();
            GameSystem.userdata.buff[buffType] = amount;
            GameSystem.SaveUserDataToLocal();
        }
    }
    protected float CurrenCoolDown
    {
        get { return currentCoolDown; }
        set
        {
            currentCoolDown = value;
            lockImg.fillAmount = value / coolDown;
        }
    }
    protected virtual void Start()
    {
        button = GetComponent<Button>();
        lockImg = gameObject.GetChildComponent<Image>("LockImg");
        amountTxt = gameObject.GetChildComponent<TextMeshProUGUI>("AmountTxt");
        Amount = GameSystem.userdata.buff[buffType];
        currentCoolDown = 0;
    }

    private void Update()
    {
        button.interactable = CurrenCoolDown <= 0 && Amount > 0;
        if (Amount <= 0)
        {
            CurrenCoolDown = 60f;
            return;
        }
        if (coolDown > 0) CurrenCoolDown -= Time.deltaTime;
    }
}

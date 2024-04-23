using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    public void LogButtonClick(string button)
    {
        FirebaseManager.Instance.LogButtonClick(button);
    }

    public void LogPopup(string popupName)
    {
        FirebaseManager.Instance.LogPopupAppear(popupName);
    }
}
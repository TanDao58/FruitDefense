using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

namespace DarkcupGames {
    public class PopupConfirm : MonoBehaviour {
        public GameObject mainObj;
        public GameObject btnYes;
        public GameObject btnNo;
        public GameObject btnOK;

        public TextMeshProUGUI title;
        public TextMeshProUGUI mess;
        public TextMeshProUGUI txtYes;
        public TextMeshProUGUI txtNo;
        public TextMeshProUGUI txtOk;

        public Action yesAction;
        public Action noAction;
        public Action okAction;

        public void Show(string title, string message) {
            this.mainObj.SetActive(true);
            EasyEffect.Appear(mainObj, 0f, 1f);
            this.title.text = title;
            this.mess.text = message;
        }

        public void ShowOK(string title, string message) {
            this.mainObj.SetActive(true);
            EasyEffect.Appear(mainObj, 0f, 1f);
            this.title.text = title;
            this.mess.text = message;
            this.btnYes.SetActive(false);
            this.btnNo.SetActive(false);
            this.btnOK.SetActive(true);
        }

        public void ShowYesNo(string title, string message, string yes, string no, Action yesAction) {
            this.mainObj.SetActive(true);
            EasyEffect.Appear(mainObj, 0.7f, 1f, speed: 0.1f);
            this.title.text = title;
            this.mess.text = message;
            this.txtYes.text = yes;
            this.txtNo.text = no;
            this.yesAction = yesAction;
            this.btnYes.SetActive(true);
            this.btnNo.SetActive(true);
            this.btnOK.SetActive(false);
        }

        public void SetMessage(string title, string mess) {
            this.title.text = title;
            this.mess.text = mess;
        }

        public void Close() {
            EasyEffect.Appear(mainObj, 1f, 0f);
        }

        public void BackToLevelSelect() {
            SceneManager.LoadScene("LevelSelect");
        }

        public void OnYes() {
            if (yesAction != null) yesAction.Invoke();
            Close();
        }
    }
}
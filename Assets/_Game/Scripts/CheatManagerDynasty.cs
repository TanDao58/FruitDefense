using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CheatManagerDynasty : MonoBehaviour {
    public GameObject cheatPanel;
    public TMP_InputField inputLevel;

    public void ShowCheat() {
        cheatPanel.SetActive(true);
    }

    public void HideCheat() {
        cheatPanel.SetActive(false);
    }

    //public void ResetAll() {
    //    GameSystem.userdata = new UserData();
    //    GameSystem.SaveUserDataToLocal();
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    public void AddCoins(int number) {
        GameSystem.userdata.gold += number;
        GameSystem.SaveUserDataToLocal();
    }

    public void PlayLevel() {
        int level = 0;
        int.TryParse(inputLevel.text, out level);

        GameSystem.userdata.currentLevel = level;
        GameSystem.SaveUserDataToLocal();
        SceneManager.LoadScene("HeroSelect");
    }

    public void ReplayCurrentLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void CheatNoDie() {
        Constants.CHEAT_NO_DIE = true;
    }

    public void AddCurrency()
    {
        GameSystem.userdata.gold += 9999;
        GameSystem.userdata.diamond += 9999;
        GameSystem.SaveUserDataToLocal();
    }
}
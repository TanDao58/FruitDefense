using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkcupGames
{
    public class ShowHideChangeSceneLogic : MonoBehaviour
    {
        public void Show(GameObject obj)
        {
            EasyEffect.Appear(obj, 0.8f, 1f);
        }

        public void Hide(GameObject obj)
        {
            EasyEffect.Disappear(obj, 1f, 0.8f);
        }

        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void PauseGame(GameObject pausePanel)
        {
            pausePanel.SetActive(true);
            pausePanel.transform.SetAsLastSibling();
            Time.timeScale = 0f;
        }

        public void ResumeGame(GameObject pausePanel)
        {
            pausePanel.SetActive(false);
            Time.timeScale = Constants.DEFAULT_TIME_SCALE;
        }
    }
}
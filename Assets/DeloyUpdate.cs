using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DeloyUpdate : MonoBehaviour
{
    [System.NonSerialized] public int maxDeloyPoint;
    [SerializeField] private float addTime;
    private Image progessFill;
    private float currentDepoyPoint;
    private TextMeshProUGUI currentDPText;
    private TextMeshProUGUI maxDPText;
    public bool testMode;
    public float DP
    {
        get { return currentDepoyPoint; }
        set
        {
            currentDepoyPoint = value;
            UpdateDislayDP();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        progessFill = gameObject.GetChildComponent<Image>("progessBar/progessFill");
        currentDPText = gameObject.GetChildComponent<TextMeshProUGUI>("currentDP");
        maxDPText = gameObject.GetChildComponent<TextMeshProUGUI>("maxDP");
        progessFill.fillAmount = 0;

        maxDPText.text = maxDeloyPoint.ToString();
        UpdateDislayDP();
        GetStartDP();
        if (SceneManager.GetActiveScene().name == Constants.SCENE_GAMEPLAY) {
            InvokeRepeating(nameof(AutoAddDP), 0f, addTime);
        }  
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            DP += 10;
        }
        //if (Gameplay.Intansce.GameState == GameState.Lose || Gameplay.Intansce.GameState == GameState.Win || currentDepoyPoint == maxDeloyPoint) return;
        //    progessFill.fillAmount += Time.deltaTime / fillFactor;
        //if(progessFill.fillAmount >= 1)
        //{
        //    progessFill.fillAmount = 0;
        //    fillComplete.AddListener(AutoAddDP);
        //    fillComplete.Invoke();
        //    fillComplete.RemoveListener(AutoAddDP);
        //}

    }
    public void AutoAddDP()
    {
        if (Gameplay.Intansce.GameState == GameplayState.Lose || Gameplay.Intansce.GameState == GameplayState.Win || currentDepoyPoint == maxDeloyPoint) return;
        DP += Constants.DP_INCREASE_PER_SEC;
    }
    public void UpdateDislayDP()
    {
        if (progessFill == null) return;
        var amount = (float)currentDepoyPoint / maxDeloyPoint;
        LeanTween.value(progessFill.fillAmount, amount, 0.2f).setOnUpdate((y) =>
        {
            if (progessFill != null)
            {
                progessFill.fillAmount = y;
            }
        });
        currentDPText.text = ((int)currentDepoyPoint).ToString();
    }

    private void GetStartDP()
    {
        List<LevelData> data = DataManager.Instance.levelDatas;
        int playerLevel;
        if (testMode)
        {
            playerLevel = 4;
        }
        else
        {
            playerLevel = GameSystem.userdata.currentLevel;
        }
        maxDeloyPoint = Constants.DP_MAX_INGAME;
        maxDPText.text = maxDeloyPoint.ToString();
        DP = data[playerLevel].startDP;

        //if (SceneManager.GetActiveScene().name == Constants.SCENE_GAMEPLAY_NEW) {
        //    DP *= 10;
        //    maxDeloyPoint *= 10;
        //    maxDPText.text = maxDeloyPoint.ToString();
        //}
    }
}

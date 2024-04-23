using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkcupGames;
using UnityEngine.SceneManagement;
using Spine.Unity;
using TMPro;
using UnityEngine.Localization.Components;

public class PopupUnlockHero : MonoBehaviour
{
    [SerializeField] SkeletonGraphic imgHero;
    [SerializeField] TextMeshProUGUI txtHeroName;
    [SerializeField] TextMeshProUGUI txtInfo;
    public LocalizeStringEvent localize;

    public string heroId;

    public void ShowUnlockHero(string heroName)
    {
        if (DataManager.Instance.dicMonsterAIs.ContainsKey(heroName) == false)
        {
            Debug.LogError($"Not found heroName with id = {heroName}");
            return;
        }
        heroId = heroName;
        imgHero.Clear();
        imgHero.skeletonDataAsset = DataManager.Instance.dicMonsterAIs[heroName].GetComponentInChildren<SkeletonAnimation>().skeletonDataAsset;       
        imgHero.startingAnimation = "Idle";
        imgHero.Initialize(true);
        var hero = DataManager.Instance.dicMonsterAIs[heroName];
        txtHeroName.text = hero.monsterData.nickName;
        txtInfo.text = hero.monsterData.decripsion;
        if (SceneManager.GetActiveScene().name == Constants.SCENE_GAMEPLAY)
        {
            Gameplay.Intansce.UpdateSpeed();
        }
        var effect = GetComponent<UIEffect>();
        if (effect != null) effect.DoEffect();

        Spine.Animation anim = UpgradePanelUpdater.FindExistAnimation(imgHero, new List<string>() { "Idle", "Run", "Ulti" });
        if (anim != null)
        {
            imgHero.AnimationState.SetAnimation(0, anim.Name, true);
        }

        localize.SetTable("New Table");
        localize.SetEntry(hero.monsterData.decripsion);
    }

    public void OnOK()
    {
        Gameplay.Intansce.GoToNextLevel();
    }
}

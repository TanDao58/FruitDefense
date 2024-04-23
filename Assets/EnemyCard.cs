using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyCard : MonoBehaviour
{
    private Image enemyImg;
    private GameObject bossIcon;

    private void OnEnable()
    {
        if (enemyImg == null) enemyImg = gameObject.GetChildComponent<Image>("Mask/Enemy");
        if (bossIcon == null) bossIcon = gameObject.GetChildComponent<GameObject>("OuterImg/bossIcon");
    }

    public void Dislay(EnemyInfo info)
    {
        var enemy = Resources.Load<MonsterAI>(info.enemyType.ToString());
        enemyImg.sprite = enemy.portrait;
        enemyImg.SetNativeSize();
        bossIcon.SetActive(info.isBoss);
    }
}

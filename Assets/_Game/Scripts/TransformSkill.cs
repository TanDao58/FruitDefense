using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;
using DG.Tweening;

public class TransformSkill : MonoBehaviour
{
    public string heroName;
    
    private void OnEnable()
    {
        Sequence sequence = DOTween.Sequence().AppendInterval(2f);
        sequence.AppendCallback(() =>
        {
            if (gameObject.activeInHierarchy)
            {
                ObjectPool.Instance.GetGameObjectFromPool<ParticleSystem>("Vfx/SmokeTransform", transform.GetChild(0).position);
            }
        });
        sequence.AppendInterval(0.2f);
        sequence.AppendCallback(() =>
        {
            if (gameObject.activeInHierarchy)
            {
                TransformHero();
            }
        });
    }

    public void TransformHero()
    {
        var list = new List<string>();
        list.AddRange(DataManager.Instance.allyNames);
        if (list.Contains(heroName))
        {
            list.Remove(heroName);
        }
        string randomName = list.RandomElement();

        var monsterAi = GetComponent<MonsterAI>();
        var heros = SelectManagerGameplay.Instance.spawnedHero;
        
        MonsterAI newMonster = ObjectPool.Instance.GetGameObjectFromPool<MonsterAI>(randomName, transform.position);

        for (int i = 0; i < heros.Count; i++)
        {
            if (heros[i] == monsterAi)
            {
                heros[i] = newMonster;
            }
        }
        //monsterAi.Die();
        gameObject.SetActive(false);
    }
}

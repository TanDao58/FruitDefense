using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkcupGames;
using DG.Tweening;

public class FireworkManager : MonoBehaviour
{
    public List<Color> colors;
    public SpriteRenderer sprVictory;
    float fireworkTime;

    private void OnEnable()
    {
        sprVictory.color = new Color(1, 1, 1, 0);
        sprVictory.DOFade(1f, 1f);
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup > fireworkTime)
        {
            fireworkTime = Time.realtimeSinceStartup + Random.Range(0.5f, 1f);
            ParticleSystem effect = ObjectPool.Instance.GetGameObjectFromPool<ParticleSystem>("Vfx/Firework", transform.position + (Vector3)Random.insideUnitCircle* 2f);
            var mainModule = effect.main;
            mainModule.startColor = colors.RandomElement();
        }
    }
}

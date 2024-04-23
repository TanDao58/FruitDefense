using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplose : MonoBehaviour
{
    [System.NonSerialized] public HitParam hitParam;
    [SerializeField] private float radius;
    private Camera mainCam;

    private void OnEnable()
    {
        mainCam = Camera.main;
    }

    public void Explode(HitParam hit)
    {
        hitParam = hit;
        RipplePostProcessor.Intansce.CallRippleEffect(mainCam.WorldToScreenPoint(transform.position));
        foreach (var enemy in EnemySpawner.Instance.spawnedEnemies)
        {
            var distanceToExplosion = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToExplosion <= radius)
            {
                enemy.TakeDame(hitParam);
            }
        }
    }
}

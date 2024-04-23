using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HomeArea : MonoBehaviour
{
    public static HomeArea Instance;
    [SerializeField] private GameObject reference;
    private Camera mainCam;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        mainCam = Camera.main;
        transform.position = (Vector2)mainCam.ScreenToWorldPoint(reference.transform.position);
        var pos = reference.transform.position;
        pos.x = Gameplay.Intansce.shield.transform.position.x;
        Gameplay.Intansce.shield.transform.position = pos;
        Destroy(reference);

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<MonsterAI>(out var monsterAI))
        {
            if (monsterAI.IsEnemy)
            {
                Gameplay.Intansce.EnemyAmount--;
                EasyEffect.Disappear(collision.gameObject, 1, 0);
                monsterAI.Col.enabled = false;
                if (monsterAI.isBoss)
                {
                    Gameplay.Intansce.LifePoint -= Gameplay.Intansce.LifePoint;
                }
                else
                {
                    Gameplay.Intansce.LifePoint -= 1;
                }
                spriteRenderer.DOColor(Color.red, 0.2f).OnComplete(() => spriteRenderer.DOColor(Color.white, 0.2f));
                monsterAI.battleStat.hp = 0;
                EnemySpawner.Instance.spawnedEnemies.Remove(monsterAI);
                Gameplay.Intansce.DoHpLostEfect();
                if (PlayerPrefs.GetInt(Constants.SETTING_VIBRATION, Constants.DEFAULT_VIBRATION)== 1)
                {
                    Handheld.Vibrate();
                }
            }
            else
            {
                collision.attachedRigidbody.velocity = Vector2.zero;
            }
        }
    }
}
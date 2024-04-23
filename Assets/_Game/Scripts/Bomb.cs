using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float disappearTime = 3f;
    [System.NonSerialized] public HitParam hitParam;
    private Collider2D col;
    public AudioClip bombSound;
    bool playEffect;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        col.enabled = true;
        playEffect = false;
        Invoke(nameof(TurnOffCollider), 0.2f);
        Invoke(nameof(Disapear), disappearTime);
        //hitParam.owner = transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

        var monsterAI = collision.GetComponent<MonsterAI>();
        if (monsterAI != null)
        {
            //hitParam.damage = monsterAI.monsterData.maxhp;
            hitParam.silentSound = true;
            monsterAI.TakeDame(hitParam);
        }
        if (!playEffect)
        {
            playEffect = true;
            SFXSystem.Instance.Play(bombSound);
            if (PlayerPrefs.GetInt(Constants.SETTING_VIBRATION, Constants.DEFAULT_VIBRATION) == 1)
            {
                Handheld.Vibrate();
            }
        }
    }

    private void Disapear()
    {
        gameObject.SetActive(false);
    }

    private void TurnOffCollider()
    {
        col.enabled = false;
    }
}
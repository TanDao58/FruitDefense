using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEffect : MonoBehaviour
{
    private MonsterAI monsterAI;

    [HideInInspector]
    public bool froze;
    private float unFreezeTime;
    private float takeFreezeDamageTime;
    private HitParam freeDamage;

    [HideInInspector]
    public bool slow;
    private float nextOutOfSlow;

    [HideInInspector]
    public bool invisible;
    private float nextVisibleTime;
    private int layer;

    [HideInInspector]
    public bool hypnotized;
    private float unhypnotizedTime;

    [HideInInspector]
    public bool white;
    private float nextNormalTime;

    [HideInInspector]
    public bool stun;
    private float nextUnstunTime;

    private void Start()
    {
        monsterAI = GetComponent<MonsterAI>();
        layer = gameObject.layer;
        freeDamage = new HitParam();
        freeDamage.damage = monsterAI.battleStat.maxhp * Constants.FREEZE_DAME_PERCENT;
    }
    private void Update()
    {
        if (slow && Time.time > nextOutOfSlow)
        {
            slow = false;
            monsterAI.battleStat.speed = monsterAI.monsterData.speed;
            monsterAI.AimSetter.SkeletonAnimation.timeScale = 1f;
            monsterAI.battleStat.attackInterval = monsterAI.monsterData.attackInterval;
        }

        if(froze && Time.time > unFreezeTime)
        {
            froze = false;
            monsterAI.AimSetter.SkeletonAnimation.enabled = true;
        }

        //if (froze && Time.time > takeFreezeDamageTime)
        //{
        //    takeFreezeDamageTime = Time.time + 1.2f;
        //    monsterAI.TakeDame(freeDamage);
        //}

        if(invisible && Time.time > nextVisibleTime)
        {
            invisible = false;
            //monsterAI.AimSetter.FramesAnimator.SpriteRenderer.enabled = true;
            monsterAI.battleStat.speed = monsterAI.monsterData.speed;
            monsterAI.AimSetter.SkeletonAnimation.AnimationName = "Run";
            gameObject.layer = layer;
        }


        /*if(hypnotized && Time.time > unhypnotizedTime)
        {
            hypnotized = false;
            SetNullAtacker();
            monsterAI.AttackTarget = null;
            monsterAI.ChangeSide();
            var icon = gameObject.GetChildComponent<GameObject>("HypontizedIcon(Clone)");
            icon.SetActive(false);
        }*/

        if(white && Time.time > nextNormalTime)
        {
            white = false;
            monsterAI.MeshOrder.SetNormal();
        }

        if(stun && Time.time > nextUnstunTime)
        {
            stun = false;
            monsterAI.battleStat.speed = monsterAI.monsterData.speed;
        }
    }

    public void Slow(float slowTime)
    {
        if (slow) return;
        monsterAI.battleStat.speed = monsterAI.monsterData.speed / 2;
        monsterAI.AimSetter.SkeletonAnimation.timeScale = 0.5f;
        monsterAI.battleStat.attackInterval = monsterAI.monsterData.attackInterval*2;
        nextOutOfSlow = Time.time + slowTime;
        slow = true;
    }

    public void Freeze(float freezeTime)
    {
        froze = true;
        unFreezeTime = Time.time + freezeTime;
        var iceObject = ObjectPool.Instance.GetGameObjectFromPool("Ice", monsterAI.transform.position);
        var ice = iceObject.GetComponent<Ice>();
        ice.freezeTime = freezeTime;
        ice.target = monsterAI;
        ice.spriteRenderer.sortingOrder = monsterAI.GetComponentInChildren<MeshRenderer>().sortingOrder + 1;
        monsterAI.AimSetter.SkeletonAnimation.enabled = false;
        SetNullAtacker();
    }

    public void Invisible(float invisibleTime)
    {
        invisible = true;
        nextVisibleTime = Time.time + invisibleTime;
        //if (monsterAI.AimSetter.FramesAnimator == null) {
        //    Debug.LogError(GeneralUltility.BuildString("null FramesAnimator in monster", gameObject.name));
        //}
        //monsterAI.AimSetter.FramesAnimator.SpriteRenderer.enabled = false;
        monsterAI.AimSetter.SkeletonAnimation.AnimationName = "Ulti";
        monsterAI.battleStat.speed = monsterAI.monsterData.speed * GhostSkill.INVISIBLE_SPEED_INCREASE;
        monsterAI.AttackTarget = null;
        gameObject.layer = 0;
        SetNullAtacker();
    }

    public void Hypnotized(float hypnotizedTime)
    {
        hypnotized = true;
        unhypnotizedTime = Time.time + hypnotizedTime;
        SetNullAtacker();
        monsterAI.AttackTarget = null;
        monsterAI.ChangeSide();
        monsterAI.IsEnemy = true;
        EnemySpawner.Instance.spawnedEnemies.Add(monsterAI);
        SelectManagerGameplay.Instance.spawnedHero.Remove(monsterAI);
        Gameplay.Intansce.UpdateHeroAmount();
    }

    public void SetNullAtacker()
    {
        var damables = FindObjectsOfType<MonsterAI>();
        for (int i = 0; i < damables.Length; i++)
        {
            if (damables[i].AttackTarget == transform)
            {
                damables[i].AttackTarget = null;
            }
        }
    }
    public void TurnWhite(float time)
    {
        white = true;
        nextNormalTime = Time.time + time;
        monsterAI.MeshOrder.SetWhite();
    }

    public void Stun(float time)
    {
        if (Time.time < nextVisibleTime) return;
        stun = true;
        nextUnstunTime = Time.time + time;
        monsterAI.battleStat.speed = 0f;
    }
}

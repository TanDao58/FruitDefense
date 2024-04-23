using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using CartoonFX;

public class AttackBomb : MonoBehaviour
{
    [SerializeField] private MonsterAI monster;
    public float shakeStrength = 0.5f;
    bool canExplode = true;
    private void OnEnable()
    {
        canExplode = true;
    }
    private void Start()
    {
        monster = GetComponent<MonsterAI>();
    }
    public void Attack()
    {
        monster.Die();
        AddEventExplose();
    }

    private void Explose(TrackEntry entry, Spine.Event e)
    {
        if (e.Data.Name != "OnAttack") return;
        if (canExplode)
        {
            canExplode = false;
            var bomb = ObjectPool.Instance.GetGameObjectFromPool<Bomb>("Bomb", transform.position);
            CFXR_Effect shakeEffect = bomb.GetComponent<CFXR_Effect>();
            shakeEffect.cameraShake.shakeStrength = Vector3.one * shakeStrength;
            bomb.hitParam = monster.HitParam;
        }
        monster.BattleStat.hp = 0;
    }

    public void AddEventExplose()
    {
        monster.AimSetter.SkeletonAnimation.AnimationState.Event -= Explose;
        monster.AimSetter.SkeletonAnimation.AnimationState.Event += Explose;
    }
}
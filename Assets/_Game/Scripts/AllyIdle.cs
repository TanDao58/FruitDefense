using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyIdle : MonoBehaviour
{
    [SerializeField] AnimationSetter anim;
    private MonsterAI monster;

    private void Start()
    {
        anim = gameObject.GetChildComponent<AnimationSetter>("skeletonAnim");
        monster = GetComponent<MonsterAI>();
    }
    public void Idle()
    {
        if (monster.IsDead()) return;
        anim.SetAnimation(monster.idleAnimationName);
        anim.SkeletonAnimation.loop = true;
    }
}

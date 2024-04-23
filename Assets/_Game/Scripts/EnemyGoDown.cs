using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoDown : MonoBehaviour
{
    [SerializeField] AnimationSetter anim;
    private MonsterAI monsterAI;

    private void Awake() {
        anim = gameObject.GetChildComponent<AnimationSetter>("skeletonAnim");
        monsterAI = GetComponent<MonsterAI>();
    }

    public void GoDown() {
        if (anim == null) return;
        if (anim == null) {
            Debug.LogError("No anim in game object " + gameObject.name);
            return;
        }
        anim.SetAnimation(monsterAI.runAnimationName);
        if (anim.SkeletonAnimation == null) {
            Debug.LogError(GeneralUltility.BuildString("null skeleton anim at game object ", gameObject.name));
        }
        anim.SkeletonAnimation.loop = true;
        transform.position += new Vector3(0, -1) * monsterAI.battleStat.speed * Time.deltaTime;
    }

    public void AddOne() {
        Debug.Log("Run!");
    }
}

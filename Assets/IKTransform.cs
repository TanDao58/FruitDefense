using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class IKTransform : MonoBehaviour
{
    [SpineBone]
    [SerializeField] private string boneName;
    private MonsterAI monsterAI;
    [SerializeField] private SkeletonAnimation anim;

    public Spine.Bone targetBone;
    private Vector2 defaultPosition;
    public float debug;
    // Start is called before the first frame update
    void Start()
    {
        monsterAI = GetComponent<MonsterAI>();
        anim = monsterAI.AimSetter.SkeletonAnimation;
        targetBone = anim.Skeleton.FindBone(boneName);
        defaultPosition = targetBone.GetLocalPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (monsterAI.AttackTarget != null)
        {
            if (targetBone == null)
            {
                Debug.LogError($"target bone is null at gameobject = {gameObject.name}");
                return;
            }
            var pos = anim.transform.InverseTransformPoint(monsterAI.AttackTarget.position);
            if (Vector2.Distance(transform.position, monsterAI.AttackTarget.position) > 0)
                targetBone.SetLocalPosition(pos);
            else
                targetBone.SetLocalPosition(defaultPosition);
        }
        else
        {
            targetBone.SetLocalPosition(defaultPosition);
        }
    }
}

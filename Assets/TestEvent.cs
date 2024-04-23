using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class TestEvent : MonoBehaviour
{
    [SerializeField] PointFollower point;

    SkeletonAnimation skeleton;

    private void Awake()
    {
        skeleton = GetComponent<SkeletonAnimation>();
        skeleton.AnimationState.Event += HandleEvent;
    }

    public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        Debug.Log(e.Data.Name);
        if (e.Data.Name == "Spawn")
        {
            ObjectPool.Instance.GetGameObjectFromPool<MonsterAI>("E11", point.transform.position);
        }
    }
}

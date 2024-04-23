using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;

public class AnimationSetter : MonoBehaviour
{
    FramesAnimator framesAnimator;
    SkeletonAnimation spineAnimation;
    Animator animator;

    string currentAnimationName = "";

    public FramesAnimator FramesAnimator => framesAnimator;
    public SkeletonAnimation SkeletonAnimation => spineAnimation;
    public Animator Animator => animator;
    
    private void Awake() {
        framesAnimator = GetComponent<FramesAnimator>();
        spineAnimation = GetComponent<SkeletonAnimation>();
        animator = GetComponent<Animator>();
    }

    public void SetAnimation(string animationName, Action doneAction = null) {
        if (spineAnimation != null)
        {
            spineAnimation.AnimationName = animationName;
            if (doneAction != null)
            {
                var myAnimation = spineAnimation.Skeleton.Data.FindAnimation(animationName);
                float duration = myAnimation.Duration;
                StartCoroutine(IEDelayCall(duration, doneAction));
            }
        }
        if (animator != null)
        {
            animator.Play(animationName);
        }
        if (framesAnimator != null)
        {
            framesAnimator.SetAnimation(animationName, doneAction);
        }
    }

    public void SetFacing(Vector3 target) {
        Vector3 localScale = transform.localScale;

        if (transform.position.x < target.x) {
            localScale.x = -Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }

        if (transform.position.x > target.x) {
            localScale.x = Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
    }

    IEnumerator IEDelayCall(float time, Action action) {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }
}

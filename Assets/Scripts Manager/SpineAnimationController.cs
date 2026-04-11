using UnityEngine;
using Spine.Unity;

public class SpineAnimationController : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;

    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        PlayIdle();
    }

    public void PlayIdle()
    {
        if (skeletonAnimation != null)
            skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
    }

    public void PlayWalk()
    {
        if (skeletonAnimation != null)
            skeletonAnimation.AnimationState.SetAnimation(0, "Walk", true);
    }

    public void PlayAttack()
    {
        if (skeletonAnimation != null)
            skeletonAnimation.AnimationState.SetAnimation(0, "Attack", false);
    }

    public void PlayDead()
    {
        if (skeletonAnimation != null)
            skeletonAnimation.AnimationState.SetAnimation(0, "Dead", false);
    }
}
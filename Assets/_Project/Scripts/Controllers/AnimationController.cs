using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    
    static readonly int LocomotionTriggerKey = Animator.StringToHash("Locomotion");
    static readonly int LocomotionKeyRatioKey = Animator.StringToHash("LocomotionRatio");
    
    static readonly int PickUpTriggerKey = Animator.StringToHash("PickUp");


    private const float AnimatorDampTime = 0.1f;


    public void PlayIdleWalkRunAnimation()
    {
        animator.SetTrigger(LocomotionTriggerKey);
    }
    
    public void UpdateIdleWalkRunRatio(float ratio, float deltaTime, bool skipDampTime = false)
    {
        if (skipDampTime)
        {
            animator.SetFloat(LocomotionKeyRatioKey, ratio);
        }
        else
        {
            animator.SetFloat(LocomotionKeyRatioKey, ratio, AnimatorDampTime, deltaTime);
        }
    }
    
    public void PlayPickUpAnimation()
    {
        animator.SetTrigger(PickUpTriggerKey);
    }
}

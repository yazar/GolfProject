using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    
    static readonly int LocomotionTriggerKey = Animator.StringToHash("Locomotion");
    static readonly int LocomotionKeyRatioKey = Animator.StringToHash("LocomotionRatio");
    
    static readonly int PickUpTriggerKey = Animator.StringToHash("PickUp");
    static readonly int DropTriggerKey = Animator.StringToHash("Drop");
    static readonly int LoseTriggerKey = Animator.StringToHash("Lose");
    static readonly int VictoryTriggerKey = Animator.StringToHash("Victory");


    private const float AnimatorDampTime = 0.1f;


    public void PlayLocomotionAnimation()
    {
        animator.SetTrigger(LocomotionTriggerKey);
    }
    
    public void UpdateLocomotionRatio(float ratio, float deltaTime)
    {
        animator.SetFloat(LocomotionKeyRatioKey, ratio, AnimatorDampTime, deltaTime);
    }
    
    public void UpdateLocomotionRatio(float ratio)
    {
        animator.SetFloat(LocomotionKeyRatioKey, ratio);
    }
    
    public void PlayPickUpAnimation()
    {
        animator.SetTrigger(PickUpTriggerKey);
    }
    
    public void PlayDropAnimation()
    {
        animator.SetTrigger(DropTriggerKey);
    }
    
    public void PlayLoseAnimation()
    {
        animator.SetTrigger(LoseTriggerKey);
    }
    
    public void PlayVictoryAnimation()
    {
        animator.SetTrigger(VictoryTriggerKey);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    
    static readonly int IdleWalkRunKey = Animator.StringToHash("IdleWalkRun");
    static readonly int IdleWalkRunRatioKey = Animator.StringToHash("IdleWalkRunRatio");

    private const float AnimatorDampTime = 0.1f;


    public void PlayIdleWalkRunAnimation()
    {
        //animator.SetTrigger(IdleWalkRunKey);
    }
    
    public void UpdateIdleWalkRunRatio(float ratio, float deltaTime)
    {
        //animator.SetFloat(IdleWalkRunRatioKey, ratio, AnimatorDampTime, deltaTime);
    }
}

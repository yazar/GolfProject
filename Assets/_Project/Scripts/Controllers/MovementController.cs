using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
    [SerializeField] private NpcSettings npcSettings;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private AnimationController animationController;

    private void Start()
    {
        navMeshAgent.speed = npcSettings.speed;
        navMeshAgent.stoppingDistance = npcSettings.stoppingDistance;
        navMeshAgent.acceleration = npcSettings.acceleration;
    }

    private void Update()
    {
        float speedRation = navMeshAgent.velocity.magnitude / npcSettings.speed;
        animationController.UpdateLocomotionRatio(speedRation, Time.deltaTime);
    }

    public void Move(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
    }

    public void Stop()
    {
        navMeshAgent.SetDestination(transform.position);
    }

    public float GetRemainingDistance()
    {
        if (navMeshAgent.pathPending)
            return 10;

        return navMeshAgent.remainingDistance;
    }
}

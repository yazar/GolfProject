using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    
    
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

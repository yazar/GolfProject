using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBallManager : MonoBehaviour
{
    [SerializeField] private List<Transform> balls;
    [SerializeField] private int numberOfBalls;

    private void Start()
    {
        ScatterGolfBalls();
    }

    private void ScatterGolfBalls()
    {
        GameManager.Instance.OnBallsScattered?.Invoke(balls);
    }
    
    private void HandleReset()
    {
        
    }

    private void OnEnable()
    {
        GameManager.Instance.OnReset += HandleReset;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnReset -= HandleReset;
    }
}

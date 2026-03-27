using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCountManager : MonoBehaviour
{
    private float _points;
    private float _pointsInCarry;

    private void OnEnable()
    {
        GameManager.Instance.OnCollectedBall += HandleOnBallCollected;
        GameManager.Instance.OnBallsDropped += HandleBallsDropped;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCollectedBall -= HandleOnBallCollected;
        GameManager.Instance.OnBallsDropped -= HandleBallsDropped;
    }

    public void HandleBallsDropped()
    {
        _points += _pointsInCarry;
        _pointsInCarry = 0f;
        GameManager.Instance.PointsUI.SetPointCountText(_points);
        GameManager.Instance.PointsWaitingUI.SetPointCountText(_pointsInCarry);
    }
    
    public void HandleOnBallCollected(float points)
    {
        _pointsInCarry += points;
        GameManager.Instance.PointsWaitingUI.SetPointCountText(_pointsInCarry);
    }
}

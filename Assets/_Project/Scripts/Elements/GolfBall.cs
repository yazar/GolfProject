using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBall : MonoBehaviour, IPoolable
{
    [SerializeField] private BallSetting _ballSettings;
    
    public void SetBallSetting(BallSetting ballSettings)
    {
        _ballSettings = ballSettings;
    }

    public float GetBallPoints()
    {
        return _ballSettings.points;
    }

    #region PoolingFunctions
    
    public bool IsInPool { get; set; }
    public void OnRemoveFromPool()
    {
        gameObject.SetActive(true);
    }

    public void OnReturnPool()
    {
        gameObject.SetActive(false);
    }

    public void OnPerformanceReUse()
    {
        
    }
    
    #endregion PoolingFunctions
    
}

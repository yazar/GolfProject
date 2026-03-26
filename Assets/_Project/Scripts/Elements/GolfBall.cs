using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GolfBall : MonoBehaviour, IPoolable
{
    [FormerlySerializedAs("_ballSettings")] [SerializeField] private BallSetting ballSettings;
    
    public void SetBallSetting(BallSetting settings)
    {
        ballSettings = settings;
    }
    
    public float GetBallPoints()
    {
        return ballSettings.points;
    }
    
    public int GetBallLevel()
    {
        return ballSettings.ballLevel;
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

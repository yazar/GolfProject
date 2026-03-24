using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GolfBallManager : MonoBehaviour
{
    [SerializeField] private BallSettings ballSettings;
    [SerializeField] private List<GolfBall> balls;
    [SerializeField] private int ballCount = 10;
    
    [SerializeField] private Terrain terrain;
    [SerializeField] private Transform startPoint;
    [SerializeField] private NavMeshData navMeshData;

    public bool useTestList;
    
    private NavMeshPath _reusablePath;

    private void Start()
    {
        ScatterGolfBalls();
    }

    private void ScatterGolfBalls()
    {
        if(!useTestList)
            PlaceBalls();
        
        GameManager.Instance.OnBallsScattered?.Invoke(balls);
        GameManager.Instance.SwitchGameState(GameStates.WaitingForPlay);
    }
    
    private void HandleReset()
    {
        if(!useTestList)
            ReturnBallsToPool();
        
        CancelInvoke(nameof(ScatterGolfBalls));
        Invoke(nameof(ScatterGolfBalls), 0.1f);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnReset += HandleReset;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnReset -= HandleReset;
    }
    
    public void PlaceBalls()
    {
        float navMeshSampleRadius = 5f;

        _reusablePath = new NavMeshPath();
        balls = new List<GolfBall>();

        TerrainData td = terrain.terrainData;
        Vector3 tp = terrain.transform.position;

        int placed = 0;
        int maxAttempts = ballCount * 10;

        if (!NavMesh.SamplePosition(startPoint.position, out var startPointHit, navMeshSampleRadius, NavMesh.AllAreas))
        {
            Debug.LogError("Start Point is not on navmesh");
            return;
        }
        Vector3 startPosition = startPointHit.position;

        for (int i = 0; i < maxAttempts && placed < ballCount; i++)
        {
            float x = tp.x + Random.Range(0f, td.size.x);
            float z = tp.z + Random.Range(0f, td.size.z);
            float y = terrain.SampleHeight(new Vector3(x, 0, z)) + tp.y;
            
            if (!NavMesh.SamplePosition(new Vector3(x, y, z), out var hit, navMeshSampleRadius, NavMesh.AllAreas))
                continue;

            if (startPoint != null)
            {
                _reusablePath.ClearCorners();
                NavMesh.CalculatePath(startPosition, hit.position, NavMesh.AllAreas, _reusablePath);
                if (_reusablePath.status != NavMeshPathStatus.PathComplete)
                    continue;
            }
            
            Vector3 pos = hit.position;
            GolfBall golfBall = PoolManager.Instance.GetFromPool<GolfBall>(pos, Quaternion.identity, transform);
            golfBall.SetBallSetting(ballSettings.ballSettingsLibrary.RandomElement());
            balls.Add(golfBall);
            placed++;
        }
    }

    private void ReturnBallsToPool()
    {
        foreach (GolfBall ball in balls)
        {
            PoolManager.Instance.ReturnToPool(ball);
        }
        
        balls.Clear();
    }
}

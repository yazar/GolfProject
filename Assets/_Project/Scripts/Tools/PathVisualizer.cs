using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathVisualizer : MonoBehaviour
{
    [SerializeField] private NpcSettings npcSettings;
    [SerializeField] private Transform visualizationParent;
    
    private bool _isVisible;
    private List<LineRenderer> _activeLines = new List<LineRenderer>();
    private List<BallInfo> _activeBallInfos = new List<BallInfo>();

    private PathVisualizationSettings _currentPathSettings;
    
    private void Awake()
    {
        _isVisible = npcSettings.showVisualizationOnStart;
        _currentPathSettings = npcSettings.settingsOnTps;
    }

    private void TogglePathVisualizer()
    {
        _isVisible = !_isVisible;

        visualizationParent.gameObject.SetActive(_isVisible);
    }
    
    public void Visualize(Vector3 startPos, List<Vector3> path, List<GolfBall> targetBalls)
    {
        ClearVisualization();

        if (path == null || path.Count == 0)
            return;

        // Split into trips and draw lines
        List<List<Vector3>> trips = SplitIntoTrips(startPos, path, targetBalls);

        for (int t = 0; t < trips.Count; t++)
        {
            Color color = _currentPathSettings.tripColors[t % _currentPathSettings.tripColors.Length];
            DrawTripLine(trips[t], color);
        }

        // Spawn ball markers from pool
        int baseDropCount = 0;
        for (int i = 0; i < path.Count; i++)
        {
            if (targetBalls[i] == null)
            {
                baseDropCount++;
                continue;
            }
            
            Vector3 pathPosition = path[i];
            BallInfo ballInfo = PoolManager.Instance.GetFromPool<BallInfo>(pathPosition, Quaternion.identity, visualizationParent);
            ballInfo.SetInfo(targetBalls[i].GetBallLevel(), i + 1 - baseDropCount, pathPosition);
            _activeBallInfos.Add(ballInfo);
        }
    }

    private void ClearVisualization()
    {
        // Return markers to pool
        for (int i = 0; i < _activeBallInfos.Count; i++)
            PoolManager.Instance.ReturnToPool(_activeBallInfos[i]);
        _activeBallInfos.Clear();

        // Return line renderers to pool
        for (int i = 0; i < _activeLines.Count; i++)
            PoolManager.Instance.ReturnToPool(_activeLines[i]);
        _activeLines.Clear();
    }
    
    private List<List<Vector3>> SplitIntoTrips(Vector3 startPos, List<Vector3> path, List<GolfBall> targetBalls)
    {
        List<List<Vector3>> trips = new List<List<Vector3>>();
        List<Vector3> currentTrip = new List<Vector3> { startPos };

        for (int i = 0; i < path.Count; i++)
        {
            currentTrip.Add(path[i]);

            if (targetBalls[i] == null)
            {
                trips.Add(currentTrip);
                currentTrip = new List<Vector3> { startPos };
            }
        }

        if (currentTrip.Count > 1)
            trips.Add(currentTrip);

        return trips;
    }

    private void DrawTripLine(List<Vector3> waypoints, Color color)
    {
        List<Vector3> fullPath = CalculateNavMeshPath(waypoints);
        if (fullPath.Count < 2) return;

        LineRenderer lr = PoolManager.Instance.GetFromPool<LineRenderer>(Vector3.zero, Quaternion.identity, visualizationParent);
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = _currentPathSettings.lineWidth;
        lr.endWidth = _currentPathSettings.lineWidth;
        lr.useWorldSpace = true;
        lr.positionCount = fullPath.Count;

        for (int i = 0; i < fullPath.Count; i++)
            lr.SetPosition(i, fullPath[i]);
        
        lr.useWorldSpace = false;
        Vector3 offsetPosition = lr.transform.position + Vector3.up * _currentPathSettings.lineHeightOffset;
        lr.transform.position = offsetPosition;
        
        _activeLines.Add(lr);
    }

    private List<Vector3> CalculateNavMeshPath(List<Vector3> waypoints)
    {
        List<Vector3> fullPath = new List<Vector3>();
        NavMeshPath navPath = new NavMeshPath();

        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            if (NavMesh.CalculatePath(waypoints[i], waypoints[i + 1], NavMesh.AllAreas, navPath))
            {
                int startIndex = (fullPath.Count == 0) ? 0 : 1;
                for (int j = startIndex; j < navPath.corners.Length; j++)
                    fullPath.Add(navPath.corners[j]);
            }
        }

        return fullPath;
    }
    
    private void HandleCameraChange(CameraType cameraType)
    {
        _currentPathSettings = cameraType == CameraType.Tps ?  npcSettings.settingsOnTps : npcSettings.settingsOnTopdown;

        foreach (LineRenderer lr in _activeLines)
        {
            lr.startWidth = _currentPathSettings.lineWidth;
            lr.endWidth = _currentPathSettings.lineWidth;

            lr.useWorldSpace = false;
            Vector3 pos = lr.transform.position;
            pos.y = _currentPathSettings.lineHeightOffset;
            lr.transform.position = pos;
        }
    }
    
    private void OnEnable()
    {
        GameManager.Instance.OnCameraChange += HandleCameraChange;
        GameManager.Instance.OnTogglePathVisualization += TogglePathVisualizer;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCameraChange -= HandleCameraChange;
        GameManager.Instance.OnTogglePathVisualization -= TogglePathVisualizer;
    }
}
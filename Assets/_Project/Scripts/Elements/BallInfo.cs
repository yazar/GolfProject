using System;
using UnityEngine;
using TMPro;

public class BallInfo : MonoBehaviour
{ 
    [SerializeField] private NpcSettings npcSettings;
    [SerializeField] private TMP_Text levelText;

    private PathVisualizationSettings _currentPathSettings;
    private Vector3 _basePosition =  Vector3.zero;
    private CameraType _cameraType = CameraType.Tps;
    
    public void SetInfo(int level, int stepOrder, Vector3 pathPosition)
    {
        levelText.text = $"{stepOrder} \n Lv. {level}";
        _basePosition = pathPosition;

        UpdateBallInfoSettings();
    }
    
    private void HandleCameraChange(CameraType cameraType)
    {
        _cameraType =  cameraType;
        
        UpdateBallInfoSettings();
    }

    private void UpdateBallInfoSettings()
    {
        _currentPathSettings = _cameraType == CameraType.Tps ?  npcSettings.settingsOnTps : npcSettings.settingsOnTopdown;
        transform.position = _basePosition + Vector3.up * _currentPathSettings.ballInfoHeightOffset;
        transform.localScale = _currentPathSettings.ballInfoScale;
    }
    
    private void OnEnable()
    {
        GameManager.Instance.OnCameraChange += HandleCameraChange;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCameraChange -= HandleCameraChange;
    }
}
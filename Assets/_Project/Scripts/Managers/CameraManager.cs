using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject tpsNpcCamera;
    [SerializeField] private GameObject topdownCamera;

    private CameraType _cameraType = CameraType.Tps;
    
    private void HandleCameraSwitch()
    {
        tpsNpcCamera.SetActive(!tpsNpcCamera.activeInHierarchy);
        topdownCamera.SetActive(!topdownCamera.activeInHierarchy);
        
        _cameraType = tpsNpcCamera.activeInHierarchy ?  CameraType.Tps : CameraType.Topdown;
        
        GameManager.Instance.OnCameraChange?.Invoke(_cameraType);
    }
    
    private void OnEnable()
    {
        GameManager.Instance.OnCameraToggle += HandleCameraSwitch;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCameraToggle -= HandleCameraSwitch;
    }
}

public enum CameraType
{
    Tps,
    Topdown
}
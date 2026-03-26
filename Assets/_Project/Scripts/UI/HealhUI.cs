using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealhUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    private Transform _camTransform;

    public void SetFillAmount(float fillAmount)
    {
        fillImage.fillAmount = fillAmount;
    }
    
    void Awake()
    {
        _camTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _camTransform.position);
    }
}

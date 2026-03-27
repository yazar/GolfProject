using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    
    public void SetFillAmount(float fillAmount)
    {
        fillImage.fillAmount = fillAmount;
    }
}

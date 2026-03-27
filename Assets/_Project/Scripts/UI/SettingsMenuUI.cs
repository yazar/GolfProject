using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsMenuUI : MonoBehaviour
{
    [SerializeField] NpcSettings settings;
    [SerializeField] TMP_InputField healthDurationField;
    [SerializeField] TMP_InputField carryCountField;
    
    public void OnClickApply()
    {
        settings.healthDuration = float.Parse(healthDurationField.text);
        settings.maxBallsToCarry = int.Parse(carryCountField.text);
        
        HideMenu();
        
        GameManager.Instance.OnReset?.Invoke();
    }
    
    public void OnDiscard()
    {
        healthDurationField.text =  settings.healthDuration.ToString();
        carryCountField.text = settings.maxBallsToCarry.ToString();
        
        HideMenu();
    }

    #region OpenClose
    
    public void ShowMenu()
    {
        healthDurationField.text =  settings.healthDuration.ToString();
        carryCountField.text = settings.maxBallsToCarry.ToString();
        
        gameObject.SetActive(true);
    }
    
    public void HideMenu()
    {
        gameObject.SetActive(false);
    }
    
    #endregion OpenClose
}

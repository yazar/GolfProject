using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenuUI : MonoBehaviour
{
    
    public void OnClickSettings()
    {
        GameManager.Instance.SettingsMenuUI.ShowMenu();
    }
    
    public void OnClickResume()
    {
        HideMenu();
    }
    
    public void OnClickExit()
    {
        Application.Quit();
    }
    
    #region OpenClose
    
    public void ToggleEscapeMenu()
    {
        if (GameManager.Instance.SettingsMenuUI.gameObject.activeInHierarchy)
        {
            GameManager.Instance.SettingsMenuUI.OnDiscard();
            return;
        }
        
        if (gameObject.activeInHierarchy)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }
    }
    
    private void ShowMenu()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }
    
    private void HideMenu()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    
    #endregion OpenClose
}

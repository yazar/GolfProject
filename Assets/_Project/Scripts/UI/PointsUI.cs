using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text countText;
    
    public void SetPointCountText(float points)
    {
        countText.text = points.ToString();
    }
}

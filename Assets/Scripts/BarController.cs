using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Image barImage;

    public void SetColor(Color color)
    {
        barImage.color = color;
    }

    public void SetPercentage(float percentage)
    {
        barImage.rectTransform.localScale = new Vector3(percentage, 1, 1);
    }
}

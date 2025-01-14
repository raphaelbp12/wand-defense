using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Image barImage;
    public TMPro.TMP_Text barText;

    private void Awake()
    {
        if (barText != null)
        {
            barText.text = "";
        }
    }

    public void SetColor(Color color)
    {
        barImage.color = color;
    }

    public void SetPercentage(float percentage)
    {
        barImage.rectTransform.localScale = new Vector3(percentage, 1, 1);
    }

    public void SetText(string text)
    {
        if (barText != null)
        {
            barText.text = text;
        }
    }
}

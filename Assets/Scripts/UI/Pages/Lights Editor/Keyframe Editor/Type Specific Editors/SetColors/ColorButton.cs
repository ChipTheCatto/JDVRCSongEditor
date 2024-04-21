using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColorButton : MonoBehaviour
{
    public int index = -1;
    public Image colorFill;
    public void SetColor(Color color)
    {
        colorFill.color = color;
    }


    public void RemoveColorButtonPressed()
    {
        KeyframeEditor.instance.keyframe.colors.RemoveAt(index);
        KeyframeEditor.instance.UpdateEditor();
    }


    public void OnColorButtonPressed()
    {
        if (index < 0 || index >= KeyframeEditor.instance.keyframe.colors.Count) return;
        SetColorsEditor.instance.ChangeColor(false, index);
    }
}

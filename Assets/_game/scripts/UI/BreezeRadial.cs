using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BreezeRadial : MonoBehaviour
{
    public Image background;
    public Image fill;
    public TextMeshProUGUI text;

    public void UpdateSelf(ThemeRadial _radial)
    {
        background.sprite = _radial.background;
        background.color = _radial.color;
        fill.sprite = _radial.fill;
        fill.color = _radial.color;
        text.color = _radial.color;
    }
}

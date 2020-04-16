using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ThemeItem : MonoBehaviour
{
    public ITEMTYPE type;

    public void UpdateSelf(Sprite _sprite)
    {
        switch (type)
        {
            case ITEMTYPE.BACKGROUND:
            case ITEMTYPE.PANEL:
            case ITEMTYPE.ICON_SETTINGS:
            case ITEMTYPE.ICON_LEADERBOARD:
            case ITEMTYPE.ICON_HOME:
            case ITEMTYPE.BUTTON_EXIT:
            default:
                GetComponent<Image>().sprite = _sprite;
                break;
        }
    }

    public void UpdateSelf(string _val){
        switch (type)
        {
            
            case ITEMTYPE.IAP_PRODUCTID:
                new NotImplementedException();
                break;
            
        }
    }

    public void UpdateSelf(ThemeText _text)
    {
        GetComponent<TextMeshProUGUI>().font = _text.font;
        if (_text.overrideColor)
        {
            GetComponent<TextMeshProUGUI>().color = _text.color;
        }
    }

    public void UpdateSelf(ThemeButton _playButton)
    {
        var button = GetComponent<Button>();
        button.targetGraphic.GetComponent<Image>().sprite = _playButton.active;
        var state = button.spriteState;
        state.pressedSprite = _playButton.pressed;
        state.disabledSprite = _playButton.disabled;
        button.spriteState = state;

    }

    internal void UpdateSelf(ThemeRadial _accuracyRadial)
    {
        GetComponent<BreezeRadial>().UpdateSelf(_accuracyRadial);
    }
}

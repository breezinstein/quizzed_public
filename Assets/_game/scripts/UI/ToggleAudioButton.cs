using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudioButton : MonoBehaviour
{
    public Image buttonIcon;
    public Sprite onSprite;
    public Sprite offSprite;
    public bool isSFX;

    // Use this for initialization
    void Start()
    {
        UpdateUI();
    }

    public void ToggleAudio()
    {
        if (isSFX)
        {
            AudioManager.ToggleSFX();
        }
        else
        {
            AudioManager.ToggleBGM();
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        if (isSFX)
        {
            buttonIcon.sprite = AudioManager.sfxEnabled ? onSprite : offSprite; 
        }
        else
        {
            buttonIcon.sprite = AudioManager.bgmEnabled ? onSprite : offSprite;
        }
        
    }
}

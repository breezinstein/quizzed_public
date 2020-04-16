using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Config", menuName = "Breeze/Theme", order = 1)]
public class QuizzedTheme : ScriptableObject
{
    [Header("Text")]
    public ThemeText titleText;
    public ThemeText headerText;
    public ThemeText buttonText;
    public ThemeText text;
    [Header("Sprites")]
    public Sprite background;
    public Sprite panel;

    [Header("Buttons")]
    public ThemeButton playButton;
    public ThemeButton quizButton;
    public ThemeButton defaultButton;
    public ThemeButton roundButton;
    public ThemeButton exitButton;

    [Header("Radial")]
    public ThemeRadial accuracyRadial;

    [Header("Icons")]
    public Sprite homeIcon;
    public Sprite settingsIcon;
    public Sprite leaderboardIcon;
    public Sprite shareIcon;

    [Header("Audio")]
    public string correctClip;
    public string incorrectClip;

    public void ApplyTheme()
    {
        //find all objects with ThemeItem component
        Scene scene = SceneManager.GetActiveScene();
        var root_objects = scene.GetRootGameObjects();

        foreach (GameObject root in root_objects)
        {
            foreach (var themeItem in root.transform.GetComponentsInChildren<ThemeItem>(true))
            {

                switch (themeItem.type)
                {
                    case ITEMTYPE.BACKGROUND:
                        themeItem.UpdateSelf(background);
                        break;
                    case ITEMTYPE.PANEL:
                        themeItem.UpdateSelf(panel);
                        break;
                    case ITEMTYPE.TEXT_HEADER:
                        themeItem.UpdateSelf(headerText);
                        break;
                    case ITEMTYPE.BUTTON_PLAY:
                        themeItem.UpdateSelf(playButton);
                        break;
                    case ITEMTYPE.BUTTON_QUIZ:
                        themeItem.UpdateSelf(quizButton);
                        break;
                    case ITEMTYPE.BUTTON_DEFAULT:
                        themeItem.UpdateSelf(defaultButton);
                        break;
                    case ITEMTYPE.BUTTON_ROUND:
                        themeItem.UpdateSelf(roundButton);
                        break;
                    case ITEMTYPE.ICON_SETTINGS:
                        themeItem.UpdateSelf(settingsIcon);
                        break;
                    case ITEMTYPE.ICON_LEADERBOARD:
                        themeItem.UpdateSelf(leaderboardIcon);
                        break;
                    case ITEMTYPE.ICON_HOME:
                        themeItem.UpdateSelf(homeIcon);
                        break;
                    case ITEMTYPE.TEXT_BUTTON:
                        themeItem.UpdateSelf(buttonText);
                        break;
                    case ITEMTYPE.TEXT:
                        themeItem.UpdateSelf(text);
                        break;
                    
                    case ITEMTYPE.BUTTON_EXIT:
                        themeItem.UpdateSelf(exitButton);
                        break;
                    case ITEMTYPE.ICON_SHARE:
                        themeItem.UpdateSelf(shareIcon);
                        break;
                    case ITEMTYPE.TEXT_TITLE:
                        themeItem.UpdateSelf(titleText);
                        break;
                    case ITEMTYPE.ACCURACY_RADIAL:
                        themeItem.UpdateSelf(accuracyRadial);
                        break;
                    default:
                        break;
                }
#if UNITY_EDITOR
                EditorFix.SetObjectDirty(themeItem);
#endif
            }
        }
    }
}

    [System.Serializable]
    public class ThemeButton
    {
        public Sprite active;
        public Sprite pressed;
        public Sprite disabled;
    }

    [System.Serializable]
    public class ThemeRadial
    {
        public Sprite background;
        public Sprite fill;
        public Color color = Color.white;
    }

    [System.Serializable]
    public class ThemeText
    {
        public TMP_FontAsset font;
        public bool overrideColor;
        public Color color = Color.white;
    }
    public enum ITEMTYPE
    {
        BACKGROUND, PANEL, TEXT_HEADER, TEXT_BUTTON, TEXT,
        BUTTON_PLAY, BUTTON_QUIZ, BUTTON_DEFAULT, BUTTON_ROUND, ICON_SETTINGS,
        ICON_LEADERBOARD, ICON_HOME, IAP_PRODUCTID, BUTTON_EXIT, ICON_SHARE,
        TEXT_TITLE, ACCURACY_RADIAL
    }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

#endif
[CreateAssetMenu(fileName = "Config", menuName = "Breeze/Config", order = 1)]
public class GameConfig : ScriptableObject
{
    public string mainTitle;
    public QuizzedTheme theme;
    public QuestionLibrary library;
    [Header("Build Settings")]
    public string productName;
    public string packageName;
    public bool overrideForIOS;
    public string productNameForIOS;
    public string packageNameForIOS;
    public Texture2D[] defaultIcon;

    [Header("Sharing")]
    public Texture2D shareImage;
    public string shareText = "Test your knowledge!";
    public string shareURL = "Download it here https://quizzed.page.link/geo";

    [Header("Gameplay")]
    public int roundSize;
    public float roundTime = 80f;
    //"https://opentdb.com/api.php?amount=10&category=22&encode=base64"

    [Header("IAP")]
    public string androidProductID = "remove.ads";
    public string iOSProductID = "remove.ads";

    [Header("Notification")]
    public string title = "Q:Geography";
    public string body = "Refresh your knowledge today!";
    public string smallIconName = "myicon.png";

#if UNITY_EDITOR
    [ContextMenu("Set Build Settings")]
    public void SetBuildSettings()
    {
        UnityEditor.PlayerSettings.productName = productName;
        UnityEditor.PlayerSettings.applicationIdentifier = packageName;
#if UNITY_IOS
        if (overrideForIOS)
        {
            UnityEditor.PlayerSettings.productName = productNameForIOS;
            UnityEditor.PlayerSettings.applicationIdentifier = packageNameForIOS;
        }
#endif
        UnityEditor.PlayerSettings.SetIconsForTargetGroup(UnityEditor.BuildTargetGroup.Unknown, defaultIcon);
        FindObjectOfType<GameManager>().config = this;
        GameObject.Find("TitleText").GetComponent<TextMeshProUGUI>().text = mainTitle;

        ApplyTheme();

    }
#endif

    public void ApplyTheme()
    {
        theme.ApplyTheme();
        //find all objects with ThemeItem component
        Scene scene = SceneManager.GetActiveScene();
        var root_objects = scene.GetRootGameObjects();

        foreach (GameObject root in root_objects)
        {
            foreach (var themeItem in root.transform.GetComponentsInChildren<ThemeItem>(true))
            {

                switch (themeItem.type)
                {
                    case ITEMTYPE.IAP_PRODUCTID:
#if UNITY_IOS
                        themeItem.UpdateSelf(iOSProductID);
#elif UNITY_ANDROID
                        themeItem.UpdateSelf(androidProductID);
#endif
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

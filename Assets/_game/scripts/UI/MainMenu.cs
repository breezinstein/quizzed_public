using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Window
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject modePanel;
    [SerializeField] BreezeButton classicButton;
    [SerializeField] BreezeButton marathonButton;
    [SerializeField] BreezeButton settingsButton;

    [Space]
    public VerticalLayoutGroup layoutGroup;
    [SerializeField] BreezeButton playEasyButton;
    [SerializeField] BreezeButton playMediumButton;
    [SerializeField] BreezeButton playHardButton;

    [Space]
    [SerializeField] GameObject quitPanel;
    [SerializeField] BreezeButton quitButton;
    [SerializeField] BreezeButton quitCancel;
    [SerializeField] BreezeButton quitClose;

    [Space]
    [SerializeField] BreezeButton leaderboardButton;
    [SerializeField] string leaderboardID;

    protected override void Awake()
    {
        base.Awake();
        classicButton.onClick.AddListener(() =>
        {
            layoutGroup.gameObject.SetActive(true);
            modePanel.SetActive(false);
        });
        marathonButton.onClick.AddListener(PlayMarathonGame);
        playEasyButton.onClick.AddListener(PlayEasyGame);
        playMediumButton.onClick.AddListener(PlayMediumGame);
        playHardButton.onClick.AddListener(PlayHardGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(Quit);
        quitCancel.onClick.AddListener(Cancel);
        quitClose.onClick.AddListener(Cancel);
    }

    private void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (quitPanel.activeInHierarchy)
            {
                Cancel();
            }
            else
            {
                ShowQuit();
            }
        }
    }

    public override void Open()
    {
        base.Open();
        StartCoroutine(ToggleToggleGroup());
    }

    void PlayEasyGame()
    {
        GameManager.Instance.gameplayWindow.difficulty = (DIFFICULTY.EASY);
        GameManager.Instance.gameplayWindow.Open();
    }

    void PlayMediumGame()
    {
        GameManager.Instance.gameplayWindow.difficulty = (DIFFICULTY.MEDIUM);
        GameManager.Instance.gameplayWindow.Open();
    }

    void PlayHardGame()
    {
        GameManager.Instance.gameplayWindow.difficulty = (DIFFICULTY.HARD);
        GameManager.Instance.gameplayWindow.Open();
    }

    void PlayMarathonGame()
    {
        GameManager.Instance.gameplayWindow.difficulty = (DIFFICULTY.MARATHON);
        GameManager.Instance.gameplayWindow.Open();
    }

    void OpenSettings()
    {
        GameManager.Instance.settingsWindow.Open();
    }

    void ShowQuit()
    {
        quitPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    void Quit()
    {
        Application.Quit();
    }

    void Cancel()
    {
        quitPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    IEnumerator ToggleToggleGroup()
    {
        layoutGroup.enabled = true;
        yield return new WaitForEndOfFrame();
        layoutGroup.enabled = false;
    }
}

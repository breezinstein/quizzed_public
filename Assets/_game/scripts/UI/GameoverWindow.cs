using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameoverWindow : Window
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI correctAnswerText;
    [SerializeField] TextMeshProUGUI incorrectAnswerText;
    [SerializeField] TextMeshProUGUI accuracyText;

    [Header("Buttons")]
    [SerializeField] BreezeButton shareButton;
    [SerializeField] BreezeButton retryButton;
    [SerializeField] BreezeButton closeButton;

    [Header("Stars")]
    [SerializeField] Image leftStarOn;
    [SerializeField] Image leftStarOff;
    [SerializeField] Image rightStarOn;
    [SerializeField] Image rightStarOff;
    [SerializeField] Image middleStarOn;
    [SerializeField] Image middleStarOff;

    Stats localStats;
    DIFFICULTY difficulty;

    protected override void Awake()
    {
        base.Awake();
        shareButton.onClick.AddListener(Share);
        retryButton.onClick.AddListener(Retry);
        closeButton.onClick.AddListener(Close);
    }
    public override void Open()
    {
        base.Open();
    }

    public void SetUI(Stats _localStats, Stats stats, DIFFICULTY _difficulty)
    {
        localStats = _localStats;
        difficulty = _difficulty;
        if (_difficulty != DIFFICULTY.MARATHON)
        { stats.correct += _localStats.correct; }
        else
        {
            if (_localStats.correct > stats.correct)
            {
                stats.correct = _localStats.correct;
            }
        }
        stats.incorrect += _localStats.incorrect;
        if (_localStats.score > stats.score)
        {
            stats.score = _localStats.score;
        }
        GameManager.Instance.config.library.SaveStats();

        scoreText.text = "Score: " + _localStats.score;
        correctAnswerText.text = string.Format("Correct Answers: {0}", _localStats.correct);
        incorrectAnswerText.text = string.Format("Incorrect Answers: {0}", _localStats.incorrect);
        accuracyText.text = string.Format("Accuracy: {0}%", (int)(_localStats.Accuracy*100f));

        //Star region
        if (_localStats.Accuracy >= 0.95f)
        {
            leftStarOff.enabled = false;
            middleStarOff.enabled = false;
            rightStarOff.enabled = false;
        }
        else if (_localStats.Accuracy >= 0.70f)
        {
            leftStarOff.enabled = false;
            middleStarOff.enabled = false;
            rightStarOff.enabled = true;
        }
        else if (_localStats.Accuracy >= 0.45f)
        {
            leftStarOff.enabled = false;
            middleStarOff.enabled = true;
            rightStarOff.enabled = true;
        }
        else
        {
            leftStarOff.enabled = true;
            rightStarOff.enabled = true;
            middleStarOff.enabled = true;
        }

        leftStarOn.enabled = !leftStarOff.enabled;
        middleStarOn.enabled = !middleStarOff.enabled;
        rightStarOn.enabled = !rightStarOff.enabled;

    }

    public void Share()
    {
         new System.NotImplementedException();
    }

    public void Retry()
    {
        GameManager.Instance.gameplayWindow.Open();
    }

    public override void Close()
    {
        base.Close();
    }
}

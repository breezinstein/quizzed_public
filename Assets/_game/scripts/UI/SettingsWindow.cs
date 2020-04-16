using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsWindow : Window
{
    public BreezeButton restorePurchasesButton;
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    public GameObject statsPanel;
    public GameObject statsItemPanel;

    [Header("Difficulties")]
    public BreezeButton easyButton;
    public BreezeButton mediumButton;
    public BreezeButton hardButton;
    public StatsMenuItem statsItem;
    [Space]
    public Image easyFilledImage;
    public Image mediumFilledImage;
    public Image hardFilledImage;
    [Space]
    public TextMeshProUGUI easyAccuracyText;
    public TextMeshProUGUI mediumAccuracyText;
    public TextMeshProUGUI hardAccuracyText;

    [Header("Overview")]
    public TextMeshProUGUI overviewAccuracyText;
    public Image overviewFilledImage;
    public TextMeshProUGUI overviewBestScoreText;
    public TextMeshProUGUI overviewQuestionText;

    [Header("Marathon")]
    public TextMeshProUGUI marathonBestStreakText;
    public TextMeshProUGUI marathonBestScoreText;

    [Header("Buttons")]
    public BreezeButton shareButton;
    public BreezeButton otherShareButton;
    public BreezeButton resetStatsButton;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            restorePurchasesButton.gameObject.SetActive(true);
        }
        else
        {
            restorePurchasesButton.gameObject.SetActive(false);
        }

        easyButton.onClick.AddListener(() =>
        {
            statsItem.UpdateText(GameManager.Instance.config.library.easyStats, "Easy Stats");
            statsItemPanel.SetActive(true);
            statsPanel.SetActive(false);

        });
        mediumButton.onClick.AddListener(() =>
        {
            statsItem.UpdateText(GameManager.Instance.config.library.mediumStats, "Medium Stats");
            statsItemPanel.SetActive(true);
            statsPanel.SetActive(false);
        });
        hardButton.onClick.AddListener(() =>
        {
            statsItem.UpdateText(GameManager.Instance.config.library.hardStats, "Hard Stats");
            statsItemPanel.SetActive(true);
            statsPanel.SetActive(false);
        });

        shareButton.onClick.AddListener(() =>
        {
            Share();
        });

        otherShareButton.onClick.AddListener(() =>
        {
            Share();
        });

        resetStatsButton.onClick.AddListener(() =>
        {
            Reset();
        });
    }

    public void RemoveAds()
    {
        PlayerPrefs.SetInt("AdsEnabled", 0);
        PlayerPrefs.Save();
    }

    public override void Open()
    {
        base.Open();
        

        UpdateUI();
    }

    void UpdateUI()
    {
        var library = GameManager.Instance.config.library;
        float totalQuestions = library.easyStats.correct + library.easyStats.incorrect +
            library.mediumStats.correct + library.mediumStats.incorrect +
            library.hardStats.correct + library.hardStats.incorrect +
            library.marathonStats.correct + library.marathonStats.incorrect;
        float totalCorrect = library.easyStats.correct + library.mediumStats.correct + library.hardStats.correct + library.marathonStats.correct;
        float accuracy = 0f;
        if (totalQuestions > 0)
        {
            accuracy = totalCorrect / totalQuestions;
        }
        int bestScore = library.easyStats.score;
        if (library.hardStats.score > library.mediumStats.score)
        {
            bestScore = library.hardStats.score;
        }
        else if (library.mediumStats.score > library.easyStats.score)
        {
            bestScore = library.mediumStats.score;
        }
        else
        {
            bestScore = library.easyStats.score;
        }
        overviewAccuracyText.text = string.Format("{0,0:0}%", accuracy * 100);
        overviewFilledImage.fillAmount = accuracy;
        overviewBestScoreText.text = bestScore.ToString();
        overviewQuestionText.text = totalQuestions.ToString();

        easyFilledImage.fillAmount = library.easyStats.Accuracy;
        mediumFilledImage.fillAmount = library.mediumStats.Accuracy;
        hardFilledImage.fillAmount = library.hardStats.Accuracy;

        easyAccuracyText.text = string.Format("{0,0:0}%", library.easyStats.Accuracy * 100);
        mediumAccuracyText.text = string.Format("{0,0:0}%", library.mediumStats.Accuracy * 100);
        hardAccuracyText.text = string.Format("{0,0:0}%", library.hardStats.Accuracy * 100);

        marathonBestStreakText.text = library.marathonStats.correct.ToString();
        marathonBestScoreText.text = library.marathonStats.score.ToString();
    }

    public void Share()
    {
        new System.NotImplementedException();
    }

    public void Reset()
    {
        GameManager.ShowMessage("This will reset all your stats", false, () => {
            GameManager.Instance.config.library.ClearStats();
            GameManager.Instance.config.library.SaveStats();
            UpdateUI();
        });
    }

    [System.Serializable]
    public class StatsMenuItem
    {
        public TextMeshProUGUI headerText;
        public TextMeshProUGUI accuracyText;
        public Image filledImage;
        public TextMeshProUGUI correctText;
        public TextMeshProUGUI incorrectText;
        public TextMeshProUGUI highscoreText;

        public void UpdateText(Stats stats, string _header)
        {
            headerText.text = _header;
            if (stats.Accuracy >= 0.01f)
            { accuracyText.text = string.Format("{0,0:0}%", stats.Accuracy * 100); }
            else
            {
                accuracyText.text = string.Format("{0,0:0}%", 0);
            }
            filledImage.fillAmount = stats.Accuracy;
            correctText.text = string.Format("{0}", stats.correct);
            incorrectText.text = string.Format("{0}", stats.incorrect);
            highscoreText.text = string.Format("{0}", stats.score);
        }
    }
}

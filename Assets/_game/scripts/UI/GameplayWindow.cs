using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class GameplayWindow : Window
{
    public VerticalLayoutGroup layoutGroup;
    QuestionLibrary library;
    List<Question> questions;
    Stats stats;
    public DIFFICULTY difficulty;
    public int currentQuestion = 0;
    [Header("UI")]
    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Slider timeSlider;
    [SerializeField] List<BreezeButton> buttons;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] BreezeButton closeButton;
    [SerializeField] Image correctIndicator;
    [SerializeField] Image incorrectIndicator;
    [SerializeField] BreezeButton shareButton;
    BreezeButton CorrectButton;
    [SerializeField] TextMeshProUGUI livesText;

    [Header("Settings")]
    public int roundSize;
    public float roundTime;
    float _roundTime = 0f;
    bool _timerEnabled = false;
    Stats localstats;
    private int lives = 3;

    private IEnumerator Start()
    {
        closeButton.onClick.AddListener(() => { GameManager.Instance.mainMenu.Open(); });
        shareButton.onClick.AddListener(Share);
        layoutGroup.enabled = true;
        yield return new WaitForEndOfFrame();
        layoutGroup.enabled = false;
    }

    void GenerateQuestion()
    {
        if (isEnded()) { return; }
        UpdateStatsIndex();
        var temp = questions[stats.questionIndex];

        //Setup Texts
        questionText.text = temp.question;
        SetupOptions(temp);

        StartCoroutine(ToggleToggleGroup());
        if (difficulty != DIFFICULTY.MARATHON)
        {
            timeSlider.maxValue = roundTime;
            _roundTime = roundTime;
        }

        //Shuffle Buttons
        UpdateUI();
        _timerEnabled = true;
    }

    void SetupOptions(Question temp)
    {
        //Debug.Log(temp.correct_answer);
        switch (temp.type)
        {
            case "boolean":
                SetButtonText(buttons[0], "True");
                buttons[0].transform.SetAsFirstSibling();
                SetButtonText(buttons[1], "False");
                buttons[1].transform.SetAsLastSibling();
                EnableButton(buttons[2], false);
                EnableButton(buttons[3], false);

                RemoveAllButtonListeners();

                if (buttons[0].GetComponentInChildren<TextMeshProUGUI>().text == temp.correct_answer)
                {
                    buttons[0].onClick.AddListener(CorrectAnswer);
                    CorrectButton = buttons[0];
                }
                else
                {
                    buttons[0].onClick.AddListener(() => { IncorrectAnswer(buttons[0]); });
                }
                if (buttons[1].GetComponentInChildren<TextMeshProUGUI>().text == temp.correct_answer)
                {
                    buttons[1].onClick.AddListener(CorrectAnswer);
                    CorrectButton = buttons[1];
                }
                else
                {
                    buttons[1].onClick.AddListener(() => { IncorrectAnswer(buttons[1]); });
                }
                break;
            case "multiple":
                EnableAllButtons(true);

                SetButtonText(buttons[0], temp.incorrect_answers[0]);
                SetButtonText(buttons[1], temp.incorrect_answers[1]);
                SetButtonText(buttons[2], temp.incorrect_answers[2]);
                SetButtonText(buttons[3], temp.correct_answer);

                //Setup Actions
                RemoveAllButtonListeners();

                buttons[0].onClick.AddListener(() => { IncorrectAnswer(buttons[0]); });
                buttons[1].onClick.AddListener(() => { IncorrectAnswer(buttons[1]); });
                buttons[2].onClick.AddListener(() => { IncorrectAnswer(buttons[2]); });
                buttons[3].onClick.AddListener(CorrectAnswer);

                CorrectButton = buttons[3];
                foreach (var item in buttons)
                {
                    item.transform.SetSiblingIndex(UnityEngine.Random.Range(0, buttons.Count));
                }
                //BreezeHelpers.ShuffleList(buttons);
                break;
        }
        SetButtonsInteractivity(true);
    }

    public void SetQuestions()
    {
        switch (difficulty)
        {
            case DIFFICULTY.EASY:
                questions = library.easyQuestions;
                stats = library.easyStats;
                break;
            case DIFFICULTY.MEDIUM:
                questions = library.mediumQuestions;
                stats = library.mediumStats;
                break;
            case DIFFICULTY.HARD:
                questions = library.hardQuestions;
                stats = library.hardStats;
                break;
            case DIFFICULTY.MARATHON:
                questions = library.marathonQuestions;
                stats = library.marathonStats;
                break;
            default:
                questions = library.easyQuestions;
                stats = library.easyStats;
                break;
        }

    }

    void SetButtonText(Button _button, string _val)
    {
        _button.GetComponentInChildren<TextMeshProUGUI>().text = _val;
    }



    void RemoveAllButtonListeners()
    {
        foreach (var button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    void EnableAllButtons(bool _val)
    {
        foreach (var button in buttons)
        {
            EnableButton(button, _val);
        }
    }

    void EnableButton(Button _button, bool _val)
    {
        _button.gameObject.SetActive(_val);
    }

    void SetButtonsInteractivity(bool _val)
    {
        foreach (var button in buttons)
        {
            button.interactable = _val;
        }
    }

    void IncorrectAnswer(BreezeButton _button = null)
    {
        SetButtonsInteractivity(false);

        StartCoroutine(DoIncorrect(_button));

    }
    IEnumerator DoIncorrect(BreezeButton _button = null)
    {
        AudioManager.PlaySFX("incorrect");
        CorrectButton.targetGraphic.DOColor(Color.green, 0.5f).OnComplete(() => { CorrectButton.targetGraphic.color = Color.white; });
        if (_button != null)
        {
            _button.targetGraphic.DOColor(Color.red, 0.5f).OnComplete(() => { _button.targetGraphic.color = Color.white; });
        }

        yield return new WaitForSeconds(0.75f);

        localstats.incorrect++;
        currentQuestion++;
        stats.questionIndex++;
        if (difficulty == DIFFICULTY.MARATHON)
        { lives--; }
        GenerateQuestion();
    }
    void CorrectAnswer()
    {
        SetButtonsInteractivity(false);

        StartCoroutine(DoCorrect());

    }
    IEnumerator DoCorrect()
    {
        AudioManager.PlaySFX("correct");
        //Add score
        CorrectButton.targetGraphic.DOColor(Color.green, 0.75f).OnComplete(() => { CorrectButton.targetGraphic.color = Color.white; });

        yield return new WaitForSeconds(0.75f);
        localstats.score += (int)(_roundTime * (1 + ((int)difficulty * 0.5f)));
        localstats.correct++;
        currentQuestion++;
        stats.questionIndex++;
        GenerateQuestion();
    }

    bool isEnded()
    {
        if (difficulty != DIFFICULTY.MARATHON)
        {
            if (currentQuestion > roundSize - 1)
            {
                EndGame();
                return true;
            }
            else return false;
        }
        else if (difficulty == DIFFICULTY.MARATHON)
        {
            if (_roundTime <= 0f)
            {
                EndGame();
                return true;
            }
            if (lives <= 0)
            {
                EndGame();
                return true;
            }
            else return false;
        }
        else return false;
    }

    private void EndGame()
    {
        GameManager.Instance.gameoverWindow.SetUI(localstats, stats, difficulty);

        GameManager.Instance.gameoverWindow.Open();
    }

    private void FixedUpdate()
    {

        timeSlider.value = _roundTime;
        if (_timerEnabled)
        {
            _roundTime -= Time.fixedDeltaTime;
            if (_roundTime <= 0f)
            {
                if (isEnded())
                {

                }
                else
                {
                    if (difficulty == DIFFICULTY.MARATHON)
                    {

                    }
                    else
                    {
                        localstats.incorrect++;
                        currentQuestion++;
                        stats.questionIndex++;
                        GenerateQuestion();
                    }
                }
            }
        }
        _roundTime = Mathf.Clamp(_roundTime, 0, roundTime);

    }

    void UpdateStatsIndex()
    {
        library.SaveStats();
        if (stats.questionIndex >= questions.Count)
        {
            stats.questionIndex = 0;
            library.ShuffleQuestions();
        }
    }

    void UpdateUI()
    {
        progressText.text = string.Format("{0}/{1}", currentQuestion, roundSize);
        scoreText.text = localstats.score.ToString();
        if (difficulty == DIFFICULTY.MARATHON)
        {
            livesText.text = string.Format("Lives: {0}", lives-1);
        }
    }

    public override void Open()
    {
        base.Open();
        roundSize = GameManager.Instance.config.roundSize;
        roundTime = GameManager.Instance.config.roundTime;
        if (difficulty == DIFFICULTY.MARATHON)
        {
            timeSlider.maxValue = roundTime;
            _roundTime = roundTime;
            lives = 3;
            livesText.gameObject.SetActive(true);
        }
        else
        {
            livesText.gameObject.SetActive(false);
        }
        library = GameManager.Instance.PlayQuestionSet();
        currentQuestion = 0;
        localstats = new Stats();
        SetQuestions();
        GenerateQuestion();
        
    }

    public override void Close()
    {

        base.Close();

    }

    public void Share()
    {
        new NotImplementedException();
    }



    DateTime pauseTime = new DateTime();
    void OnApplicationPause(bool pause)
    {

        if (_timerEnabled)
        {
            float duration = _roundTime;
            if (pause)
            {
                pauseTime = DateTime.Now;
                Debug.Log("We got the current time");
            }
            else
            {
                //Debug.Log((pauseTime + duration) + " was when we paused and we are not at " + Time.realtimeSinceStartup);
                if (pauseTime != null)
                {
                    float timeSpent = (float)(DateTime.Now - pauseTime).TotalSeconds;
                    Debug.Log("we've been gone for this long " + timeSpent);
                    if (timeSpent > _roundTime)
                    {
                        GameManager.ShowMessage("You left the game");
                        _roundTime -= timeSpent;
                    }
                    else
                    {
                        _roundTime -= timeSpent;
                    }
                }
            }
        }
    }

    IEnumerator ToggleToggleGroup()
    {
        layoutGroup.enabled = true;
        yield return new WaitForEndOfFrame();
        layoutGroup.enabled = false;
    }

}


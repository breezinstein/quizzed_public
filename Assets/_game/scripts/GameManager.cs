using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameConfig config;
    public bool hasQuestionSet;
    [SerializeField] QuestionSet questionSet;

    [Header("UI")]
    [SerializeField] MessageBox messageBox;

    [Header("Windows")]
    public MainMenu mainMenu;
    public GameplayWindow gameplayWindow;
    public SettingsWindow settingsWindow;
    public GameoverWindow gameoverWindow;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        if (config == null)
        {
            Debug.LogError("Please add a config file");
        }

        config.library.Initialize(config.productName);
    }


    public QuestionLibrary PlayQuestionSet()
    {
        return config.library;
    }

    public static void ShowMessage(string _message, bool _isLoading = false, UnityAction okayAction = null)
    {
        Instance.messageBox.Close();
        Instance.messageBox.SetMessage(_message, _isLoading, okayAction);
        Instance.messageBox.Open();
    }

    public static void CloseMessage()
    {
        Instance.messageBox.Close();
    }

}

public enum DIFFICULTY { EASY, MEDIUM, HARD, MARATHON }

[Serializable]
public class Question
{
    public string category;
    public string type;
    public string difficulty;
    public string question;
    public string correct_answer;
    public List<string> incorrect_answers;
}

[Serializable]
public class QuestionSet
{
    public int response_code;
    public List<Question> results;

    public static QuestionSet CreateFromJSON(string jsonString)
    {
        var temp = JsonUtility.FromJson<QuestionSet>(jsonString);
        foreach (var item in temp.results)
        {
            item.category = Decode(item.category);
            item.type = Decode(item.type);
            item.difficulty = Decode(item.difficulty);
            item.question = Decode(item.question);
            item.correct_answer = Decode(item.correct_answer);
            for (int i = 0; i < item.incorrect_answers.Count; i++)
            {
                item.incorrect_answers[i] = Decode(item.incorrect_answers[i]);
            }
        }
        return temp;
    }

    static string Decode(string encodedText)
    {
        byte[] decodedBytes = Convert.FromBase64String(encodedText);
        return Encoding.UTF8.GetString(decodedBytes);
    }
}

[System.Serializable]
public class Stats
{
    public int correct;
    public int incorrect;
    public int questionIndex;
    public float Accuracy
    {
        get
        {
            if (correct + incorrect == 0) { return 0; }
            else { return (float)(correct * 100f) / (float)((correct + incorrect) * 100f); }
        }
    }
    public int score;
}
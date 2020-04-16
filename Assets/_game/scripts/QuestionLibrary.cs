using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question Library", menuName = "Breeze/Library", order = 1)]
public class QuestionLibrary : ScriptableObject
{
    public TextAsset jsonFile;
    string saveID;
    public CATEGORY category;
    [Header("Stats")]
    public Stats easyStats;
    public Stats mediumStats;
    public Stats hardStats;
    public Stats marathonStats;
    [Header("Questions")]
    public List<Question> easyQuestions;
    public List<Question> mediumQuestions;
    public List<Question> hardQuestions;
    public List<Question> marathonQuestions;

    public void Initialize(string _saveID){
        saveID = _saveID;
        LoadStats();
    }

    public void LoadStats(){
        easyStats = JsonUtility.FromJson<Stats>(PlayerPrefs.GetString(saveID + "_easy", JsonUtility.ToJson(new Stats())));
        mediumStats = JsonUtility.FromJson<Stats>(PlayerPrefs.GetString(saveID + "_medium", JsonUtility.ToJson(new Stats())));
        hardStats = JsonUtility.FromJson<Stats>(PlayerPrefs.GetString(saveID + "_hard", JsonUtility.ToJson(new Stats())));
        marathonStats = JsonUtility.FromJson<Stats>(PlayerPrefs.GetString(saveID + "_marathon", JsonUtility.ToJson(new Stats())));
    }

    public void SaveStats(){
        PlayerPrefs.SetString(saveID+"_easy",JsonUtility.ToJson(easyStats));
        PlayerPrefs.SetString(saveID + "_medium", JsonUtility.ToJson(mediumStats));
        PlayerPrefs.SetString(saveID + "_hard", JsonUtility.ToJson(hardStats));
        PlayerPrefs.SetString(saveID + "_marathon", JsonUtility.ToJson(marathonStats));
        PlayerPrefs.Save();
    }

    public void ClearStats(){
        easyStats = new Stats();
        mediumStats = new Stats();
        hardStats = new Stats();
        marathonStats = new Stats();
    }

    public void GenerateQuestions()
    {
        easyQuestions= new List<Question>();
        mediumQuestions= new List<Question>();
        hardQuestions= new List<Question>();
        marathonQuestions = new List<Question>();

        foreach (var item in JsonUtility.FromJson<QuestionSet>(jsonFile.text).results)
        {
            if(category == CATEGORY.All){
                        marathonQuestions.Add(item);
                switch (item.difficulty)
                {
                    case "easy":
                        easyQuestions.Add(item);
                        break;
                    case "medium":
                        mediumQuestions.Add(item);
                        break;
                    case "hard":
                        hardQuestions.Add(item);
                        break;
                }
            }
            else{
                if(category == CategoryFromString(item.category)){
                    marathonQuestions.Add(item);
                    switch (item.difficulty)
                    {
                        case "easy":
                            easyQuestions.Add(item);
                            break;
                        case "medium":
                            mediumQuestions.Add(item);
                            break;
                        case "hard":
                            hardQuestions.Add(item);
                            break;
                    }
                }
            }
        }
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }

    public void ShuffleQuestions(){
        BreezeHelpers.ShuffleList(easyQuestions);
        BreezeHelpers.ShuffleList(mediumQuestions);
        BreezeHelpers.ShuffleList(hardQuestions);
        BreezeHelpers.ShuffleList(marathonQuestions);
    }

    CATEGORY CategoryFromString(string _category)
    {
        CATEGORY temp = CATEGORY.All;
        switch (_category)
        {
            case "Entertainment: Video Games":
                temp = CATEGORY.Video_Games;
                break;
            case "Entertainment: Music":
                temp = CATEGORY.Music;
                break;
            case "History":
                temp = CATEGORY.History;
                break;
            case "Geography":
                temp = CATEGORY.Geography;
                break;
            case "General Knowledge":
                temp = CATEGORY.General_Knowledge;
                break;
            case "Entertainment: Film":
                temp = CATEGORY.Film;
                break;
            case "Science & Nature":
                temp = CATEGORY.Science_And_Nature;
                break;
            case "Entertainment: Japanese Anime & Manga":
                temp = CATEGORY.Anime_And_Manga;
                break;
            case "Entertainment: Television":
                temp = CATEGORY.Television;
                break;
            case "Science: Computers":
                temp = CATEGORY.Computers;
                break;
            case "Entertainment: Books":
                temp = CATEGORY.Books;
                break;
            case "Sports":
                temp = CATEGORY.Sports;
                break;
            case "Entertainment: Cartoon & Animations":
                temp = CATEGORY.Cartoon_And_Animations;
                break;
            case "Vehicles":
                temp = CATEGORY.Vehicles;
                break;
            case "Animals":
                temp = CATEGORY.Animals;
                break;
            case "Entertainment: Board Games":
                temp = CATEGORY.Board_Games;
                break;
            case "Politics":
                temp = CATEGORY.Politics;
                break;
            case "Celebrities":
                temp = CATEGORY.Celebrities;
                break;
            case "Entertainment: Comics":
                temp = CATEGORY.Comics;
                break;
            case "Mythology":
                temp = CATEGORY.Mythology;
                break;
            case "Science: Mathematics":
                temp = CATEGORY.Mathematics;
                break;
            case "Entertainment: Musicals & Theatres":
                temp = CATEGORY.Musicals_And_Theatres;
                break;
            case "Art":
                temp = CATEGORY.Art;
                break;
            case "Science: Gadgets":
                temp = CATEGORY.Gadgets;
                break;
            default:
                temp = CATEGORY.All;
                break;
        }
        return temp;
    }

    public enum CATEGORY
    {
        All, Video_Games, Music, History, Geography, General_Knowledge, Film, Science_And_Nature, Anime_And_Manga, Television, Computers,
        Books, Sports, Cartoon_And_Animations, Vehicles, Animals, Board_Games, Politics, Celebrities, Comics, Mythology, Mathematics,
        Musicals_And_Theatres, Art, Gadgets
    }
}

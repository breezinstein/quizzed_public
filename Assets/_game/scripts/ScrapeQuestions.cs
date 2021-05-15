using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Networking;

public class ScrapeQuestions : MonoBehaviour
{
    public int totalQuestions;
    public string accessToken;
    public int timesToRun;
    public int finalRunAmount;

    string url = "https://opentdb.com/api.php?encode=base64";
    public QuestionSet set;
    // Start is called before the first frame update
    string generateURL(int amount)
    {
        return url + string.Format("&amount={0}&token={1}", amount, accessToken);
    }

    IEnumerator GetAccessToken()
    {
        string temp = "https://opentdb.com/api_token.php?command=request";
        UnityWebRequest www =  UnityWebRequest.Get(temp);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            try
            {
                accessToken = JsonUtility.FromJson<GetTokenObject>(www.downloadHandler.text).token;
                Debug.Log("Access Token:"+accessToken);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log("Failure getting accessToken");
                throw;
            }
        }
        yield return null;
    }

    IEnumerator GetTotalQuestions()
    {
        string temp = "https://opentdb.com/api_count_global.php";
        UnityWebRequest www = UnityWebRequest.Get(temp);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            try
            {
                totalQuestions = JsonUtility.FromJson<TotalQuestions>(www.downloadHandler.text).overall.total_num_of_verified_questions;
                Debug.Log("Verified Questions:" + totalQuestions);
                timesToRun = totalQuestions / 50;
                finalRunAmount = totalQuestions % 50;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log("Failure getting global count");
                throw;
            }
        }
        yield return null;
    }
    IEnumerator ResetAssesToken(string _token)
    {
        yield return null;
        string temp = "https://opentdb.com/api_token.php?command=reset&token=" + _token;
    }

    IEnumerator GetText(int _timesToRun, int _finalRunAmount)
    {
        for (int i = 0; i <= _timesToRun; i++)
        {
            UnityWebRequest www;
            if (i == _timesToRun)
            {
                www = UnityWebRequest.Get(generateURL(_finalRunAmount));
            }else{
                 www = UnityWebRequest.Get(generateURL(50));
            }

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);

            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
                var questionSet = QuestionSet.CreateFromJSON(www.downloadHandler.text);
                set.results.AddRange(questionSet.results);
            }
            Debug.Log(string.Format("{0}/{1} done", i, _timesToRun));
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Start()
    {
        set = new QuestionSet();
        set.results = new List<Question>();
        yield return GetAccessToken();
        yield return GetTotalQuestions();
        yield return GetText(timesToRun, finalRunAmount);
    }

#if UNITY_EDITOR
    [ContextMenu("Get json")]
    public void GetQuestionsInJSON(){
        
            string temporaryTextFileName = "questions";
        File.WriteAllText(Application.dataPath + "/Resources/" + temporaryTextFileName + ".json", JsonUtility.ToJson(set));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            

    }
#endif

    [System.Serializable]
    public class GetTokenObject
    {
        public int response_code;
        public string response_message;
        public string token;
    }

    [System.Serializable]
    public class TotalQuestions
    {
        public Overall overall;
        public Dictionary<string, Overall> categories;
    }

    [System.Serializable]
    public class Overall
    {
        public int total_num_of_questions;
        public int total_num_of_pending_questions;
        public int total_num_of_verified_questions;
        public int total_num_of_rejected_questions;
    }

}

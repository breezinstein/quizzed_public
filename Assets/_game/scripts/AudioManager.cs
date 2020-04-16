using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public static bool sfxEnabled = true;
    public static bool bgmEnabled = true;

    [Header("BGM")]
    public AudioSource bgmSource;
    public List<SourceItem> bgmItems;

    [Header("SFX")]
    public AudioSource sfxSource;
    public List<SourceItem> sfxItems;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Audio Manager instance found at " + instance.name + " , destroying this instance");
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        sfxEnabled = PlayerPrefs.GetInt("sfxEnabled", 1)==1?true:false;
        bgmEnabled = PlayerPrefs.GetInt("bgmEnabled", 1) == 1 ? true : false;
    }

    AudioClip GetClip(string clipName, bool isBGM = false)
    {
        AudioClip clip = null;
        try
        {
            var clips = (isBGM) ? bgmItems : sfxItems;
            clip = clips.Find(i => i.clipName == clipName).audioClip;
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(clipName + " not found");
        }
        finally
        {
            
        }
        return clip;
    }

    public static void PlaySFX(string clipName)
    {
        instance.PlaySoundEffect(clipName);
    }

    public static void PlayBGM(string clipName, bool overwrite = false)
    {
        instance.PlayBackroundMusic(clipName,overwrite);
    }

    public static void ResumeBGM()
    {
        if (instance.bgmSource.clip == null && instance.bgmItems.Count != 0)
        {
            instance.bgmSource.clip = instance.bgmItems[0].audioClip;
        }
        instance.bgmSource.Play();
    }

    void PlaySoundEffect(string clipName)
    {
        if (!sfxEnabled)
        {
            return;
        }
        sfxSource.clip = GetClip(clipName);
        sfxSource.Play();
        //sfxSource.PlayOneShot(GetClip(clipName));
    }

     void PlayBackroundMusic(string clipName, bool overwrite = false)
    {
        if (!bgmEnabled)
        {
            return;
        }
        if (bgmSource.isPlaying && overwrite == false)
        {
            //do not overwrite track
            return;
        }

        //check if clip is already playing
        if (overwrite)
        {
            if(bgmSource.clip == GetClip(clipName, true))
            {
                return;
            }
        }
        bgmSource.clip = GetClip(clipName, true);
        
        bgmSource.Play();
    }

    void StopBGM()
    {
        bgmSource.Pause();
    }

    public static void ToggleBGM()
    {
        bgmEnabled= !bgmEnabled;
        PlayerPrefs.SetInt("bgmEnabled", bgmEnabled?1:0);
        PlayerPrefs.Save();
        if (!bgmEnabled)
        {
            instance.StopBGM();
        }
        else
        {
            ResumeBGM();
        }
    }
    public static void ToggleSFX()
    {
        sfxEnabled = !sfxEnabled;
        PlayerPrefs.SetInt("sfxEnabled", sfxEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }
    [System.Serializable]
    public class SourceItem
    {
        public string clipName;
        public AudioClip audioClip;
    }

}

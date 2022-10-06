using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Audio
{
    public string name;
    
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    public bool isMucic;
    [HideInInspector]
    public AudioSource source;
}       

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        foreach (Audio s in audios) 
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        soundOn = true;
        soundOn = PlayerPrefs.GetInt("Sound") == 0;
        vibrationOn = PlayerPrefs.GetInt("Vibration") == 0;
    }

    public bool vibrationOn;
    public bool soundOn;
    public Audio[] audios;

    public void Play(string name)
    {
//        Debug.Log(name);
        Audio s = Array.Find(audios, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        if (!soundOn) return;
        s.source.Play();
    }
    
    
    public void OnOffSound()
    {
        soundOn = !soundOn;
        PlayerPrefs.SetInt("Sound", soundOn ? 0 : 1);
        if (!soundOn)
        {
            foreach (var s in audios)
            {
                if (!s.isMucic)
                {
                    s.source.Stop();
                }
            }
        }
    }

    public void OnOffVibration()
    {
        vibrationOn = !vibrationOn;
        PlayerPrefs.SetInt("Vibration", vibrationOn ? 0 : 1);
    }
}
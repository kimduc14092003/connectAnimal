using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public List<Sound> currentMusicSounds;
    public AudioSource musicSource,sfxSource;
    private int currentIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic(musicSounds[0].name);
    }

    public void GetRandomMusicToList()
    {
        currentIndex = 0;
        currentMusicSounds = new List<Sound>();
        List<int> listIndex= new List<int>();
        do
        {
            int randomNum = UnityEngine.Random.Range(1, musicSounds.Length);
            if (listIndex.Contains(randomNum)) continue;
            
            listIndex.Add(randomNum);
            currentMusicSounds.Add(musicSounds[randomNum]);

            if (currentMusicSounds.Count >= 3)
            {
                break;
            }
            
        } while (true);
    }

    public void PlayRandomMusic()
    {
        PlayMusic(currentMusicSounds[currentIndex].name);
        currentIndex++;
        if(currentIndex==currentMusicSounds.Count)
        {
            currentIndex = 0;
        }
    }

    public void PlayMusic(string name)
    {
        Sound s=Array.Find(musicSounds, x => x.name == name);
        if(s != null)
        {
            musicSource.clip=s.clip;
            musicSource.Play();
        }
        else
        {
            Debug.Log("music sound not found!");
        }
    }

    public bool IsPlayMusic()
    {
        return musicSource.isPlaying;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(string name)
    {
        Sound s= Array.Find(sfxSounds, x => x.name == name);
        if (s != null)
        {
            sfxSource.PlayOneShot(s.clip);
        }
        else
        {
            Debug.Log("SFX sound "+name+" not found!");
        }
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}

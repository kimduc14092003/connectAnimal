using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManagerEscapeMode : MonoBehaviour
{
    public ListCellEscapeController listCellEscapeController;
    public Slider musicSlider, sfxSlider;

    public Slider timeSlider;
    public float limitTimeOfLevel;
    public int currentLevel;
    public GameObject PausePanel,TimePanel,FunctionPanel;
    public GameObject winLevelPanel,loseGamePanel,winGamePanel;
    public GameObject currentMap;
    public GameObject[] listMap;
    public GameObject rabbit;
    public LineController lineController;
    public float timeRemaining;
    public int limitMoveTurn;
    public TMP_Text limitMoveTurnTxt, levelTitle;
    private bool isPauseGame;

    public LoadSceneManager loadSceneManager;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        GetInforOfGame();
        /*HandleLevelStart();
        HandlePlayeMode();*/

    }

    private void GetInforOfGame()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevelEscapeMode", 1);
        if (currentLevel < listMap.Length)
        {
            currentMap = Instantiate(listMap[currentLevel - 1], transform);
        }
        else
        {
            currentMap = Instantiate(listMap[listMap.Length - 1], transform);
        }
        ListCellEscapeController listCell = currentMap.GetComponent<ListCellEscapeController>();
        listCell.rabbit = rabbit;
        listCell.lineController = lineController;
        lineController.panel = currentMap;

        listCellEscapeController = listCell;

        currentMap.transform.SetSiblingIndex(3);
    }

    private void Start()
    {
        if (FunctionPanel)
        {
            FunctionPanel.SetActive(true);
            levelTitle.text = "Level " + currentLevel;
        }
        timeSlider.maxValue = limitTimeOfLevel;
        timeRemaining = limitTimeOfLevel;
        timeSlider.value = timeRemaining;
        AudioManager.Instance.PlayRandomMusic();
    }

    private void FixedUpdate()
    {
        TimeRemainingController();
    }

    public void MinusTurnMove()
    {
        limitMoveTurn--;
        limitMoveTurnTxt.text = limitMoveTurn + "";

    }
    private void TimeRemainingController()
    {
        if (isPauseGame)
        {
            /*TimePanel.SetActive(false);
            listCellEscapeController.gameObject.SetActive(false);
            if (FunctionPanel)
            {
                FunctionPanel.SetActive(false);
            }*/
            return;
        }
        else
        {
           /* TimePanel.SetActive(true);
            if (FunctionPanel)
            {
                FunctionPanel.SetActive(true);
            }
            listCellEscapeController.gameObject.SetActive(true);*/

        }

        if (timeRemaining >= 0 )
        {
            timeRemaining -= Time.deltaTime;
            timeSlider.value = timeRemaining;
        }
        else
        {
            LoseGameNotification();
        }
    }

    public void PauseGame()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PausePanel.SetActive(true);
        SetDefaultSlider();
        isPauseGame = true;
        rabbit.SetActive(false);
    }

    public void ReturnHomeScene()
    {
        AudioManager.Instance.PlaySFX("click_button");
        loadSceneManager.LoadScene("HomeScene");
    }

    public void PlayNewGame()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetInt("currentLevelEscapeMode", 1);
        loadSceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

   /* private void SetNewGameFuncionalRemaining()
    {
        PlayerPrefs.SetInt("totalScore"+currentDifficultLevel, 0);
        PlayerPrefs.SetInt("remainingHP", 10);
        PlayerPrefs.SetInt("remainingFind", 10);
        PlayerPrefs.SetInt("remainingShuffle", 10);
        switch (currentDifficultLevel)
        {
            case "Easy":
                {
                    PlayerPrefs.SetInt("currentLevelEasy", 1);
                    break;
                }
            case "Medium":
                {
                    PlayerPrefs.SetInt("currentLevelMedium", 1);
                    break;
                }
            case "Hard":
                {
                    PlayerPrefs.SetInt("currentLevelHard", 1);
                    break;
                }
        }
    }*/

    public void ResumeGame()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PausePanel.SetActive(false);
        isPauseGame = false;
        rabbit.SetActive(true);
    }

    public void WinLevelNotification()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySFX("win");
        isPauseGame = true;
        
 /*       TimePanel.SetActive(false);
        FunctionPanel.SetActive(false);
        currentMap.SetActive(false);
        rabbit.SetActive(false);*/
        winLevelPanel.SetActive(true);
        currentLevel++;
        PlayerPrefs.SetInt("currentLevelEscapeMode", currentLevel);
    }

    public void LoseGameNotification()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySFX("lose");
        PlayerPrefs.SetInt("currentLevelEscapeMode", 1);

        isPauseGame = true;
        loseGamePanel.SetActive(true);
        //rabbit.SetActive(false);
    }

    public void NextLevel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        loadSceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetDefaultSlider()
    {
        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }
    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(sfxSlider.value);
    }

}

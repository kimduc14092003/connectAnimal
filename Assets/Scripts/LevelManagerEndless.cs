using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManagerEndless : MonoBehaviour
{
    public EndlessModeController endlessModeController;
    public Slider musicSlider, sfxSlider;
    public TMP_Text 
         currentScoreTxt, highScoreTxt, loseTotalScoreTxt, loseHighScoreTxt;
    public Slider timeSlider;
    public float limitTimeOfLevel;
    public int currentLevel;
    public GameObject PausePanel,DetailPanel;
    public GameObject loseGamePanel;
    public GameObject[] listMatrix;
    public LineController lineController;

    public float timeRemaining;
    private string currentMatrix;
    private int currentScore;
    private bool isPauseGame;

    public List<GameObject> listGameObjectDeActive;
    private void Awake()
    {
        if(PlayerPrefs.GetString("PlayeMode")!= "EndlessMode")
        {
            GetComponent<LevelManagerEndless>().enabled = false;
        }
        Application.targetFrameRate = 60;
        float coefficient= Mathf.Pow(0.667f, currentLevel/9);
        limitTimeOfLevel = StaticData.limitTimeInEndless*coefficient ;
        DeActiveListGameObject();
    }
    private void DeActiveListGameObject()
    {
        for(int i = 0; i < listGameObjectDeActive.Count; i++)
        {
            listGameObjectDeActive[i].SetActive(false);
        }
    }
    private void Start()
    {
        currentScore = 0;
        highScoreTxt.transform.parent.gameObject.SetActive(true);
        highScoreTxt.text = PlayerPrefs.GetInt("highScoreTotalEndlessMode", 0)+"";
        isPauseGame = false;
        timeSlider.maxValue = limitTimeOfLevel;
        timeRemaining = limitTimeOfLevel;
        timeSlider.value = timeRemaining;
        SpawnMatrix();
        AudioManager.Instance.PlayRandomMusic();
    }

    private void SpawnMatrix()
    {
        int a = UnityEngine.Random.Range(0, listMatrix.Length);
        GameObject matrix= Instantiate(listMatrix[a],transform);
        matrix.GetComponent<EndlessModeController>().lineController = lineController;
        endlessModeController = matrix.GetComponent<EndlessModeController>();
        lineController.panel = matrix;
        currentMatrix = matrix.name;
        matrix.transform.SetSiblingIndex(2);
    }

    public void AddCurrentScore(int amount)
    {
        currentScore += amount;
        currentScoreTxt.text =  currentScore+"";
    }

    private void FixedUpdate()
    {
        TimeRemainingController();
    }

    private void TimeRemainingController()
    {
        if(isPauseGame)
        {
            DetailPanel.SetActive(false);
            endlessModeController.gameObject.SetActive(false);
            return;
        }
        else
        {
            DetailPanel.SetActive(true);
            endlessModeController.gameObject.SetActive(true);
        }

        if (timeRemaining >= 0 )
        {
            timeRemaining -= Time.deltaTime;
            timeSlider.value = timeRemaining;
        }
        else
        {
            timeRemaining = limitTimeOfLevel;
            MoveController();
        }
    }

    private void MoveController()
    {
        switch (currentMatrix)
        {
            case "Matrix1(Clone)":
                {
                    endlessModeController.MoveListCell(-3,1);
                    break;
                }
            case "Matrix2(Clone)":
                {
                    endlessModeController.MoveListCell(3,2);
                    break;
                }
            case "Matrix3(Clone)":
                {
                    endlessModeController.MoveListCell(3, 3);
                    break;
                }
            case "Matrix4(Clone)":
                {
                    endlessModeController.MoveListCell(3, 4);
                    break;
                }

        }
    }



    public void PauseGame()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PausePanel.SetActive(true);
        SetDefaultSlider();
        isPauseGame = true;
    }

    public void ReturnHomeScene()
    {
        AudioManager.Instance.PlaySFX("click_button");
        SceneManager.LoadScene("HomeScene");
    }

    public void PlayNewGame()
    {
        AudioManager.Instance.PlaySFX("click_button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void ResumeGame()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PausePanel.SetActive(false);
        isPauseGame = false;
    }

    public void LoseGameNotification()
    {
        AudioManager.Instance.PlaySFX("lose");
        AudioManager.Instance.StopMusic();

        isPauseGame = true;
        loseGamePanel.SetActive(true);

        int totalScore = currentScore;

        loseTotalScoreTxt.text = "Tổng điểm " + totalScore;

        int highScoreTotal = PlayerPrefs.GetInt("highScoreTotalEndlessMode" , totalScore);
        if (totalScore >= highScoreTotal)
        {
            PlayerPrefs.SetInt("highScoreTotalEndlessMode", highScoreTotal);
            loseHighScoreTxt.text = "Kỷ lục " + totalScore;
        }
        else
        {
            loseHighScoreTxt.text = "Kỷ lục " + highScoreTotal;
        }

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

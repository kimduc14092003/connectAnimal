using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShadowModeLevelManager : MonoBehaviour
{
    public ShadowModeController[] listMatrixShadowModeController;
    public ShadowModeController[] listMatrixButterflyController;
    public ShadowModeController shadowModeController;

    public Slider musicSlider, sfxSlider;
    public TMP_Text numberShuffle, numberFind, numberHP,
        levelTitle, currentScoreTxt, levelScoreTxt, totalScoreTxt, highScoreTxt,
        victoryTotalScoreTxt, victoryHighScoreTxt, loseTotalScoreTxt, loseHighScoreTxt;
    public Image bg;
    public Sprite[] listSpriteBg;
    public int currentLevel;
    public GameObject PausePanel,TimePanel,FunctionPanel;
    public GameObject winLevelPanel,loseGamePanel,winGamePanel;
    public GameObject notifyMinusHearth;
    public GameObject TutorialPanel;
    public bool isShuffle;
    public bool isFinded;
    public LineController lineController;
    public Button btnFind, btnShuffle;
    public float limitTimeOfLevel;
    public Slider timeSlider;
    public float timeRemaining;

    private string currentDifficultLevel;
    private int remainingHP;
    private int remainingFind;
    private int remainingShuffle;
    private int currentScore;
    private bool isPauseGame;
    private string currentMode;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        currentMode = PlayerPrefs.GetString("PlayeMode");
        if (currentMode == "ShadowMode")
        {
            currentLevel = PlayerPrefs.GetInt("currentLevelShadowMode", 1);
            shadowModeController = listMatrixShadowModeController[currentLevel-1];
        }
        else if(currentMode =="ButterflyMode")
        {
            currentLevel= PlayerPrefs.GetInt("currentLevelButterflyMode", 1);
            shadowModeController = listMatrixButterflyController[currentLevel-1];
        }
        GameObject panel= Instantiate(shadowModeController.gameObject,transform);

        shadowModeController = panel.GetComponent<ShadowModeController>();
        shadowModeController.levelManager = gameObject.GetComponent<ShadowModeLevelManager>();
        shadowModeController.lineController = lineController;
        panel.transform.SetSiblingIndex(2);
        lineController.panel= panel;
    }

    private void Start()
    {
        if (FunctionPanel)
        {
            FunctionPanel.SetActive(true);
            levelTitle.text = "Màn " + currentLevel;
        }

        currentScore = 0;
        isPauseGame = false;
        isFinded = false;
        isShuffle = false;

        if (currentMode == "ShadowMode")
        {
            remainingHP = PlayerPrefs.GetInt("remainingHPShadowMode",10);
            remainingFind = PlayerPrefs.GetInt("remainingFindShadowMode",10);
            remainingShuffle = PlayerPrefs.GetInt("remainingShuffleShadowMode", 10);
        }
        else if(currentMode =="ButterflyMode")
        {
            remainingHP = PlayerPrefs.GetInt("remainingHPButterflyMode", 10);
            remainingFind = PlayerPrefs.GetInt("remainingFindButterflyMode", 10);
            remainingShuffle = PlayerPrefs.GetInt("remainingShuffleButterflyMode", 10);
        }
        bg.sprite = listSpriteBg[PlayerPrefs.GetInt("bgSpriteIndex",0)];
        if (numberShuffle)
        {
            SetRemainingOfFuncional();
        }
        btnFind.onClick.AddListener(FindFunctional);
        btnShuffle.onClick.AddListener(ShuffleListCellFunctional);

        timeSlider.maxValue = limitTimeOfLevel;
        timeRemaining = limitTimeOfLevel;
        timeSlider.value = timeRemaining;
        AudioManager.Instance.PlayRandomMusic();
    }

    public void AddCurrentScore(int amount)
    {
        currentScore += amount;
        currentScoreTxt.text = "Điểm: "+ currentScore;
    }

    private void SetRemainingOfFuncional()
    {
        if (currentMode == "ShadowMode")
        {
            remainingHP = PlayerPrefs.GetInt("remainingHPShadowMode",10);
            remainingFind = PlayerPrefs.GetInt("remainingFindShadowMode",10);
            remainingShuffle = PlayerPrefs.GetInt("remainingShuffleShadowMode", 10);
        }
        else if (currentMode == "ButterflyMode")
        {
            remainingHP = PlayerPrefs.GetInt("remainingHPButterflyMode", 10);
            remainingFind = PlayerPrefs.GetInt("remainingFindButterflyMode", 10);
            remainingShuffle = PlayerPrefs.GetInt("remainingShuffleButterflyMode", 10);
        }
        numberShuffle.text = remainingShuffle + "";
        numberFind.text = remainingFind + "";
        numberHP.text = remainingHP + "";
    }

    private void FixedUpdate()
    {
        if (numberShuffle)
        {
            SetRemainingOfFuncional();
        }
        TimeRemainingController();
    }

    private void TimeRemainingController()
    {
        if (isPauseGame)
        {
            TimePanel.SetActive(false);
            shadowModeController.gameObject.SetActive(false);
            if (FunctionPanel)
            {
                FunctionPanel.SetActive(false);
            }
            return;
        }
        else
        {
            TimePanel.SetActive(true);
            if (FunctionPanel)
            {
                FunctionPanel.SetActive(true);
            }
            shadowModeController.gameObject.SetActive(true);

        }
        if (timeRemaining >= 0)
        {
            timeRemaining -= Time.deltaTime;
            timeSlider.value = timeRemaining;
        }
        else
        {
            LoseGameNotification();
        }
    }

    public void HandleMinusHP()
    {
        remainingHP--;

        if (currentMode == "ShadowMode")
        {
            PlayerPrefs.SetInt("remainingHPShadowMode", remainingHP);
        }else if(currentMode == "ButterflyMode")
        {
            PlayerPrefs.SetInt("remainingHPButterflyMode", remainingHP);

        }
        numberHP.text = remainingHP + "";
        notifyMinusHearth.SetActive(true);
        Invoke("TurnOffMinusHearth", 1.15f);
        if (remainingHP <= 0) 
        {
            LoseGameNotification();
        }
    }

    private void TurnOffMinusHearth()
    {
        notifyMinusHearth.SetActive(false);

    }

    public void ShuffleListCellFunctional()
    {
        if (remainingShuffle > 0&&!isShuffle)
        {
            isShuffle = true;
            isFinded = false;
            AudioManager.Instance.PlaySFX("shuffle");
            shadowModeController.ShuffleAllCell(0.45f);

            if (currentMode == "ShadowMode")
            {
                PlayerPrefs.SetInt("remainingShuffleShadowMode", --remainingShuffle);
            }
            else if(currentMode == "ButterflyMode")
            {
                PlayerPrefs.SetInt("remainingShuffleButterflyMode", --remainingShuffle);
            }
        }
    }
    public void FindFunctional()
    {
        if(remainingFind > 0&&!isShuffle)
        {
            AudioManager.Instance.PlaySFX("hint");

            shadowModeController.OnPointerClickFindFunctional();
            if (!isFinded)
            {
                isFinded = true;

                if (currentMode == "ShadowMode")
                {
                    PlayerPrefs.SetInt("remainingFindShadowMode", --remainingFind);
                }else
                    if(currentMode == "ButterflyMode")
                {
                    PlayerPrefs.SetInt("remainingFindButterflyMode", --remainingFind);
                }
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
        SetNewGameFuncionalRemaining();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetNewGameFuncionalRemaining()
    {
        if (currentMode == "ShadowMode")
        {
            PlayerPrefs.SetInt("currentLevelShadowMode", 1);
            PlayerPrefs.SetInt("totalScoreShadowMode", 0);
            PlayerPrefs.SetInt("remainingHPShadowMode", 10);
            PlayerPrefs.SetInt("remainingFindShadowMode", 10);
            PlayerPrefs.SetInt("remainingShuffleShadowMode", 10);
        }
        else if(currentMode == "ButterflyMode")
        {
            PlayerPrefs.SetInt("currentLevelButterflyMode", 1);
            PlayerPrefs.SetInt("totalScoreButterflyMode", 0);
            PlayerPrefs.SetInt("remainingHPButterflyMode", 10);
            PlayerPrefs.SetInt("remainingFindButterflyMode", 10);
            PlayerPrefs.SetInt("remainingShuffleButterflyMode", 10);
        }
    }

    public void ResumeGame()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PausePanel.SetActive(false);
        isPauseGame = false;
    }

    public void WinLevelNotification()
    {
        AudioManager.Instance.PlaySFX("win");
        AudioManager.Instance.StopMusic();
        currentLevel++;

        isPauseGame = true;
        int levelScore = currentScore ;
        levelScoreTxt.text ="Điểm "+ levelScore;

        int highScore = PlayerPrefs.GetInt("highScoreLevelShadowMode" + currentLevel + currentDifficultLevel, levelScore);
        //Set giá trị cho high score nếu điểm của người chơi > điểm high score trước
        if (currentMode == "ButterflyMode")
        {
            highScore = PlayerPrefs.GetInt("highScoreLevelButterflyMode" + currentLevel + currentDifficultLevel, levelScore);
        }
        if (levelScore >= highScore){
            if (currentMode == "ShadowMode")
            {
                PlayerPrefs.SetInt("highScoreLevelShadowMode" + currentLevel + currentDifficultLevel, levelScore);
            }
            else if (currentMode == "ButterflyMode")
            {
                PlayerPrefs.SetInt("highScoreLevelButterflyMode" + currentLevel + currentDifficultLevel, levelScore);

            }
            highScoreTxt.text = "Kỷ lục " + levelScore;
        }
        else
        {
            highScoreTxt.text = "Kỷ lục " + highScore;
        }

        //Set tổng điểm qua các level của player
        int totalScore=0;
        if(currentMode == "ShadowMode")
        {
            totalScore = PlayerPrefs.GetInt("totalScoreShadowMode", 0) + levelScore;
            PlayerPrefs.SetInt("totalScoreShadowMode", totalScore);

        }
        else
        if(currentMode == "ButterflyMode")
        {
            totalScore= PlayerPrefs.GetInt("totalScoreButterflyMode", 0) + levelScore;
            PlayerPrefs.SetInt("totalScoreButterflyMode", totalScore);

        }


        totalScoreTxt.text = "Tổng điểm "+ totalScore;

        if (currentLevel > listMatrixShadowModeController.Length)
        {
            winGamePanel.SetActive(true);

            victoryTotalScoreTxt.text = "Tổng điểm " + totalScore;

            int highScoreTotal=0;

            if (currentMode == "ShadowMode")
            {
                highScoreTotal = PlayerPrefs.GetInt("totalScoreShadowMode", totalScore);
            }
            else if (currentMode == "ButterflyMode")
            {
                highScoreTotal = PlayerPrefs.GetInt("totalScoreButterflyMode", totalScore);

            }

            if (totalScore >= highScoreTotal)
            {

                if (currentMode == "ShadowMode")
                {
                    PlayerPrefs.SetInt("highScoreShadowMode", highScoreTotal);

                }
                else if (currentMode == "ButterflyMode")
                {
                    PlayerPrefs.SetInt("highScoreButterflyMode", highScoreTotal);

                }
                victoryHighScoreTxt.text = "Kỷ lục " + totalScore;
            }
            else
            {
                victoryHighScoreTxt.text = "Kỷ lục " + highScoreTotal;
            }

            SetNewGameFuncionalRemaining();
            return;
        }
        //Người chơi chiến thắng game khi chơi thắng level 9
        winLevelPanel.SetActive(true);


        if (currentMode == "ShadowMode")
        {
            PlayerPrefs.SetInt("remainingHPShadowMode", ++remainingHP);
            PlayerPrefs.SetInt("remainingFindShadowMode", ++remainingFind);
            PlayerPrefs.SetInt("remainingShuffleShadowMode", ++remainingShuffle);

            PlayerPrefs.SetInt("currentLevelShadowMode", currentLevel);
        }
        else if (currentMode == "ButterflyMode")
        {
            PlayerPrefs.SetInt("remainingHPButterflyMode", ++remainingHP);
            PlayerPrefs.SetInt("remainingFindButterflyMode", ++remainingFind);
            PlayerPrefs.SetInt("remainingShuffleButterflyMode", ++remainingShuffle);

            PlayerPrefs.SetInt("currentLevelButterflyMode", currentLevel);
        }
        
    }

    public void LoseGameNotification()
    {
        AudioManager.Instance.PlaySFX("lose");
        AudioManager.Instance.StopMusic();

        isPauseGame = true;
        loseGamePanel.SetActive(true);

        // Số điểm khi thua = tổng số điểm đã tích trước đó + số điểm hiện tại

        int totalScore = 0;
        int highScoreTotal = 0;
        if (currentMode == "ShadowMode")
        {
            totalScore = PlayerPrefs.GetInt("totalScoreShadowMode", 0) + currentScore;
            highScoreTotal = PlayerPrefs.GetInt("highScoreShadowMode", totalScore);
        }
        else if (currentMode == "ButterflyMode")
        {
            totalScore = PlayerPrefs.GetInt("totalScoreButterflyMode", 0) + currentScore;
            highScoreTotal = PlayerPrefs.GetInt("highScoreButterflyMode", totalScore);
        }

        loseTotalScoreTxt.text = "Tổng điểm " + totalScore;

        if (totalScore >= highScoreTotal)
        {

            if (currentMode == "ShadowMode")
            {
                PlayerPrefs.SetInt("highScoreShadowMode" , highScoreTotal);

            }
            else if (currentMode == "ButterflyMode")
            {
                PlayerPrefs.SetInt("highScoreButterflyMode" , highScoreTotal);

            }
            loseHighScoreTxt.text = "Kỷ lục " + totalScore;
        }
        else
        {
            loseHighScoreTxt.text = "Kỷ lục " + highScoreTotal;
        }

        SetNewGameFuncionalRemaining();
    }

    public void NextLevel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

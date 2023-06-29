using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public ListCellController listCellController;
    public Slider musicSlider, sfxSlider;
    public TMP_Text numberShuffle, numberFind, numberHP,
        levelTitle, currentScoreTxt, levelScoreTxt, totalScoreTxt, highScoreTxt,
        victoryTotalScoreTxt, victoryHighScoreTxt, loseTotalScoreTxt, loseHighScoreTxt;

    public Slider timeSlider;
    public float limitTimeOfLevel;
    public int currentLevel;
    public GameObject PausePanel,DetailPanel;
    public GameObject winLevelPanel,loseGamePanel,winGamePanel;
    public GameObject notifyMinusHearth;
    public GameObject TutorialPanel;
    public bool isShuffle;
    public bool isFinded;

    public float timeRemaining;
    private string currentDifficultLevel;
    private int remainingHP;
    private int remainingFind;
    private int remainingShuffle;
    private int currentScore;
    private bool isPauseGame;

    public LoadSceneManager loadSceneManager;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        string playerMode = PlayerPrefs.GetString("PlayeMode");
        if (playerMode != "ClassicMode" && playerMode!= "RelaxRandomMode" && playerMode != "ChallengeMode")
        {
            GetComponent<LevelManager>().enabled = false;
        }
        currentDifficultLevel = PlayerPrefs.GetString("currentDifficultLevel");
    }

    private void HandlePlayeMode()
    {
        float coefficient= Mathf.Pow(0.667f, currentLevel/9);
        switch (PlayerPrefs.GetString("PlayeMode"))
        {
            case "ChallengeMode":
                {
                    limitTimeOfLevel = StaticData.limitTimeInLevel * coefficient * 0.5f;
                    break;
                }
            case "RelaxRandomMode":
                {
                    limitTimeOfLevel = 10;
                    timeSlider.gameObject.SetActive(false);

                    listCellController.colPlay = 18;
                    listCellController.rowPlay = 8;
                    listCellController.col = 20;
                    listCellController.row = 10;
                    listCellController.numOfDifferentCell = 26;
                    listCellController.limitCellAdjacent = 8;
                    GridLayoutGroup gridLayoutGroup = listCellController.gameObject.GetComponent<GridLayoutGroup>();
                    gridLayoutGroup.cellSize = new Vector2(85, 100);
                    currentLevel = PlayerPrefs.GetInt("currentLevelRandomMode", 1);
                    break;
                }
            default:
                {
                    limitTimeOfLevel = StaticData.limitTimeInLevel * coefficient;
                    break;
                }
        }


    }

    private void Start()
    {
        HandleLevelStart();
        HandlePlayeMode();
        if (DetailPanel)
        {
            DetailPanel.SetActive(true);
            levelTitle.text = "Level " + currentLevel;
        }

        currentScore = 0;
        isPauseGame = false;
        isFinded = false;
        isShuffle = false;
        timeSlider.maxValue = limitTimeOfLevel;
        timeRemaining = limitTimeOfLevel;
        timeSlider.value = timeRemaining;
        remainingHP = PlayerPrefs.GetInt("remainingHP");
        remainingFind = PlayerPrefs.GetInt("remainingFind");
        remainingShuffle = PlayerPrefs.GetInt("remainingShuffle");
        if (numberShuffle)
        {
            SetRemainingOfFuncional();
        }
        try
        {
            AudioManager.Instance.PlayRandomMusic();
        }
        catch
        {
            Debug.Log("Audio Manager is null!");
        }
    }

    public void AddCurrentScore(int amount)
    {
        currentScore += amount;
        currentScoreTxt.text = currentScore+"";
    }

    private void SetRemainingOfFuncional()
    {
        remainingHP = PlayerPrefs.GetInt("remainingHP");
        remainingFind = PlayerPrefs.GetInt("remainingFind");
        remainingShuffle = PlayerPrefs.GetInt("remainingShuffle");
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
        if(isPauseGame)
        {

            return;
        }
        else
        {

        }

        if (timeRemaining >= 0 )
        {
            if(PlayerPrefs.GetString("PlayeMode") != "RelaxRandomMode")
            {
                timeRemaining -= Time.deltaTime;
                timeSlider.value = timeRemaining;
            }
        }
        else
        {
            LoseGameNotification();
        }
    }

    private void HandleLevelStart()
    {

        switch (currentDifficultLevel)
        {
            case "Easy":
                {
                    listCellController.colPlay = 8;
                    listCellController.rowPlay = 6;
                    listCellController.numOfDifferentCell = 18;
                    listCellController.limitCellAdjacent = 4;
                    GridLayoutGroup gridLayoutGroup= listCellController.gameObject.GetComponent<GridLayoutGroup>();
                    gridLayoutGroup.cellSize = new Vector2(110 , 137.5f);
                    currentLevel = PlayerPrefs.GetInt("currentLevelEasy");
                    break;
                }
            case "Medium":
                {
                    listCellController.colPlay = 14;
                    listCellController.rowPlay = 7;
                    listCellController.numOfDifferentCell = 22;
                    listCellController.limitCellAdjacent = 6;
                    GridLayoutGroup gridLayoutGroup = listCellController.gameObject.GetComponent<GridLayoutGroup>();
                    gridLayoutGroup.cellSize = new Vector2(100, 125);
                    currentLevel = PlayerPrefs.GetInt("currentLevelMedium");
                    break;
                }
            case "Hard":
                {
                    listCellController.colPlay = 18;
                    listCellController.rowPlay = 8; 
                    listCellController.numOfDifferentCell = 26;
                    listCellController.limitCellAdjacent = 8;
                    GridLayoutGroup gridLayoutGroup = listCellController.gameObject.GetComponent<GridLayoutGroup>();
                    gridLayoutGroup.cellSize = new Vector2(85, 100);
                    currentLevel = PlayerPrefs.GetInt("currentLevelHard");
                    break;
                }
        }
    }

    public void HandleMinusHP()
    {
        remainingHP--;
        PlayerPrefs.SetInt("remainingHP", remainingHP);
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
            remainingShuffle--;
            listCellController.ShuffleAllCell(0.45f);
            PlayerPrefs.SetInt("remainingShuffle",remainingShuffle);
        }
    }
    public void FindFunctional()
    {
        if(remainingFind > 0&&!isShuffle)
        {
            AudioManager.Instance.PlaySFX("hint");
            listCellController.OnPointerClickFindFunctional();
            if (!isFinded)
            {
                isFinded = true;
                remainingFind--;
                PlayerPrefs.SetInt("remainingFind", remainingFind);
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
        loadSceneManager.LoadScene("HomeScene");
    }

    public void PlayNewGame()
    {
        AudioManager.Instance.PlaySFX("click_button");
        SetNewGameFuncionalRemaining();
        loadSceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetNewGameFuncionalRemaining()
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
        isPauseGame = true;
        int levelScore = currentScore + (int)timeRemaining;
        levelScoreTxt.text ="Score : "+ levelScore;
        
        //Set giá trị cho high score nếu điểm của người chơi > điểm high score trước
        int highScore = PlayerPrefs.GetInt("highScoreLevel" + currentLevel + currentDifficultLevel, levelScore);
        if (levelScore >= highScore){
            PlayerPrefs.SetInt("highScoreLevel" + currentLevel + currentDifficultLevel, levelScore);
            highScoreTxt.text = "Record : " + levelScore;
        }
        else
        {
            highScoreTxt.text = "Record " + highScore;
        }

        //Set tổng điểm qua các level của player
        int totalScore = PlayerPrefs.GetInt("totalScore" + currentDifficultLevel,0)+ levelScore;
        PlayerPrefs.SetInt("totalScore"+ currentDifficultLevel,totalScore);

        totalScoreTxt.text = "Total score : "+ totalScore;

        //Người chơi chiến thắng game khi chơi thắng level 9
     /*   if (currentLevel >= 9)
        {
            winGamePanel.SetActive(true);

            victoryTotalScoreTxt.text = "Tổng điểm " + totalScore;

            int highScoreTotal = PlayerPrefs.GetInt("highScoreTotal" + currentDifficultLevel, totalScore);
            if (totalScore >= highScoreTotal)
            {
                PlayerPrefs.SetInt("highScoreTotal" + currentDifficultLevel, highScoreTotal);
                victoryHighScoreTxt.text = "Kỷ lục " + totalScore;
            }
            else
            {
                victoryHighScoreTxt.text = "Kỷ lục " + highScoreTotal;
            }

            SetNewGameFuncionalRemaining();
            return;
        }*/
        winLevelPanel.SetActive(true);
        currentLevel++;
        PlayerPrefs.SetInt("remainingHP", ++remainingHP);
        PlayerPrefs.SetInt("remainingFind", ++remainingFind);
        PlayerPrefs.SetInt("remainingShuffle",++remainingShuffle);

        switch (currentDifficultLevel)
        {
            case "Easy":
                {
                    PlayerPrefs.SetInt("currentLevelEasy",currentLevel);
                    break;
                }
            case "Medium":
                {
                    PlayerPrefs.SetInt("currentLevelMedium", currentLevel);
                    break;
                }
            case "Hard":
                {
                    PlayerPrefs.SetInt("currentLevelHard", currentLevel);
                    break;
                }
        }
    }

    public void LoseGameNotification()
    {
        AudioManager.Instance.PlaySFX("lose");
        AudioManager.Instance.StopMusic();

        isPauseGame = true;
        loseGamePanel.SetActive(true);

        // Số điểm khi thua = tổng số điểm đã tích trước đó + số điểm hiện tại
        int totalScore = PlayerPrefs.GetInt("totalScore" + currentDifficultLevel, 0) + currentScore;

        loseTotalScoreTxt.text = "Total score " + totalScore;

        int highScoreTotal = PlayerPrefs.GetInt("highScoreTotal" + currentDifficultLevel, totalScore);
        if (totalScore >= highScoreTotal)
        {
            PlayerPrefs.SetInt("highScoreTotal" + currentDifficultLevel, highScoreTotal);
            loseHighScoreTxt.text = "Record : " + totalScore;
        }
        else
        {
            loseHighScoreTxt.text = "Record : " + highScoreTotal;
        }

        SetNewGameFuncionalRemaining();
    }

    public void NextLevel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        loadSceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetDefaultSlider()
    {
        musicSlider.value = 
            AudioManager.Instance.musicSource.volume;
        sfxSlider.value = 
            AudioManager.Instance.sfxSource.volume;
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

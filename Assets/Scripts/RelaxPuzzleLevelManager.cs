using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RelaxPuzzleLevelManager : MonoBehaviour
{
    public PuzzleModeController[] listMatrixPuzzleModeController;
    public PuzzleModeController puzzleModeController;
    public Slider musicSlider, sfxSlider;
    public TMP_Text numberShuffle, numberFind, numberHP,
        levelTitle, currentScoreTxt, levelScoreTxt, totalScoreTxt, highScoreTxt,
        victoryTotalScoreTxt, victoryHighScoreTxt, loseTotalScoreTxt, loseHighScoreTxt;
    public int currentLevel;
    public GameObject PausePanel,DetailPanel;
    public GameObject winLevelPanel,loseGamePanel,winGamePanel;
    public GameObject notifyMinusHearth;
    public bool isShuffle;
    public bool isFinded;
    public LineController lineController;
    public Button btnFind, btnShuffle;

    private string currentDifficultLevel;
    private int remainingHP;
    private int remainingFind;
    private int remainingShuffle;
    private int currentScore;
    private bool isPauseGame;
    public List<GameObject> listGameObjectDeActive;
    public LoadSceneManager loadSceneManager;

    private void Awake()
    {
        if (PlayerPrefs.GetString("PlayeMode") != "RelaxPuzzleMode")
        {
            GetComponent<RelaxPuzzleLevelManager>().enabled = false;
            return;
        }
        //Application.targetFrameRate = 60;
        currentLevel = PlayerPrefs.GetInt("currentLevelRelaxPuzzleMode", 1);
       puzzleModeController = listMatrixPuzzleModeController[currentLevel-1];
        GameObject panel= Instantiate(puzzleModeController.gameObject,transform);

        puzzleModeController = panel.GetComponent<PuzzleModeController>();
        puzzleModeController.levelManager = gameObject.GetComponent<RelaxPuzzleLevelManager>();
        puzzleModeController.lineController = lineController;
        panel.transform.SetSiblingIndex(2);
        lineController.panel= panel;
    }

    private void DeActiveListGameObject()
    {
        for (int i = 0; i < listGameObjectDeActive.Count; i++)
        {
            listGameObjectDeActive[i].SetActive(false);
        }
    }

    private void Start()
    {
        DeActiveListGameObject();
        if (DetailPanel)
        {
            DetailPanel.SetActive(true);
            levelTitle.text = "Level " + currentLevel;
        }

        currentScore = 0;
        isPauseGame = false;
        isFinded = false;
        isShuffle = false;
        remainingHP = PlayerPrefs.GetInt("remainingHPRelaxPuzzleMode",10);
        remainingFind = PlayerPrefs.GetInt("remainingFindRelaxPuzzleMode",10);
        remainingShuffle = PlayerPrefs.GetInt("remainingShuffleRelaxPuzzleMode", 10);
        if (numberShuffle)
        {
            SetRemainingOfFuncional();
        }
        btnFind.onClick.AddListener(FindFunctional);
        btnShuffle.onClick.AddListener(ShuffleListCellFunctional);
        AudioManager.Instance.PlayRandomMusic();
    }

    public void AddCurrentScore(int amount)
    {
        currentScore += amount;
        currentScoreTxt.text = ""+ currentScore;
    }

    private void SetRemainingOfFuncional()
    {
        remainingHP = PlayerPrefs.GetInt("remainingHPRelaxPuzzleMode",10);
        remainingFind = PlayerPrefs.GetInt("remainingFindRelaxPuzzleMode",10);
        remainingShuffle = PlayerPrefs.GetInt("remainingShuffleRelaxPuzzleMode", 10);
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
           /* DetailPanel.SetActive(false);
            puzzleModeController.gameObject.SetActive(false);*/
            return;
        }
        else
        {
          /*  DetailPanel.SetActive(true);
            puzzleModeController.gameObject.SetActive(true);
*/
        }
    }

    public void HandleMinusHP()
    {
        remainingHP--;
        PlayerPrefs.SetInt("remainingHPRelaxPuzzleMode", remainingHP);
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
            puzzleModeController.ShuffleAllCell(0.45f);
            PlayerPrefs.SetInt("remainingShuffleRelaxPuzzleMode", remainingShuffle);
        }
    }
    public void FindFunctional()
    {
        if(remainingFind > 0&&!isShuffle)
        {
            AudioManager.Instance.PlaySFX("hint");

            puzzleModeController.OnPointerClickFindFunctional();
            if (!isFinded)
            {
                isFinded = true;
                remainingFind--;
                PlayerPrefs.SetInt("remainingFindRelaxPuzzleMode", remainingFind);
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
        PlayerPrefs.SetInt("currentLevelRelaxPuzzleMode", 1);
        PlayerPrefs.SetInt("totalScoreRelaxPuzzleMode", 0);
        PlayerPrefs.SetInt("remainingHPRelaxPuzzleMode", 10);
        PlayerPrefs.SetInt("remainingFindRelaxPuzzleMode", 10);
        PlayerPrefs.SetInt("remainingShuffleRelaxPuzzleMode", 10);
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
        puzzleModeController.gameObject.SetActive(false);
        isPauseGame = true;
        int levelScore = currentScore ;
        levelScoreTxt.text ="Score : "+ levelScore;
        
        //Set giá trị cho high score nếu điểm của người chơi > điểm high score trước
        int highScore = PlayerPrefs.GetInt("highScoreLevelRelaxPuzzleMode" + currentLevel + currentDifficultLevel, levelScore);
        if (levelScore >= highScore){
            PlayerPrefs.SetInt("highScoreLevelRelaxPuzzleMode" + currentLevel + currentDifficultLevel, levelScore);
            highScoreTxt.text = "Record : " + levelScore;
        }
        else
        {
            highScoreTxt.text = "Record : " + highScore;
        }

        //Set tổng điểm qua các level của player
        int totalScore = PlayerPrefs.GetInt("totalScoreRelaxPuzzleMode",0)+ levelScore;
        PlayerPrefs.SetInt("totalScoreRelaxPuzzleMode",totalScore);

        totalScoreTxt.text = "Total score : "+ totalScore;

        currentLevel++;

        if (currentLevel > listMatrixPuzzleModeController.Length)
        {
            winGamePanel.SetActive(true);

            victoryTotalScoreTxt.text = "Total score : " + totalScore;

            int highScoreTotal = PlayerPrefs.GetInt("totalScoreRelaxPuzzleMode", totalScore);
            if (totalScore >= highScoreTotal)
            {
                PlayerPrefs.SetInt("totalScoreRelaxPuzzleMode", highScoreTotal);
                victoryHighScoreTxt.text = "Record : " + totalScore;
            }
            else
            {
                victoryHighScoreTxt.text = "Record : " + highScoreTotal;
            }

            SetNewGameFuncionalRemaining();
            return;
        }
        //Người chơi chiến thắng game khi chơi thắng level 9
        winLevelPanel.SetActive(true);
        PlayerPrefs.SetInt("remainingHPRelaxPuzzleMode", ++remainingHP);
        PlayerPrefs.SetInt("remainingFindRelaxPuzzleMode", ++remainingFind);
        PlayerPrefs.SetInt("remainingShuffleRelaxPuzzleMode", ++remainingShuffle);

        PlayerPrefs.SetInt("currentLevelRelaxPuzzleMode", currentLevel);
        
    }

    public void LoseGameNotification()
    {
        AudioManager.Instance.PlaySFX("lose");
        AudioManager.Instance.StopMusic();

        isPauseGame = true;
        loseGamePanel.SetActive(true);

        // Số điểm khi thua = tổng số điểm đã tích trước đó + số điểm hiện tại
        int totalScore = PlayerPrefs.GetInt("totalScoreRelaxPuzzleMode", 0) + currentScore;

        loseTotalScoreTxt.text = "Total score : " + totalScore;

        int highScoreTotal = PlayerPrefs.GetInt("highScoreRelaxPuzzleMode" + currentDifficultLevel, totalScore);
        if (totalScore >= highScoreTotal)
        {
            PlayerPrefs.SetInt("highScoreRelaxPuzzleMode" + currentDifficultLevel, highScoreTotal);
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

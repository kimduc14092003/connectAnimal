using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevModeScript : MonoBehaviour
{
    public GameObject devModePanel;
    public GameObject btnOnUnlimitedTime, btnOffUnlimitedTime, btnOnUnlimitedAssist, btnOffUnlimitedAssist;

    private bool isUnlimitedTime,isUnlimitedAssist;

    public static DevModeScript instance;

    private void Awake()
    {
        if(instance != null&& instance!=this)
        {
            Destroy(this.gameObject );
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if(isUnlimitedTime)
        {
            LevelManager levelManager = GameObject.Find("Canvas").GetComponent<LevelManager>();
            ShadowModeLevelManager shadowModeLevelManager = GameObject.Find("Canvas").GetComponent<ShadowModeLevelManager>();
            LevelManagerThreeMatch levelManagerThreeMatch = GameObject.Find("Canvas").GetComponent<LevelManagerThreeMatch>();
            LevelManagerEscapeMode levelManagerEscapeMode = GameObject.Find("Canvas").GetComponent<LevelManagerEscapeMode>();

            if (levelManager != null)
            {
                levelManager.timeRemaining= levelManager.limitTimeOfLevel;
            }else if(shadowModeLevelManager!= null)
            {
                shadowModeLevelManager.timeRemaining= shadowModeLevelManager.limitTimeOfLevel;
            }
            else if (levelManagerThreeMatch != null)
            {
                levelManagerThreeMatch.timeRemaining=levelManagerThreeMatch.limitTimeOfLevel;
            }
            else if(levelManagerEscapeMode!= null)
            {
                levelManagerEscapeMode.timeRemaining = levelManagerEscapeMode.limitTimeOfLevel;
            }
        }
        if (isUnlimitedAssist)
        {
            LevelManager levelManager = GameObject.Find("Canvas").GetComponent<LevelManager>();
            RelaxPuzzleLevelManager relaxPuzzleLevelManager = GameObject.Find("Canvas").GetComponent<RelaxPuzzleLevelManager>();
            ShadowModeLevelManager shadowModeLevelManager = GameObject.Find("Canvas").GetComponent<ShadowModeLevelManager>();
            LevelManagerThreeMatch levelManagerThreeMatch = GameObject.Find("Canvas").GetComponent< LevelManagerThreeMatch>();

            if (levelManager != null)
            {
                 PlayerPrefs.SetInt("remainingHP",10);
                 PlayerPrefs.SetInt("remainingFind",10);
                 PlayerPrefs.SetInt("remainingShuffle",10);
            }else if(relaxPuzzleLevelManager != null)
            {
                PlayerPrefs.SetInt("remainingHPRelaxPuzzleMode", 10);
                PlayerPrefs.SetInt("remainingFindRelaxPuzzleMode", 10);
                PlayerPrefs.SetInt("remainingShuffleRelaxPuzzleMode", 10);
            }else if(shadowModeLevelManager != null)
            {
                PlayerPrefs.SetInt("remainingHPShadowMode", 10);
                PlayerPrefs.SetInt("remainingFindShadowMode", 10);
                PlayerPrefs.SetInt("remainingShuffleShadowMode", 10);
            }
            else if(levelManagerThreeMatch != null) 
            {
                PlayerPrefs.SetInt("bombRemainingThreeMatchMode", 5);
                PlayerPrefs.SetInt("findRemainingThreeMatchMode", 5);
                PlayerPrefs.SetInt("shuffleRemainingThreeMatchMode", 5);
            }
        }
    }

    public void ToggleDevModePanel()
    {
        if (devModePanel.active)
        {
            devModePanel.SetActive(false);
        }
        else
        {
            devModePanel.SetActive(true);
        }
    }

    public void ToggleIsUnlimitedTime()
    {
        if (isUnlimitedTime)
        {
            isUnlimitedTime = !isUnlimitedTime;
            btnOnUnlimitedTime.SetActive(false);
            btnOffUnlimitedTime.SetActive(true);
        }
        else
        {
            isUnlimitedTime = !isUnlimitedTime;
            btnOnUnlimitedTime.SetActive(true);
            btnOffUnlimitedTime.SetActive(false);
        }
    }

    public void ToggleIsUnlimitedAssist()
    {
        if (isUnlimitedAssist)
        {
            isUnlimitedAssist = !isUnlimitedAssist;
            btnOnUnlimitedAssist.SetActive(false);
            btnOffUnlimitedAssist.SetActive(true);
        }
        else
        {
            isUnlimitedAssist = !isUnlimitedAssist;
            btnOnUnlimitedAssist.SetActive(true);
            btnOffUnlimitedAssist.SetActive(false);
        }
    }

    public void NextLevel()
    {
        LevelManager levelManager = GameObject.Find("Canvas").GetComponent<LevelManager>();
        RelaxPuzzleLevelManager relaxPuzzleLevelManager = GameObject.Find("Canvas").GetComponent<RelaxPuzzleLevelManager>();
        ShadowModeLevelManager shadowModeLevelManager = GameObject.Find("Canvas").GetComponent<ShadowModeLevelManager>();
        LevelManagerThreeMatch levelManagerThreeMatch = GameObject.Find("Canvas").GetComponent<LevelManagerThreeMatch>();
        LevelManagerEscapeMode levelManagerEscapeMode = GameObject.Find("Canvas").GetComponent<LevelManagerEscapeMode>();

        if (levelManager != null)
        {
            levelManager.WinLevelNotification();
        }
        else if(relaxPuzzleLevelManager!=null)
        {
            relaxPuzzleLevelManager.WinLevelNotification();
        }
        else if (shadowModeLevelManager)
        {
            shadowModeLevelManager.WinLevelNotification();
        }
        else if (levelManagerThreeMatch != null)
        {
            levelManagerThreeMatch.WinLevelNotification();
        }
        else if (levelManagerEscapeMode != null)
        {
            levelManagerEscapeMode.WinLevelNotification();
        }
        else
                {
            print("Next level error!");
        }
    }
    public void LoseGame()
    {
        LevelManager levelManager = GameObject.Find("Canvas").GetComponent<LevelManager>();
        RelaxPuzzleLevelManager relaxPuzzleLevelManager = GameObject.Find("Canvas").GetComponent<RelaxPuzzleLevelManager>();
        ShadowModeLevelManager shadowModeLevelManager = GameObject.Find("Canvas").GetComponent<ShadowModeLevelManager>();
        LevelManagerEndless levelManagerEndless = GameObject.Find("Canvas").GetComponent<LevelManagerEndless>();
        LevelManagerThreeMatch levelManagerThreeMatch = GameObject.Find("Canvas").GetComponent<LevelManagerThreeMatch>();
        LevelManagerEscapeMode levelManagerEscapeMode = GameObject.Find("Canvas").GetComponent<LevelManagerEscapeMode>();

        if (levelManager != null)
        {
            levelManager.LoseGameNotification();
        }
        else if (relaxPuzzleLevelManager != null)
        {
            relaxPuzzleLevelManager.LoseGameNotification();
        }
        else if (shadowModeLevelManager)
        {
            shadowModeLevelManager.LoseGameNotification();
        }
        else if (levelManagerEndless)
        {
            levelManagerEndless.LoseGameNotification();
        }
        else if (levelManagerThreeMatch != null)
        {
            levelManagerThreeMatch.LoseGame();
        }
        else if (levelManagerEscapeMode != null)
        {
            levelManagerEscapeMode.LoseGameNotification();
        }
        else
                {
            print("Lose game error ");
        }
    }

    public void ResetAllValueToDefault()
    {
        PlayerPrefs.DeleteAll();
        SetDefaulValuePlayerPrefs();
        SceneManager.LoadScene("HomeScene");
    }

    private void SetDefaulValuePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();

        if (!PlayerPrefs.HasKey("currentDifficultLevel"))
        {
            PlayerPrefs.SetString("currentDifficultLevel", "Easy");
        }
        if (!PlayerPrefs.HasKey("remainingHP"))
        {
            PlayerPrefs.SetInt("remainingHP", 10);
        }
        if (!PlayerPrefs.HasKey("remainingFind"))
        {
            PlayerPrefs.SetInt("remainingFind", 10);
        }
        if (!PlayerPrefs.HasKey("remainingShuffle"))
        {
            PlayerPrefs.SetInt("remainingShuffle", 10);
        }
        if (!PlayerPrefs.HasKey("currentLevelHard"))
        {
            PlayerPrefs.SetInt("currentLevelHard", 1);
        }
        if (!PlayerPrefs.HasKey("currentLevelMedium"))
        {
            PlayerPrefs.SetInt("currentLevelMedium", 1);
        }
        if (!PlayerPrefs.HasKey("currentLevelEasy"))
        {
            PlayerPrefs.SetInt("currentLevelEasy", 1);
        }
    }

    public void UpdateDataMatrix()
    {
        GameObject panel= GameObject.Find("Panel");
        if(panel != null)
        {
            ListCellController listCellController = panel.GetComponent<ListCellController>();
            listCellController.DevModeUpdateDataMatrix();
        }
    }

}

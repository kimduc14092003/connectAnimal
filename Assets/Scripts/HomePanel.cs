using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomePanel : MonoBehaviour
{
    public int defaultHP;
    public int defaultFind;
    public int defaultShuffle;
    public Image backgroundSprite;

    public Sprite[] listSpriteBg;

    public GameObject themePanel, homePanel, moreGamePanel, relaxModePanel;
    [SerializeField]
    private GameObject SettingPanel;
    private int bgSpriteIndex;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        SetDefaulValuePlayerPrefs();
        StaticData.SetDefaultValueThemeColor();
    }

    private void SetDefaulValuePlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("currentDifficultLevel"))
        {
            PlayerPrefs.SetString("currentDifficultLevel", "Easy");
        }
        if (!PlayerPrefs.HasKey("remainingHP"))
        {
            PlayerPrefs.SetInt("remainingHP", defaultHP);
        }
        if (!PlayerPrefs.HasKey("remainingFind"))
        {
            PlayerPrefs.SetInt("remainingFind", defaultFind);
        }
        if (!PlayerPrefs.HasKey("remainingShuffle"))
        {
            PlayerPrefs.SetInt("remainingShuffle", defaultShuffle);
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

    private void Start()
    {
        bgSpriteIndex = PlayerPrefs.GetInt("bgSpriteIndex", 0);
        backgroundSprite.sprite = listSpriteBg[bgSpriteIndex];
        AudioManager.Instance.PlayMusic("HomeTheme");
        AudioManager.Instance.GetRandomMusicToList();

    }

    public void OpenSettingPanel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        SettingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        SettingPanel.SetActive(false);
    }
    public void OpenMoreGamePanel()
    {
        homePanel.SetActive(false);
        moreGamePanel.SetActive(true);
    }
    public void CloseMoreGamePanel()
    {
        homePanel.SetActive(true);
        moreGamePanel.SetActive(false);
    }

    public void ChangeBackground()
    {
        AudioManager.Instance.PlaySFX("click_button");
        bgSpriteIndex++;
        if (bgSpriteIndex >= listSpriteBg.Length)
        {
            bgSpriteIndex = 0;
        }
        backgroundSprite.sprite = listSpriteBg[bgSpriteIndex];
        PlayerPrefs.SetInt("bgSpriteIndex", bgSpriteIndex);
        StaticData.SetDefaultValueThemeColor();
    }
    public void PlayGame()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "ClassicMode");
        SceneManager.LoadScene("PlayScene");
    }

    public void OpenEndlessMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        SceneManager.LoadScene("MoreGameScene");
    }

    public void OpenChallengeMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "ChallengeMode");
        SceneManager.LoadScene("PlayScene");
    }

    public void OpenRandomMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "RelaxRandomMode");
        SceneManager.LoadScene("PlayScene");
    }

    public void OpenRelaxPuzzleMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "RelaxPuzzleMode");
        SceneManager.LoadScene("RelaxPuzzleScene");
    }

    public void OpenRelaxModePanel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        moreGamePanel.SetActive(false );
        relaxModePanel.SetActive(true);
    }
    public void CloseRelaxModePanel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        relaxModePanel.SetActive(false);
        moreGamePanel.SetActive(true );
    }
    public void OpenShadowMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "ShadowMode");
        SceneManager.LoadScene("ShadowModeScene");
    }
    public void OpenButterflyMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "ButterflyMode");
        SceneManager.LoadScene("ShadowModeScene");
    }
    public void OpenThreeMatchMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "ThreeMatchMode");
        SceneManager.LoadScene("ThreeMatchModeScene");
    }

    public void OpenEscapeMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "EscapeMode");
        SceneManager.LoadScene("EscapeMode");
    }
    public void OpenThemePanel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        themePanel.SetActive(true);
        homePanel.SetActive(false);
    }
    public void CloseThemePanel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        homePanel.SetActive(true);
        themePanel.SetActive(false);
    }

    public void NoAdsButton()
    {
        AudioManager.Instance.PlaySFX("click_button");
        Debug.Log("No Ads!");
    }
}

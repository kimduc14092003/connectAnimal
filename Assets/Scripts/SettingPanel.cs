using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [Header("Change Theme")]
    public Sprite[] listBgContent;
    public Sprite[] listBgTicker;
    public TMP_Text[] labelTextNeedChangeColor;
    public Image[] listImgNeedChangeColor;
    public Image[] listImgChangeBg;

    public ToggleGroup toggleGroup;
    public Slider musicSlider, sfxSlider;
    private string difficultLevel;

    private void OnEnable()
    {
        difficultLevel = PlayerPrefs.GetString("currentDifficultLevel");
        SetDefaultToggle();
        SetDefaultSlider();
        SetDefaultTheme();
    }

    private void SetDefaultTheme()
    {
        int currentIndexBg = PlayerPrefs.GetInt("bgSpriteIndex", 0);
        for (int i = 0; i < labelTextNeedChangeColor.Length; i++)
        {
            labelTextNeedChangeColor[i].color = StaticData.ThemeColor;
        }
        for(int i = 0; i < listImgNeedChangeColor.Length; i++)
        {
            listImgNeedChangeColor[i].color= StaticData.ThemeColor;
        }
        for (int i = 0; i < listImgChangeBg.Length; i++)
        {
            listImgChangeBg[i].sprite = listBgTicker[currentIndexBg];
        }
        transform.GetChild(0).GetComponent<Image>().sprite = listBgContent[currentIndexBg];
    }

    private void SetDefaultToggle()
    {
        for(int i = 0;i<toggleGroup.gameObject.transform.childCount;i++)
        {
            Toggle toggle= toggleGroup.gameObject.transform.GetChild(i).GetComponent<Toggle>();
            if (toggle.name == difficultLevel)
            {
                toggle.isOn = true;
            }
            else
            {
                toggle.isOn = false;
            }
        }
    }

    private void SetDefaultSlider()
    {
        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value= AudioManager.Instance.sfxSource.volume;
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }
    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(sfxSlider.value);
    }

    public void PlayGame()
    {
        SetDifficultLevel();
        SceneManager.LoadScene("PlayScene");
    }

    private void SetDifficultLevel()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        switch (toggle.name)
        {
            case "Easy":
                {
                    PlayerPrefs.SetString("currentDifficultLevel", "Easy");
                    break;
                }
            case "Medium":
                {
                    PlayerPrefs.SetString("currentDifficultLevel", "Medium");
                    break;
                }
            case "Hard":
                {
                    PlayerPrefs.SetString("currentDifficultLevel", "Hard");
                    break;
                }
        }
    }

    public void CloseSettingPanel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        SetDifficultLevel();
        gameObject.SetActive(false);
    }

    public void EffectClickChangeDifficult()
    {
        AudioManager.Instance.PlaySFX("click_button");
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseGamePanel : MonoBehaviour
{
    [Header("Change Theme")]
    public Sprite[] listBgContent;
    public Sprite[] listBgSprites;
    public Image[] listImgNeedChangeColor;
    public Image[] listImgNeedChangeSprite;

    public Slider musicSlider, sfxSlider;

    private void OnEnable()
    {
        StaticData.SetDefaultValueThemeColor();
        SetDefaultSlider();
        SetDefaultTheme();
    }

    private void SetDefaultSlider()
    {
        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
    }

    private void SetDefaultTheme()
    {
        int currentIndexBg = PlayerPrefs.GetInt("bgSpriteIndex", 0);
        for (int i = 0; i < listImgNeedChangeColor.Length; i++)
        {
            listImgNeedChangeColor[i].color = StaticData.ThemeColor;
            
        }
        for(int i = 0; i < listImgNeedChangeSprite.Length; i++)
        {
            listImgNeedChangeSprite[i].sprite = listBgSprites[currentIndexBg];
        }
        transform.GetChild(0).GetComponent<Image>().sprite = listBgContent[currentIndexBg];
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

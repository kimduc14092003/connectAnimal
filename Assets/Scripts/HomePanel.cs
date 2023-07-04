using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
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

    public List<BackgroundSprite> listSpriteBg;
    public LoadSceneManager loadSceneManager;
    public GameObject themePanel, homePanel, moreGamePanel, relaxModePanel, removeAdsPanel,removeAdsButton,notificationPanel;
    public MG_Interface mG_Interface;
    [SerializeField]
    private GameObject SettingPanel;
    private int bgSpriteIndex;

    [SerializeField] private SkeletonGraphic skeletonGraphic;

    private void Awake()
    {
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
        //bgSpriteIndex = PlayerPrefs.GetInt("bgSpriteIndex", 0);
        ChangeBackgroundToBackgroundName("bgHome");
        AudioManager.Instance.PlayMusic("HomeTheme");
        AudioManager.Instance.GetRandomMusicToList();
        if (mG_Interface.removeAds)
        {
            removeAdsButton.SetActive(false);
        }

        skeletonGraphic = GetComponent<SkeletonGraphic>();
    }

    void OnRectTransformDimensionsChange()
    {
        // Lấy kích thước màn hình hiện tại
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        // Áp dụng tỷ lệ zoom cho SkeletonGraphic
        try
        {
            skeletonGraphic.transform.localScale = new Vector3((screenSize.x*9)/(screenSize.y*16), 1f, 1f);
        }
        catch
        {
            Debug.Log("Change size Button Play fail!");
        }
    }

    private void ChangeBackgroundToBackgroundName(string bgName)
    {
        for (int i = 0; i < listSpriteBg.Count; i++)
        {
            if (listSpriteBg[i].name == bgName)
            {
                backgroundSprite.sprite = listSpriteBg[i].sprite;
                break;
            }
        }
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
        AudioManager.Instance.PlaySFX("click_button");
        ChangeBackgroundToBackgroundName("bgOtherGame");
        homePanel.SetActive(false);
        moreGamePanel.SetActive(true);
    }
    public void CloseMoreGamePanel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        ChangeBackgroundToBackgroundName("bgHome");

        homePanel.SetActive(true);
        moreGamePanel.SetActive(false);
    }

    public void ChangeBackground()
    {
        AudioManager.Instance.PlaySFX("click_button");
        bgSpriteIndex++;
        if (bgSpriteIndex >= listSpriteBg.Count)
        {
            bgSpriteIndex = 0;
        }
        backgroundSprite.sprite = listSpriteBg[bgSpriteIndex].sprite;
        PlayerPrefs.SetInt("bgSpriteIndex", bgSpriteIndex);
        StaticData.SetDefaultValueThemeColor();
    }
    public void PlayGameClassicMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "ClassicMode");
        loadSceneManager.LoadScene("PlayScene");
    }

    public void OpenEndlessMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        loadSceneManager.LoadScene("MoreGameScene");
    }

    public void OpenChallengeMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "ChallengeMode");
        loadSceneManager.LoadScene("PlayScene");
    }

    public void OpenRandomMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "RelaxRandomMode");
        loadSceneManager.LoadScene("PlayScene");
    }

    public void OpenRelaxPuzzleMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "RelaxPuzzleMode");
        loadSceneManager.LoadScene("PlayScene");
    }

    public void OpenRelaxModePanel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        moreGamePanel.SetActive(false);
        relaxModePanel.SetActive(true);
    }
    public void CloseRelaxModePanel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        relaxModePanel.SetActive(false);
        moreGamePanel.SetActive(true);
    }
    public void OpenShadowMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "ShadowMode");
        loadSceneManager.LoadScene("ShadowModeScene");
    }
    public void OpenButterflyMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "ButterflyMode");
        loadSceneManager.LoadScene("ShadowModeScene");
    }
    public void OpenThreeMatchMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "ThreeMatchMode");
        loadSceneManager.LoadScene("ThreeMatchModeScene");
    }

    public void OpenEscapeMode()
    {
        AudioManager.Instance.PlaySFX("click_button");
        PlayerPrefs.SetString("PlayeMode", "EscapeMode");
        loadSceneManager.LoadScene("EscapeMode");
    }
    public void OpenThemePanel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        themePanel.transform.SetSiblingIndex(1);
        themePanel.SetActive(true);
        homePanel.SetActive(false);
        ChangeBackgroundToBackgroundName("bgOtherGame");
    }
    public void CloseThemePanel()
    {
        AudioManager.Instance.PlaySFX("click_button");
        homePanel.SetActive(true);
        themePanel.SetActive(false);
        ChangeBackgroundToBackgroundName("bgHome");
    }
    public void StateNotificationPanel(bool isOpen)
    {
        AudioManager.Instance.PlaySFX("click_button");
        if (isOpen)
        {
            notificationPanel.SetActive(true);
        }
        else
        {
            notificationPanel.SetActive(false);
        }
    }

    public void StateRemoveAdsPanelButton(bool isOpen)
    {
        AudioManager.Instance.PlaySFX("click_button");
        if (isOpen)
        {
            removeAdsPanel.SetActive(true);
        }
        else
        {
            removeAdsPanel.SetActive(false);
        }
    }

    public void ConfirmBuyRemoveAds()
    {
        mG_Interface.Purchase_Item(MG_ProductData.NoAds_Pack.productId, HandleBuyRemoveAds);
    }

    private void HandleBuyRemoveAds(bool result,bool onIAP,string productID)
    {
        if(result)
        {
            MG_PlayerPrefs.SetBool("RemoveAds", true);
            mG_Interface.removeAds = true;
            StateRemoveAdsPanelButton(false);
            removeAdsButton.SetActive(false);
            StateNotificationPanel(true);
        }
    }
}
[System.Serializable]
public class BackgroundSprite
{
    public string name;
    public Sprite sprite;
}

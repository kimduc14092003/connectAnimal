using UnityEngine;
using System;
using System.Collections.Generic;
#if LEADER
using UnityEngine.Purchasing;
using pingak9;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.Analytics;
#endif

public class MG_Interface : MonoBehaviour {

	public static MG_Interface Current;
    private event Action<bool> OnRewardLoaded = delegate { };
    private event Action<bool, bool, string> OnPurchaseResult = delegate { };
    private event Action<bool> OnRestoreCompleted = delegate { };
    [HideInInspector]
    public bool RewardLoaded;
    [HideInInspector]
    public bool backupAds;
    [HideInInspector]
    public int maxRewardPerDay;
    private int numRewardToday;
    [SerializeField]
    private float StepTimeAds;
    private float lastTimeShowAd;
    [HideInInspector]
    public string Version, UpdateNote;
    public string linkPage, linkPrivacy;
    [Header("Android App Info")]
    #if LEADER
    public ObscuredString PackageName;
    #endif
    public string linkMoreAndroid;
    [Header("IOS App Info")]
    public string IosAppId;
    public string linkMoreIos;
    [HideInInspector]
    public bool notifyState;
    [HideInInspector]
    public bool removeAds;
    [Header("Dev Control")]
    public GameObject reporter;
    public bool devMode;
    public bool testAds;
    private int devCount, devCount2;
    [HideInInspector]
    public int firstPageId;
    [HideInInspector] 
    public bool rated;
    [HideInInspector] 
    public bool isFromStore;
    [HideInInspector] 
    public bool isFirstIAP, isRestored;
    private bool onIAP, onRestoring;
    private bool isRewardPlaying;
    void Awake()
    {
        Application.targetFrameRate = 60;
        devCount = 0;
        devCount2 = 0;
#if UNITY_EDITOR
        isFromStore = false;
#else
        #if UNITY_ANDROID
                isFromStore = Application.installerName == "com.android.vending";
        #elif UNITY_IOS
                isFromStore = true;
        #endif
#endif
        if (Current)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Current = this;
            DontDestroyOnLoad(gameObject);
        }
#if LEADER && UNITY_ANDROID
        if (Application.identifier != PackageName)
        {
            Application.Quit();
        }
        //Debug.unityLogger.logEnabled = false;
#endif
        
        Init();
    }

    private void Start()
    {
#if LEADER
        Invoke("InitAds", 0.1f);
        Invoke("ShowUpdateDialog", 3f);
#endif
        CheckDevMode();
    }

#region MSService ---------------------------------

    private void Init()
    {
        isRestored = MG_PlayerPrefs.GetBool("isRestored", false);
        isFirstIAP = MG_PlayerPrefs.GetBool("isFirstIAP", true);
        notifyState = MG_PlayerPrefs.GetBool("isNotify", true);
        removeAds = MG_PlayerPrefs.GetBool("RemoveAds", false);
        rated = MG_PlayerPrefs.GetBool("isRated", false);
        if (checkNewDay())
        {
            numRewardToday = 0;
            PlayerPrefs.SetInt("numRewardToday", 0);
        }
        else
        {
            numRewardToday = PlayerPrefs.GetInt("numRewardToday", 0);
        }
    }

    private bool checkNewDay()
    {
        if (PlayerPrefs.GetInt("dayofyear", 0) < UnbiasedTime.Instance.Now().DayOfYear)
        {
            PlayerPrefs.SetInt("dayofyear", UnbiasedTime.Instance.Now().DayOfYear);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ShowUpdateDialog()
    {
        // TrackEvent("InstallSource", Application.installerName);
        //Debug.Log("Version: " + Version);
        //Debug.Log("Update: " + UpdateNote);
        // Debug.Log("MaxAds: " + maxRewardPerDay);
#if LEADER
        if (String.Compare(Application.version, Version, true) < 0)
        {
            string[] line = UpdateNote.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var title = "Update Required";
            var message = Application.productName + " " + "new version available" + "!\r\n----------------------------------";
            foreach (string str in line)
            {
                message += "\r\n       " + str;
            }

            NativeDialog.OpenDialog(title, message, "UPDATE NOW", "LATER",
            () =>
            {
#if UNITY_ANDROID
                Application.OpenURL("market://details?id=" + Application.identifier);

#elif UNITY_IOS
                Application.OpenURL ("https://itunes.apple.com/us/app/id" + IosAppId);
#endif
            },
            () =>
            {
                Debug.Log("Later button pressed");
            });

        }
#endif
    }

    public void TrackEvent(string eventName)
    {
    #if LEADER
        Analytics.CustomEvent(eventName);
    #endif
    }
    
    public void TrackEvent(string eventName,int value)
    {
#if LEADER
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["Value"] = value;
        Analytics.CustomEvent(eventName, parameters);
#endif
    }
    
    public void TrackEvent(string eventName,string value)
    {
#if LEADER
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["Value"] = value;
        Analytics.CustomEvent(eventName, parameters);
#endif
    }

    public void TrackEvent(string eventName, Dictionary<string, object> parameters)
    {
#if LEADER
        Analytics.CustomEvent(eventName, parameters);
#endif
    }

    public void ShowRateUsDialog()
    {
#if LEADER
        string appName = Application.productName;
        string title = string.Format("{0} {1}", appName, "Rating!");
		string message = string.Format("{0} {1}, {2}", "----------------------------------\r\n" + "If you enjoy", appName, "please take a moment to rate it.\r\nThanks for your support!");
        NativeDialog.OpenDialog(title, message, "RATE NOW", "LATER",
            () =>
            {
                OnRate();
            },
            () =>
            {
                Debug.Log("Later button pressed");
            });
#endif
    }

    public void OnFanPage()
    {
        Application.OpenURL(linkPage);

    }

    public void OnPrivacy()
    {
        Application.OpenURL(linkPrivacy);

    }

    public void OnRate()
    {
        rated = true;
        MG_PlayerPrefs.SetBool("isRated", true);
#if UNITY_IOS
        if(!UnityEngine.iOS.Device.RequestStoreReview())
            Application.OpenURL ("https://itunes.apple.com/us/app/id" + IosAppId);
#elif UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.identifier);
#endif
    }

    public void OnMoreGame()
    {
#if UNITY_IOS
        Application.OpenURL (linkMoreIos);
#elif UNITY_ANDROID
        Application.OpenURL(linkMoreAndroid);
#endif
    }

    public void NativeShare()
    {
#if NATIVE_SHARE
        NativeShare NS = new NativeShare();
        NS.SetSubject(Application.productName);
#if UNITY_ANDROID
        NS.SetText("https://play.google.com/store/apps/details?id=" + Application.identifier);
#elif UNITY_IOS
        NS.SetText("https://itunes.apple.com/us/app/id" + IosAppId);
#endif
        NS.SetTitle(Application.productName);
        //NS.AddFile(filePath);
        NS.Share();
#endif
    }
    
    public void GotoDevMode(){
        devCount ++;
    }
    public void GotoDevMode2()
    {
        devCount2++;
    }
    public void CheckDevMode(){
        if (devMode)
        {
            GetComponent<MG_FPS> ().enabled = true;
            reporter.SetActive (true);
        }
        else
        {
            if (devCount == 7 && devCount2 == 3) {
                devMode = true;
                GetComponent<MG_FPS> ().enabled = true;
                reporter.SetActive (true);
            } else {
                devMode = false;
                GetComponent<MG_FPS> ().enabled = false;
                reporter.SetActive (false);
            }
        }
    }
    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Reset Data");
    }
    public void Cheat()
    {
        Debug.Log("Cheating Funcsion");
    }
#endregion

#region Advertisement ---------------------------------

    private void InitAds()
    {
    #if LEADER
        if (isFromStore)
        {
            if (backupAds || testAds)
            {
                MG_Ads_Backup.Current.Reward_Loaded += RewardAdsLoaded;
            }
            else
            {
                MG_Ads.Current.Reward_Loaded += RewardAdsLoaded;
            }
        }
        else
        {
            MG_Ads_Backup.Current.Reward_Loaded += RewardAdsLoaded;
        }

    #endif
    }
    public void Banner_Show()
    {
#if LEADER
        MG_Ads.Current.Banner_Show();
#endif
    }

    public void Banner_Hide()
    {
#if LEADER
        MG_Ads.Current.Banner_Hide();
#endif
    }

    public void Banner_Reload()
    {
#if LEADER
        MG_Ads.Current.Banner_Request();
#endif
    }

    public void Interstitial_Show()
    {
        Debug.Log("Interstitial_Show");
#if LEADER
        if (!removeAds && Time.time - lastTimeShowAd >= StepTimeAds)
        {
            lastTimeShowAd = Time.time;
            if (isFromStore)
            {
                if (backupAds || testAds)
                {
                    MG_Ads_Backup.Current.ShowIntersBackup();
                }
                else
                {
                    MG_Ads.Current.Interstitial_Show();
                }
            }
            else
            {
                MG_Ads_Backup.Current.ShowIntersBackup();
            }
        }
#endif
	}

    public void Interstitial_Reload()
    {
#if LEADER
        MG_Ads.Current.Interstitial_Request();
#endif
    }

    public bool canRewardView()
    {
        if (maxRewardPerDay == -1 || numRewardToday < maxRewardPerDay)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    private void RewardAdsLoaded(bool stt)
    {
        RewardLoaded = stt;
        OnRewardLoaded(stt);
    }

    public void Reward_Show(Action<bool> callback)
    {
        if(isRewardPlaying) return;
        isRewardPlaying = true;
        callback += RewardCallBack;
#if LEADER
        if (canRewardView())
        {
            if (isFromStore)
            {
                if (backupAds || testAds)
                {
                    MG_Ads_Backup.Current.ShowRewardBackup(callback);
                }
                else
                {
                    MG_Ads.Current.Reward_Show(callback);
                }
            }
            else
            {
                MG_Ads_Backup.Current.ShowRewardBackup(callback);
            }
        }
        else
        {
            callback(false);
        }
#else
            callback(true);
            Debug.Log("Reward_Show");
#endif

    }

    private void RewardCallBack(bool stt)
    {
        isRewardPlaying = false;
        if (stt)
        {
            numRewardToday++;
            PlayerPrefs.SetInt("numRewardToday", numRewardToday);
        }
    }
     

    public void Reward_Reload()
    {
#if LEADER
        MG_Ads.Current.Reward_Request();
#endif
    }

    public void RegisterAdsAvailableCallback(Action<bool> callback)
    {
        OnRewardLoaded += callback;
    }

    public void UnRegisterAdsAvailableCallback(Action<bool> callback)
    {
        OnRewardLoaded -= callback;
    }

#endregion

#region InApp-Purchase ---------------------------------

    public void Purchase_Item(string productId, Action<bool, bool, string> callback)
	{
        if (!onIAP && !onRestoring)
        {
            onIAP = true;
            OnPurchaseResult = callback;
#if LEADER
            if (devMode)
            {
                devSuccsseResult(MG_IAP.Current.PurchaseDevMode(productId), productId);
            }
            else
            {
                MG_IAP.Current.Purchase(productId);
            }
#else
            devSuccsseResult(true, productId);
#endif
        }

    }

    private void devSuccsseResult(bool result, string productId)
    {
        // if (MG_Interface.Current.isFirstIAP)
        // {
        //     MG_Interface.Current.isFirstIAP = false;
        //     MG_PlayerPrefs.SetBool("isFirstIAP", false);
        //     MG_Interface.Current.isRestored = true;
        //     MG_PlayerPrefs.SetBool("isRestored", true);
        // }
        OnPurchaseResult(result,onIAP, productId);
        onIAP = false;
        if (result)
        {
            Debug.Log("MSS Purchase: SUSSCCES " + productId);
        }
        else
        {
            Debug.LogError("MSS Purchase: ERROR " + productId);
        }
    }

#if LEADER
    public void PurchaseSuccsseCallBack(Product product)
    {
        OnPurchaseResult(true,onIAP, product.definition.id);
        if (onIAP)
        {
            Debug.Log("MSS Purchase: SUSSCCES " + product.definition.id);
            onIAP = false;
        }
        else
        {
            Debug.Log("MSS Restore: SUSSCCES " + product.definition.id);
        }
    }

	public void PurchaseErrorCallBack(Product product, PurchaseFailureReason reason)
	{
        if (product == null)
        {
            OnPurchaseResult(false,onIAP, null);
            if (onIAP) onIAP = false;
            if (onRestoring) onRestoring = false;
            Debug.Log("MSS Purchase: INIT ERROR");
        }
        else
        {
            OnPurchaseResult(false,onIAP, product.definition.id);
            if (onIAP)
            {
                Debug.Log("MSS Purchase: ERROR " + product.definition.id);
                onIAP = false;
            }
            else
            {
                Debug.Log("MSS Restore: ERROR " + product.definition.id);
            }
        }
    }
#endif
    public void RestorePurchase(Action<bool, bool, string> procesAction, Action<bool> completedAction)
    {
#if LEADER && UNITY_IOS
        if (!onRestoring && !onIAP)
        {
            onRestoring = true;
            OnPurchaseResult = procesAction;
            OnRestoreCompleted = completedAction;
            MS_IAP.Current.RestorePurchase(OnRestoreCallback);
        }
#endif
    }
    void OnRestoreCallback(bool success)
    {
        if (success)
        {
            isRestored = true;
            MG_PlayerPrefs.SetBool("isRestored", true);
        }
        OnRestoreCompleted(success);
        onRestoring = false;
    }
#endregion
}

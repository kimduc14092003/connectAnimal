using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoreGameItemController : MonoBehaviour
{
    public string nameSceneToLoad;
    public string modeName;
    private GameObject rootGameObjectParent;
    public LoadSceneManager loadSceneManager;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenGameMode);
        rootGameObjectParent = transform.parent.root.gameObject;
    }

    private void OpenGameMode()
    {
        if(nameSceneToLoad == "RelaxMode")
        {
            if(rootGameObjectParent != null)
            {
                HomePanel homePanel = rootGameObjectParent.GetComponent<HomePanel>();
                if(homePanel != null)
                {
                    homePanel.OpenRelaxModePanel();
                }
            }
            else
            {
                Debug.Log(rootGameObjectParent + " is null");
            }
        }
        else
        if (nameSceneToLoad != null)
        {
            PlayerPrefs.SetString("PlayeMode", modeName);
            loadSceneManager =rootGameObjectParent.GetComponent<LoadSceneManager>();
            loadSceneManager.LoadScene(nameSceneToLoad);
        }
    }

}

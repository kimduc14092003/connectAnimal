using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayModeManager : MonoBehaviour
{
    public Component[] listComponent;
    public LoadSceneManager loadSceneManager;

    void Awake()
    {
         Application.targetFrameRate = 144;
        string currentGameMode = PlayerPrefs.GetString("PlayeMode");

        switch (currentGameMode)
        {
            case "RelaxPuzzleMode":
                {
                    gameObject.GetComponent<RelaxPuzzleLevelManager>().enabled = true;
                    RemoveComponentNotUsing(2);
                    break;
                }

            case "EndlessMode":
                {
                    gameObject.GetComponent<LevelManagerEndless>().enabled = true;
                    RemoveComponentNotUsing(1);
                    break;
                }
            case "ShadowMode":
            case "ButterflyMode":
                {
                    gameObject.GetComponent<ShadowModeLevelManager>().enabled = true;
                    RemoveComponentNotUsing(3);
                    break;
                }
            case "ClassicMode":
            case "ChallengeMode":
            case "RelaxRandomMode":
                {
                    gameObject.GetComponent<LevelManager>().enabled = true;
                    RemoveComponentNotUsing(0);
                    break;
                }
        }
    }


    private void RemoveComponentNotUsing(int index)
    {
        for (int i = 0; i < listComponent.Length; i++)
        {
            if (i == index)
            {
                continue;
            }
            Destroy(listComponent[i]);
        }
    }

    public void ReturnHomeScene()
    {
        AudioManager.Instance.PlaySFX("click_button");
        loadSceneManager.LoadScene("HomeScene");
    }
}

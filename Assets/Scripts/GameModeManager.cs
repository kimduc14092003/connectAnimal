using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{

    void Awake()
    {
        string currentGameMode = PlayerPrefs.GetString("PlayeMode");

        switch (currentGameMode)
        {
            case "RelaxPuzzleMode":
                {
                    gameObject.GetComponent<RelaxPuzzleLevelManager>().enabled = true;
                    break;
                }

            case "EndlessMode":
                {
                    gameObject.GetComponent<LevelManagerEndless>().enabled = true;
                    break;
                }
            case "ShadowMode":
            case "ButterflyMode":
                {
                    gameObject.GetComponent<ShadowModeLevelManager>().enabled = true;
                    break;
                }
            case "ClassicMode":
            case "ChallengeMode":
            case "RelaxRandomMode":
                {
                    gameObject.GetComponent<LevelManager>().enabled = true;
                    break;
                }

        }

    }

}

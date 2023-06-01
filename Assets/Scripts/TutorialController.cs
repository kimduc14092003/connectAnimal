
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject[] listPanelTutorial;
    public LineController lineController;
    public LevelManager levelManager;
  
    private void OnEnable()
    {
        Instantiate(listPanelTutorial[0],transform);
    }
    
    public void OpenPanelTutorial2()
    {
        Instantiate(listPanelTutorial[1],transform);

    }
    public void OpenPanelTutorial3()
    {
        Instantiate(listPanelTutorial[2], transform);
    }
    public void OpenPanelTutorialDone()
    {
        levelManager.PausePanel.SetActive(true);
        gameObject.SetActive(false);
    }

}

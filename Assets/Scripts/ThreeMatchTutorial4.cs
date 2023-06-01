using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ThreeMatchTutorial4 : MonoBehaviour
{
    private int remainingCell;
    public GameObject tutorialPanel;
    public GameObject[] listLayer;
    private bool isDoneTutorial;
    private GameObject helpPanel;

    // Start is called before the first frame update
    void Start()
    {
        helpPanel = transform.parent.GetComponent<LevelManagerThreeMatch>().helpPanel;
        PreventPlayerDoOther();

        for(int i = 0; i < helpPanel.transform.childCount; i++)
        {
            if (i == 2)
            {
                helpPanel.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
                {
                    isDoneTutorial = true;
                    transform.parent.GetComponent<LevelManagerThreeMatch>().ReCheckAllCell();
                    tutorialPanel.SetActive(false);
                    isDoneTutorial = true;
                    this.enabled = false;
                });
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDoneTutorial)
        {
            PreventPlayerDoOther();
        }
        remainingCell = transform.parent.GetComponent<LevelManagerThreeMatch>().remainingCell;
        
    }

    private void PreventPlayerDoOther()
    {
        for (int j = 0; j < listLayer.Length; j++)
        {
            for (int i = 0; i < listLayer[j].transform.childCount; i++)
            {
                CellControllerThreeMatch cellControllerThreeMatch = listLayer[j].transform.GetChild(i).GetComponent<CellControllerThreeMatch>();
                if (cellControllerThreeMatch)
                {
                    cellControllerThreeMatch.isDisable = true;
                }
            }
        }
    }
}

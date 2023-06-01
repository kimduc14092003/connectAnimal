using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ThreeMatchTutorial2 : MonoBehaviour
{
    private int remainingCell;
    public GameObject tutorialPanel;
    public GameObject[] listLayer;
    public bool isDoneTutorial;
    private GameObject helpPanel;

    // Start is called before the first frame update
    void Start()
    {
        helpPanel = transform.parent.GetComponent<LevelManagerThreeMatch>().helpPanel;
        PreventPlayerDoOther();

        for(int i = 0; i < helpPanel.transform.childCount; i++)
        {
            if(i>0)
            {
                helpPanel.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        remainingCell = transform.parent.GetComponent<LevelManagerThreeMatch>().remainingCell;
        if(remainingCell <= 30)
        {
            this.enabled=false;
            tutorialPanel.SetActive(false);
        }
        if (!isDoneTutorial)
        {
            PreventPlayerDoOther();
        }
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

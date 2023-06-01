using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ThreeMatchTutorial1 : MonoBehaviour
{
    private bool isDoneTutorial;
    public GameObject[] listLayer;
    private int remainingCell;
    public GameObject pointGameObject;
    public Button[] buttons;
    public GameObject tutorialPanel;

    // Start is called before the first frame update
    void Start()
    {
        isDoneTutorial = false;
        pointGameObject.transform.DOScale(1.2f, 0.3f).SetLoops(-1,LoopType.Yoyo);
    }

    public void MovePointToPos0()
    {
        pointGameObject.transform.DOMove(buttons[1].transform.position, 0.25f);
    }
    public void MovePointToPos1()
    {
        pointGameObject.transform.DOMove(buttons[2].transform.position, 0.25f);
    }
    public void MovePointToPos2()
    {
        pointGameObject.transform.DOMove(buttons[2].transform.position, 0.25f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        remainingCell = transform.parent.GetComponent<LevelManagerThreeMatch>().remainingCell;

        if (!isDoneTutorial)
        {
            PreventPlayerDoOther();
        }
        if (remainingCell <= 18)
        {
            isDoneTutorial = true;
            tutorialPanel.SetActive(false);
            transform.parent.GetComponent<LevelManagerThreeMatch>().ReCheckAllCell();
            this.enabled = false;
        }
    }

    private void PreventPlayerDoOther()
    {
        for(int j=0; j<listLayer.Length; j++)
        {
         for(int i = 0; i < listLayer[j].transform.childCount;i++)
        {
            CellControllerThreeMatch cellControllerThreeMatch= listLayer[j].transform.GetChild(i).GetComponent<CellControllerThreeMatch>();
            if (i >= 6&&j==1)
            {
                cellControllerThreeMatch.isDisable = false;
            }
            else
            {
                cellControllerThreeMatch.isDisable = true;
            }
        }
        }
    }
}
